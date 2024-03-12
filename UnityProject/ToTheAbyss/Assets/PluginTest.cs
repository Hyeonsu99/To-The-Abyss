using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        var pluginClass = new AndroidJavaClass("com.example.plugin.UnityPlugin");

        var instance = pluginClass.CallStatic<AndroidJavaObject>("instance");

        var str = instance.Call<string>("returnText");

        text.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
