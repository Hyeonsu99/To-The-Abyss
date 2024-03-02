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

    IEnumerator AutoDamage()
    {
        yield return new WaitUntil(() => GameManager.Instance.monsterSpawner != null);

        var a = new WaitForSeconds(GameManager.Instance.delay);

        while (true)
        {
            var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();

            var damage = GameManager.Instance.playerAutoDamage;

            monster.TakeDamage(damage);

            yield return a;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
