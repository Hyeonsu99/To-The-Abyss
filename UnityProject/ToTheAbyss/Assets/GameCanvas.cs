using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    public List<GameObject> Views = new List<GameObject>();

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnClick(GameObject obj)
    {
        if (obj != null && obj.activeSelf == false)
        {
            obj.SetActive(true);
        }
    }

    public void OnShowPanel(GameObject obj)
    {
        foreach(GameObject go in Views)
        {
            if(go != obj)
            {
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
            }
        }
    }

    public void LoadMiniGameScene()
    {
        SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
    }
}
