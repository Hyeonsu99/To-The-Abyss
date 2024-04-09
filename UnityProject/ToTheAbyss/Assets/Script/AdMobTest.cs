using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class AdMobTest : MonoBehaviour
{
    private RewardedAd rewardAd;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardAd;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        InitAds();
    }

    private void InitAds()
    {
        if (_rewardAd != null)
        {
            _rewardAd.Destroy();
            _rewardAd = null;
        }

        var adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest, adLoadCallback);
    }

    private void adLoadCallback(RewardedAd rewardAd, LoadAdError loadAdError)
    {
        if(rewardAd != null)
        {
            _rewardAd = rewardAd;
            Debug.Log("�ε� ����");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }
    }

    public void ShowAds()
    {
        if(_rewardAd.CanShowAd())
        {
            _rewardAd.Show(GetReward);
        }
        else
        {
            Debug.Log("���� ��� ����");
        }
    }


    public void GetReward(Reward reward)
    {
        Debug.Log("���� ���� ����");

        InitAds();
    }
}
