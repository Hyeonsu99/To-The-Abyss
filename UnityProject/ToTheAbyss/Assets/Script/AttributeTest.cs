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
    }

    public float GetAttributeDamage(string str1, string str2)
    {
        float Damage = 0f;

        for (int i = 0; i < AttributeData.Count; i++)
        {
            if ((string)AttributeData[i]["Attribute"] == str1 + str2)
            {
                Damage = (float)AttributeData[i]["Damage"];

                return Damage;
            }
        }

        return Damage;
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
