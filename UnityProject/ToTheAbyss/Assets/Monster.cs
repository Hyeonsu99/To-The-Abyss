using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    public int Health;

    public UnityAction OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        var monsterSpawner = FindObjectOfType<MonsterSpawner>();

        if(monsterSpawner != null)
        {
            if(monsterSpawner.Count <= 0)
            {
                Health = 100;
            }
            else
            {
                Health = 100 + (int)(100 * monsterSpawner.Count * 0.1f);
            }
        }
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if(Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(OnDeath != null)
        {
            OnDeath.Invoke();
        }

        Destroy(gameObject);
    }
}
