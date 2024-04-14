using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeTest : MonoBehaviour
{
    private List<Dictionary<string, object>> AttributeData = null;

    private void Awake()
    {
        AttributeData = CSVRead.Read("Attributes");

        if(AttributeData != null)
        {
            Debug.Log("Àß °¡Á®¿È");
        }

        for(int i = 0; i < AttributeData.Count; i++)
        {
            Debug.Log(AttributeData[i]["Damage"]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
