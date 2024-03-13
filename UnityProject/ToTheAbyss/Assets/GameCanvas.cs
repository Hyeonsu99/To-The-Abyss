using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    public List<GameObject> Views = new List<GameObject>();

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    //public void OnClick(GameObject obj)
    //{
    //    if (obj != null && obj.activeSelf == false)
    //    {
    //        obj.SetActive(true);
    //    }
    //}

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

    public void BuyPeer(GameObject obj)
    {
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "1":
                if(GameManager.Instance.coin > 100)
                {
                    GameManager.Instance.coin -= 100;
                    obj.SetActive(true);
                }
                else
                {
                    return;
                }
                break;
            case "2":
                if (GameManager.Instance.coin > 200)
                {
                    GameManager.Instance.coin -= 200;
                    obj.SetActive(true);
                }
                else
                {
                    return;
                }
                break;
            case "3":
                if (GameManager.Instance.coin > 300)
                {
                    GameManager.Instance.coin -= 300;
                    obj.SetActive(true);
                }
                else
                {
                    return;
                }
                break;
            case "4":
                if (GameManager.Instance.coin > 400)
                {
                    GameManager.Instance.coin -= 400;
                    obj.SetActive(true);
                }
                else
                {
                    return;
                }
                break;
        }
    }

    public void LoadMiniGameScene()
    {
        SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
    }
}
