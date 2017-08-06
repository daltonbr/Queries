using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using YleJson;

/// <summary>
/// This is the buttom used in the SearchPanel
/// </summary>
[RequireComponent(typeof(Button))]
public class TitleButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text titleLabel;
    public Datum datum;   

    private void Awake()
    {
        Assert.IsNotNull(buttonComponent);
        buttonComponent.onClick.AddListener(() => UIManager.Instance.onLoadProgramDetails(datum));
    }

    /// <summary>
    /// fill the label of the buttom and stores the datum for later usages
    /// </summary>
    /// <param name="_datum">the label (title) is passed inside the datum</param>
    public void Setup(Datum _datum)
    {
        this.datum = _datum;
        // validating the string before assigning it
        if (string.IsNullOrEmpty(datum.itemTitle.fi))
        {
            titleLabel.text = "<<Finnish title unavailable>>";
        }
        else
        {
            titleLabel.text = datum.itemTitle.fi;
        }
         
    }
}
