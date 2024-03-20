using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    public List<GameObject> Views = new List<GameObject>();

    public bool isUp = false;

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
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

    public void BuyPeer(GameObject obj)
    {
        if(obj.activeInHierarchy == false)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "1":
                    if (GameManager.Instance.coin > 100)
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
    }

    public void UpgradePeer(GameObject obj)
    {
        var peer = obj.GetComponent<Peer>();

        if(obj.activeSelf)
        {
            peer.Level += 1;

            peer.SetDamage();
        }
    }

    public void UpgradePlayer()
    {
        if(GameManager.Instance.coin > 50)
        {
            GameManager.Instance.coin -= 50;

            GameManager.Instance.playerDamage += 1;
            GameManager.Instance.playerAutoDamage += 1;
        }
    }

    // 스킬 강화 넣어야 됨
    public void Skill1()
    {
        // 나중에 다 float 형으로 변경해야 됨
        int damage = GameManager.Instance.playerDamage * 2;

        Damage(damage);
    }

    // 분노 시에는 데미지 저장 안되게 설정해야함..
    public void Skill2()
    {
        StartCoroutine(Up());
    }

    void Damage(int damage)
    {
        var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();

        monster.TakeDamage(damage);
    }

    // 분노 : 공격력 증가 코루틴
    IEnumerator Up()
    {
        if (!isUp)
        {
            isUp = true;

            var a = GameManager.Instance.playerDamage;

            GameManager.Instance.playerDamage = GameManager.Instance.playerDamage * 2;

            yield return new WaitForSeconds(10f);

            GameManager.Instance.playerDamage = a;

            isUp = false;
        }
    }

    // 미니게임 씬 불러올 때 이벤트 시스템 비활성화 처리 해줘야 함..
    public void LoadMiniGameScene()
    {
        SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
    }
}
