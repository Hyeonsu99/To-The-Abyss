using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private DateTime backgroundTime;
    private DateTime foregroundTime;

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

    // �ڵ������� �������� �ִ� �ڵ�
    IEnumerator AutoDamage()
    {
        var manager = GameManager.Instance;

        yield return new WaitUntil(() => manager.monsterSpawner != null);

        var delaytime = new WaitForSeconds(manager.delay);

        while(true)
        {
            var damage = manager.playerAutoDamage;

            manager.monsterSpawner.currentMonster.GetComponent<Monster>().TakeDamage(damage);

            yield return delaytime;
        }
    }

    IEnumerator AutoMiniDamage()
    {
        var manager = GameManager.Instance;

        var delaytime = new WaitForSeconds(manager.delay);

        while (true)
        {
            var damage = manager.playerAutoDamage;

            GameManager.Instance.MiniGamedDamage += damage;

            yield return delaytime;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            backgroundTime = DateTime.Now;
        }
        else
        {
            foregroundTime = DateTime.Now;

            TimeSpan amount = foregroundTime - backgroundTime;

            var manager = GameManager.Instance;

            manager.pauseDamage += manager.playerAutoDamage * (int)amount.TotalSeconds;
        }
    }
}