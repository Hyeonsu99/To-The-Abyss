using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
        StopAllCoroutines();
        StartCoroutine(AutoMiniDamage());
    }

    // 자동적으로 데메지를 주는 코드
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
}
