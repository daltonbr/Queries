using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

/*
 * More Yle API information about images
 * http://developer.yle.fi/tutorial-retrieving-images/index.html
 * 
 * example of this query
 * https://images.cdn.yle.fi/image/upload/{transformations}/{id}.{format}
 */

/// <summary>
/// Loads a image into the ThumbnailPanel
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Image))]
public class ThumbnailPanel : MonoBehaviour
{    
    public string baseUrl = "https://images.cdn.yle.fi/image/upload/w_360,h_240,c_fit/";
    public Image image;

    void Awake()
    {
        Assert.IsNotNull(image, "[ThumbnailPanel] image was null");
    }

    /// <summary>
    /// Wrapper to load a thumbnail
    /// </summary>
    /// <param name="_id">the id from a valid image</param>
    public void LoadThumbnail(string _id)
    {
        string url = baseUrl + _id + ".jpg";
        //Debug.Log(url);
        StartCoroutine(LoadThumbnailCoroutine(url));
    }

    IEnumerator LoadThumbnailCoroutine(string url)
    {
        var www = new WWW(url);       
        yield return www;

        // Create a RGB24 texture
        var texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);

        // Assign the texture into the sprite
        www.LoadImageIntoTexture(texture);
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        image.sprite = spriteToUse;

        www.Dispose();
        www = null;
    }
}