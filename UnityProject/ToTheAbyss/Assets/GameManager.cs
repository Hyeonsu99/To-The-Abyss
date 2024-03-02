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

    public MonsterSpawner monsterSpawner;

    public float delay;

    private DateTime _backGroundTime;
    private DateTime _foreGroundTime;

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
        SetGame();

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

    private void SetGame()
    {
        if(!PlayerPrefs.HasKey("autoGold") || !PlayerPrefs.HasKey("touchGold"))
        {
            touchGold = 1;
            autoGold = 1;
        }
        else
        {
            touchGold = PlayerPrefs.GetInt("touchGold");
            autoGold = PlayerPrefs.GetInt("autoGold");
        }

        if(!PlayerPrefs.HasKey("playerDamage"))
        {
            playerDamage = 10;
        }
        else
        {
            playerDamage = PlayerPrefs.GetInt("playerDamage");
        }

        playerAutoDamage = 5;

        delay = 1;
    }

    private void Update()
    {
        CoinText.text = coin.ToString();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            //_backGroundTime = DateTime.Now;
        }
        else
        {
            //_foreGroundTime = DateTime.Now;

            //var sec = _foreGroundTime.Subtract(_backGroundTime).TotalSeconds;

            //Coin += (int)sec;
        }
    }

    private void OnApplicationQuit()
    {

    }
}
