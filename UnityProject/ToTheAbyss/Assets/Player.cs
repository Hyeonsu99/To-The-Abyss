using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoDamage());
    }

    // �ڵ������� �������� �ִ� �ڵ�
    IEnumerator AutoDamage()
    {
        var manager = GameManager.Instance;

        yield return new WaitUntil(() => manager.monsterSpawner != null);

        var a = new WaitForSeconds(manager.delay);

        var monster = manager.monsterSpawner.currentMonster;

        var mon = manager.monsterSpawner.currentMonster.GetComponent<Monster>();

        while (true)
        {
            var damage = manager.playerAutoDamage;

            mon.TakeDamage(damage);

            yield return a;
        }      
    }
}
