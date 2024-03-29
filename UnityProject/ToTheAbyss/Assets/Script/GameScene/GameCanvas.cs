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

    // ��ų ��ȭ �־�� ��
    public void Skill1()
    {
        // ���߿� �� float ������ �����ؾ� ��
        int damage = GameManager.Instance.playerDamage * 2;

        Damage(damage);
    }

    // �г� �ÿ��� ������ ���� �ȵǰ� �����ؾ���..
    public void Skill2()
    {
        StartCoroutine(Up());
    }

    void Damage(int damage)
    {
        var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();

        monster.TakeDamage(damage);
    }

    // �г� : ���ݷ� ���� �ڷ�ƾ
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

    // Ư�� �������� �̻󿡼��� ��ư�� ��ȣ�ۿ� ����� Ȱ��ȭ�ǰ� ����
    public void Rebirth()
    {
        var manager = GameManager.Instance;

        var monsterSpawner = manager.monsterSpawner;

        var peers = manager.peers;

        if (monsterSpawner.GetComponent<MonsterSpawner>().Count >= 5)
        {
            monsterSpawner.GetComponent<MonsterSpawner>().Count = 0;

            IncreaseRebirthCoin(manager.playerAutoDamage);
            IncreaseRebirthCoin(manager.playerDamage);
            IncreaseRebirthCoin(manager.coin / 100);

            for (int i = 0; i < peers.Count; i++)
            {
                var peer = peers[i].GetComponent<Peer>();

                IncreaseRebirthCoin((peer != null) ? peer.Level : 0);
            }

            manager.playerAutoDamage = 5;
            manager.playerDamage = 10;
            manager.coin = 0;

            for (int i = 0; i < peers.Count; i++)
            {
                var peer = peers[i].GetComponent<Peer>();

                peer.Level = 1;
            }

            foreach (GameObject obj in manager.peers)
            {
                obj.SetActive(false);
            }        
        }
    }

    void IncreaseRebirthCoin(int amount)
    {
        var manager = GameManager.Instance;

        manager.RebirthCoin += amount;
    }

    // �̴ϰ��� �� �ҷ��� �� �̺�Ʈ �ý��� ��Ȱ��ȭ ó�� ����� ��..
    public void LoadMiniGameScene()
    {
        if (!isSceneLoaded("MiniGameScene"))
        {
            SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
        }
    }

    private bool isSceneLoaded(string sceneName)
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if(loadedScene.name == sceneName)
            {
                return true;
            }
        }

        return false;
    }
}
