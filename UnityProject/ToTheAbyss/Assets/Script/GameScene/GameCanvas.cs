using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{

    // public variables
    public List<GameObject> Views = new List<GameObject>();

    public Button[] peerBuyBtns;
    public Button[] peerUpgradeBtns;

    public Button adBtn;

    public bool isUp = false;

    public AdMobTest _admob;

    // private variables
    private GameManager manager;

    // Mono Method
    private void Awake()
    {
        manager = GameManager.Instance;
    }

    private void Update()
    {
        var cc = manager.peers.ToArray();

        for(int i = 0; i < cc.Length; i++)
        {
            peerBuyBtns[i].interactable = !cc[i].activeSelf;
            peerUpgradeBtns[i].interactable = cc[i].activeSelf;
        }
    }
    //

    // public Method
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnShowPanel(GameObject obj)
    {
        foreach (GameObject go in Views)
        {
            if (go != obj)
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
        if (obj.activeSelf == false)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "1":
                    if (manager.coin > 100)
                    {
                        manager.coin -= 100;
                        obj.SetActive(true);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "2":
                    if (manager.coin > 200)
                    {
                        manager.coin -= 200;
                        obj.SetActive(true);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "3":
                    if (manager.coin > 300)
                    {
                        manager.coin -= 300;
                        obj.SetActive(true);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "4":
                    if (manager.coin > 400)
                    {
                        manager.coin -= 400;
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

        if (obj.activeSelf)
        {
            peer.Level += 1;

            peer.SetDamage();
        }
    }

    public void UpgradePlayer()
    {
        if (manager.coin > 50)
        {
            manager.coin -= 50;

            manager.playerDamage += 1;
            manager.playerAutoDamage += 1;
        }
    }

    // 스킬 강화 넣어야 됨
    public void Skill1()
    {
        // 나중에 다 float 형으로 변경해야 됨
        float damage = manager.playerDamage * 2;

        Damage(damage);
    }

    // 분노 시에는 데미지 저장 안되게 설정해야함..
    public void Skill2()
    {
        StartCoroutine(Up());
    }

    // 특정 스테이지 이상에서만 버튼의 상호작용 기능이 활성화되게 적용
    public void Rebirth()
    {
        var monsterSpawner = manager.monsterSpawner;

        var peers = manager.peers;

        if (monsterSpawner.TryGetComponent(out MonsterSpawner spawner))
        {
            if (spawner.Count >= 5)
            {               
                spawner.Count = -1;

                IncreaseRebirthCoin(manager.playerAutoDamage);
                IncreaseRebirthCoin(manager.playerDamage);
                IncreaseRebirthCoin(manager.coin / 100);

                for (int i = 0; i < peers.Count; i++)
                {
                    if (peers[i].TryGetComponent(out Peer peer))
                    {
                        IncreaseRebirthCoin((peer != null) ? peer.Level : 0);
                    }
                }

                manager.playerAutoDamage = 5;
                manager.playerDamage = 10;
                manager.coin = 0;

                for (int i = 0; i < peers.Count; i++)
                {
                    if (peers[i].TryGetComponent(out Peer peer))
                    {
                        peer.Level = 1;
                    }
                }

                foreach (GameObject obj in manager.peers)
                {
                    obj.SetActive(false);
                }

                spawner.currentMonster.GetComponent<Monster>().OnDeath.Invoke();
            }
        }
    }

    // 미니게임 씬 불러올 때 이벤트 시스템 비활성화 처리 해줘야 함..
    public void LoadMiniGameScene()
    {
        if (!isSceneLoaded("MiniGameScene"))
        {
            SceneManager.LoadSceneAsync("MiniGameScene", LoadSceneMode.Additive);
        }
    }

    // 광고 시청 버튼 함수
    public void ShowRewardAD()
    {
        _admob.ShowAds();
    }
    //

    // private Method
    private void Damage(float damage)
    {
        var monster = manager.monsterSpawner.currentMonster.GetComponent<Monster>();

        monster.TakeDamage(damage);
    }

    // 분노 : 공격력 증가 코루틴
    private IEnumerator Up()
    {
        var duration = new WaitForSeconds(10f);

        if (!isUp)
        {
            isUp = true;

            var a = manager.playerDamage;

            manager.playerDamage *= 2;

            yield return duration;

            manager.playerDamage = a;

            isUp = false;
        }
    }

    // 코인 float 형태로 바꿔야 됨
    private void IncreaseRebirthCoin(float amount)
    {
        manager.RebirthCoin += (int)amount;
    }

    private bool isSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.name == sceneName)
            {
                return true;
            }
        }

        return false;
    }
    //
}
