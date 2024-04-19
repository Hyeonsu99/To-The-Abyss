using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //private DateTime backgroundTime;
    //private DateTime foregroundTime;

    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(AutoDamage());
    }

    public void StartMainCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(AutoDamage());
    }

    public void StartMiniGameCoroutine()
    {
        StartCoroutine(AutoMiniDamage());
    }

    // 자동적으로 데메지를 주는 코드
    IEnumerator AutoDamage()
    {
        var waitUntil  = new WaitUntil(() => GameManager.Instance.monsterSpawner != null);

        yield return waitUntil;

        var delaytime = new WaitForSeconds(GameManager.Instance.delay);

        while(true)
        {
            var damage = GameManager.Instance.playerAutoDamage;

            GameManager.Instance.monster.TakeDamage(damage);

            yield return delaytime;
        }
    }

    IEnumerator AutoMiniDamage()
    {
        var delaytime = new WaitForSeconds(GameManager.Instance.delay);

        while (true)
        {
            var damage = GameManager.Instance.playerAutoDamage;

            GameManager.Instance.MiniGamedDamage += damage;

            yield return delaytime;
        }
    }
}
