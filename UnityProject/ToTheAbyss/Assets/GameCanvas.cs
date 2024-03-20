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

    // �̴ϰ��� �� �ҷ��� �� �̺�Ʈ �ý��� ��Ȱ��ȭ ó�� ����� ��..
    public void LoadMiniGameScene()
    {
        SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
    }
}
