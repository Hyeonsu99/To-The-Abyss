using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    public int Health;

    public UnityAction OnDeath;

    public Slider HpBar;

    public TextMeshProUGUI HpText;


    // Start is called before the first frame update
    void Start()
    {
        var monsterSpawner = FindObjectOfType<MonsterSpawner>();

        if (monsterSpawner != null)
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

        HpBar.minValue = 0;
        HpBar.maxValue = Health;

        HpBar.value = Health;
    }

    private void Update()
    {
        HpBar.value = Health;

        HpText.text = string.Format(Health.ToString() + " / " + HpBar.maxValue);
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
            GameManager.Instance.coin += (int)HpBar.maxValue;

            OnDeath.Invoke();
        }

        Destroy(gameObject);
    }
}
