using UnityEngine;
using UnityEngine.UI;
using YleJson;
using UnityEngine.Assertions;

/// <summary>
/// Helper class to manage the Results Panel
/// </summary>
public class ResultsPanel : MonoBehaviour {

    public Text field0;
    public Text field1;
    public Text field2;
    public Text field3;
    public Text field4;

    //private Datum datum;

    private void Awake()
    {
        Assert.IsNotNull(field0, "[ResultsPanel] field0 not set");
        Assert.IsNotNull(field1, "[ResultsPanel] field1 not set");
        Assert.IsNotNull(field2, "[ResultsPanel] field2 not set");
        Assert.IsNotNull(field3, "[ResultsPanel] field3 not set");
        Assert.IsNotNull(field4, "[ResultsPanel] field4 not set");
    }

    public void Setup(Datum _datum)
    {
        if (_datum != null)
        {
            //this.datum = _datum;

            field0.text = _datum.duration;
            field1.text = _datum.type;
            field2.text = _datum.itemTitle.fi;
            field3.text = _datum.title.fi;
            field4.text = _datum.description.fi;
        }
        else
        {
            Debug.LogWarning("Trying to populate details with null datum");
        }
        
    }
}
