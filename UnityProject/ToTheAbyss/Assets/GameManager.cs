using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int Coin;

    public TextMeshProUGUI CoinText;

    private DateTime _backGroundTime;
    private DateTime _foreGroundTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Method());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Method()
    {
        var cointime = new WaitForSeconds(1f);

        while(true)
        {
            yield return cointime;

            Coin++;

            CoinText.text = Coin.ToString();
        }
    }

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

            Coin += (int)sec;
        }
    }
}
