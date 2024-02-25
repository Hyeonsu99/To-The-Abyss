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

    public float playerDamage;

    public TextMeshProUGUI CoinText;

    public bool isTouch;

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

            CoinText.text = coin.ToString();
        }
    }

    private void SetGame()
    {
        touchGold = 1;
        autoGold = 1;
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
