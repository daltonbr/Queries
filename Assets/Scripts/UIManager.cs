using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using YleJson;
using UnityEngine.Assertions;

/// <summary>
/// Manages the UI Panels. This class uses a Singleton Pattern
/// </summary>
public class UIManager : MonoBehaviour
{
    
    #region 'Variables'
    public static UIManager Instance = null;

    [Header("References")]
    public List<Datum> datum;
    public Transform contentPanel;  
    public ResultsPanel resultsPanel;
    public ThumbnailPanel thumbnailPanel;
    public InputField inputField;
    public Button searchButton;    
    public Button titleButtonPrefab;
    public ScrollRect scrollRect;

        
    [Header("Query Tweaks")]
    public String url = "https://external.api.yle.fi/v1/programs/items.json";
    [Range(1,100)]
    public int maxItemsPerChunk = 10;         // 'limit' in the query
    [Range(0, 3)]
    public float queryCooldown = 1f;          // to avoid spawning too many queries (in seconds)
    public string app_id = "YOUR_APP_ID";
    public string app_key = "YOUR_APP_KEY";
    [Range(0f, 0.5f)]
    public float thresholdScrollRectY = 0.1f; // to detect when the user scrolls downs (normalized value)

    private Items newItems;
    private int offset = 0;
    private bool isQuerying = false;  
    private Meta meta;                        // 'meta' from Json (metadata from the query)
    private string userInput;
    
    #endregion


