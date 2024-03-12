using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
    public Text text;
    public Text text2;

    AndroidJavaObject obj;
    AndroidJavaObject obj2;

    // Start is called before the first frame update
    void Start()
    {
        var pluginClass = new AndroidJavaClass("com.example.plugin.UnityPlugin");

        obj = pluginClass.CallStatic<AndroidJavaObject>("instance");

        var str = obj.Call<string>("returnText");

        text.text = str;
    }
}
