using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