    // Singleton pattern
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        #region 'Assertions'
        Assert.IsNotNull(contentPanel);
        Assert.IsNotNull(resultsPanel, "[UIManager] resultsPanel was null");
        Assert.IsNotNull(thumbnailPanel, "[UIManager] thumbnailPanel was null");
        Assert.IsNotNull(inputField, "[UIManager] inputField was null");
        Assert.IsNotNull(searchButton, "[UIManager] searchButton was null");
        Assert.IsNotNull(titleButtonPrefab, "[UIManager] titleButtonPrefab was null");
        Assert.IsNotNull(scrollRect, "[UIManager] scrollRect was null");
        #endregion
    }

    void Start()
    {
        searchButton.onClick.AddListener(HandleSearchButton);
    }

    /// <summary>
    /// Listener to check when the user scrolls to the bottom
    /// </summary>
    public void OnScrollRectMove()
    {
        if (scrollRect != null && !isQuerying)
        {
            if (scrollRect.normalizedPosition.y < thresholdScrollRectY)
            {
                //Debug.Log("ScrollRect near bottom");
                if (meta != null)
                {
                    StartCoroutine(MakeQuery());
                }
            }
        }
    }

    /// <summary>
    /// Listener to load from the buttons the resultsPanel (details of a program)
    /// </summary>
    /// <param name="_datum"></param>
    public void onLoadProgramDetails(Datum _datum)
    {
        if (_datum != null)
        {
            resultsPanel.Setup(_datum);

            // Load a thumbnail if we have one
            if (_datum.image.available)
            {
                //Debug.Log("thumbnail available");
                thumbnailPanel.LoadThumbnail(_datum.image.id);
            }            
        }
        else
        {
            Debug.Log("Datum is null!");
        }
        
    }
    
    /// <summary>
    /// Helper method to verify if we have internet connection
    /// </summary>
    /// <returns></returns>
    public bool IsInternetReachable()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("Error. Check internet connection!");
            //TODO: Implement feedback to user when disconnected from the internet
            return false;
        }
        return true;
    }
        

    /// <summary>
    /// Cleans some private variables to make a new query
    /// </summary>
    private void ResetQuery()
    {
        meta = null;
        offset = 0;
        datum.Clear();
        //Debug.Log("Reseting Query");
    }

    public void HandleSearchButton()
    {       
        if (!IsInternetReachable()) return;

        ResetQuery();
        RemoveButtons();
        
        // we just abort the query if the input is invalid (just empty for now)
        userInput = ValidateUserInput(inputField.text);
        if (userInput == null) return;
        
        StartCoroutine(MakeQuery());
    }

    /// <summary>
    /// returns null when the input is invalid
    /// </summary>
    /// <param name="_userInput"></param>
    /// <returns></returns>
    private string ValidateUserInput(string _userInput)
    {
        if (string.IsNullOrEmpty(_userInput))
        {
            Debug.Log("String is null or empty");
            //TODO: Implement feedback to user when input is invalid
            return null;
        }
        //if (userInput.Length < 3)
        //{
        //    Debug.Log("Type at least 3 charactes");
        //    return null;
        //}
        return _userInput;        
    }

    IEnumerator MakeQuery()
    {
        isQuerying = true;

        // When there is no more samples left, we don't even start this Coroutine
        if (IsSampleOver())
        {
            Debug.Log("Sample is Over - Stopping MakeQuery()");
            yield return new WaitForSeconds(queryCooldown);
            isQuerying = false;
            yield break;
        }

        // Check if we don't "overshoot" the sample
        if (CanWeHaveNextOffset())
        {
            NextOffset();
        }
        
        // Querying
        var www = new WWW(url + "?q=" + userInput +
                             /* "&order=playcount.24h:desc" +   */
                             /* "&availability=ondemand&mediaobject=video   */
                                "&offset=" + offset +
                                "&limit=" + maxItemsPerChunk +
                                "&app_key=" + app_key +
                                "&app_id=" + app_id);                           
        
        yield return www;

        // Checking for a valid response
        if (string.IsNullOrEmpty(www.error))
        {
            newItems = JsonConvert.DeserializeObject<Items>(www.text);

            // check for if we really have some objects to work with 
            if (newItems != null && newItems.data.Length > 0)
            {
                meta = newItems.meta;
                
                // check if we don't 'overshoot' the number of programs
                if (datum.Count < meta.count)
                {
                    // append the programs to the List
                    datum.AddRange(newItems.data);
                    AddButtons(newItems.data);
                    Debug.Log("Offset: " + offset + 
                              " - Total clips: " + meta.count +
                              " - Total Programs: " + meta.program +
                              " - ApiVersion: " + newItems.apiVersion);
                }
                Debug.Log("#Datum loaded: " + datum.Count);
            }
            else
            {
                Debug.LogWarning("Can't deserialize Json");
            }
        }
        else
        {
            // Bad Web Responses could be addressed here
            Debug.LogWarning("Bad Web Response: " + www.error);
        }

        yield return new WaitForSeconds(queryCooldown);
        isQuerying = false;
    }

    /// <summary>
    /// Just calculate IF we can have a next offset
    /// </summary>
    /// <returns> If we "overshoot" our sample, returns false</returns>
    public bool CanWeHaveNextOffset()
    {
        if (meta != null && offset < meta.count - maxItemsPerChunk)
        {               
            return true;
        }
        return false;
    }

    /// <summary>
    /// Return if the list of programs ended
    /// </summary>
    /// <returns></returns>
    public bool IsSampleOver()
    {
        if (meta != null && datum.Count >= meta.count)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// If possible set the next offset
    /// </summary>
    private void NextOffset()
    {
        if (meta != null && offset < meta.count - maxItemsPerChunk)
        {
            offset += maxItemsPerChunk;
            Debug.Log("Offset increased");
        }
        else
        {
            Debug.LogWarning("Can't get a new offset! Either out of bounds or there isn't a query in progress");
        }
    }

    /// <summary>
    /// Just destroy all buttons childed in the contentPanel
    /// </summary>
    private void RemoveButtons()
    {
        var childrenToRemove = new List<GameObject>();
        foreach (Transform child in contentPanel.transform) childrenToRemove.Add(child.gameObject);
        childrenToRemove.ForEach(child => Destroy(child));
    }

    /// <summary>
    /// Add buttons (childed in the contentPanel) and set their labels accordingly
    /// </summary>
    /// <param name="_datum">an array of programs to be appended</param>
    public void AddButtons(Datum[] _datum)
    {
        for (int i = 0; i < _datum.Length; i++)
        {
            Button newButton = Instantiate(titleButtonPrefab, this.transform.position, Quaternion.identity, contentPanel);            
            var titleButtonScript = newButton.GetComponent<TitleButton>();
            titleButtonScript.Setup(_datum[i]);
        }
    }

}