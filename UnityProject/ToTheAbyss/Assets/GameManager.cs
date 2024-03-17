using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public int coin;

    public int touchGold;
    public int autoGold;

    public int playerDamage;
    public int playerAutoDamage;

    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI StageText;

    public MonsterSpawner monsterSpawner;

    public float delay;

    private DateTime _backGroundTime;
    private DateTime _foreGroundTime;

    [SerializeField]
    private List<GameObject> peers = new List<GameObject>();

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameState();

        StartCoroutine(Method());
    }

    IEnumerator Method()
    {
        var cointime = new WaitForSeconds(1f);

        while(true)
        {
            yield return cointime;

            coin += autoGold;           
        }
    }

    private void LoadGameState()
    {
        if(PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        else
        {
            coin = 0;
        }

        if(!PlayerPrefs.HasKey("playerDamage"))
        {
            playerDamage = 10;
        }
        else
        {
            playerDamage = PlayerPrefs.GetInt("playerDamage");
        }

        if (!PlayerPrefs.HasKey("playerAutoDamage"))
        {
            playerAutoDamage = 5;
        }
        else
        {
            playerAutoDamage = PlayerPrefs.GetInt("playerDamage");
        }

        for(int i = 0; i < peers.Count; i++)
        {
            int state = PlayerPrefs.GetInt("Peer_" + i, 0);

            peers[i].SetActive(state == 1);
        }

        // 돈 오르는 딜레이
        delay = 1;
    }

    private void Update()
    {
        CoinText.text = coin.ToString();

        StageText.text = $"Stage : {monsterSpawner.Count + 1}";
    }

    // 게임이 일시정지 되어있는 시간을 계산하여 코인에 추가
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            _backGroundTime = DateTime.Now;
        }
        else
        {
            _foreGroundTime = DateTime.Now;

            var sec = _foreGroundTime.Subtract(_backGroundTime).TotalSeconds;

            coin += (int)sec;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    private void SaveGameState()
    {
        for(int i = 0; i < peers.Count; i++)
        {
            PlayerPrefs.SetInt("Peer_" + i, peers[i].activeSelf ? 1 : 0);
        }

        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.SetInt("playerDamage", playerDamage);
        PlayerPrefs.SetInt("playerAutoDamage", playerAutoDamage);
        PlayerPrefs.SetInt("count", monsterSpawner.Count);

        PlayerPrefs.Save();
    }
}
