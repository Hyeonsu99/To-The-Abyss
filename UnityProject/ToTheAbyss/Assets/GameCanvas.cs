using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
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

    public void LoadMiniGameScene()
    {
        SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
    }
}
