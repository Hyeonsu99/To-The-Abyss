using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class Monster : MonoBehaviour
{
    // public Variables
    public float MaxHealth;

    public float CurrentHealth;

    public UnityAction OnDeath;

    public Slider HpBar;

    public TextMeshProUGUI HpText;

    public Canvas canvas;

    // private Variables
    private GameManager manager;

    public enum MonsterAttribute
    {
        None,
        Fire,
        Water,
        Earth,
        Light,
        Dark
    }

    public MonsterAttribute attribute;

    private void Awake()
    {
        manager = GameManager.Instance;
    }

    // Mono Method
    // Start is called before the first frame update
    void Start()
    {
        var monsterSpawner = manager.monsterSpawner;

        canvas.worldCamera = Camera.main;

        if (monsterSpawner != null)
        {
            if (monsterSpawner.Count <= 0)
            {
                MaxHealth = 100;
            }
            else
            {
                MaxHealth = 100 + (int)(100 * monsterSpawner.Count * 0.1f);
            }

            if (PlayerPrefs.HasKey("CurrentBossHealth"))
            {
                CurrentHealth = PlayerPrefs.GetFloat("CurrentBossHealth");

                if(CurrentHealth < MaxHealth)
                {
                    HpBar.value = (int)CurrentHealth;
                }
                else
                {
                    HpBar.value = MaxHealth;
                }

                PlayerPrefs.DeleteKey("CurrentBossHealth");
            }
            else
            {
                CurrentHealth = MaxHealth;
            }
        }

        HpBar.minValue = 0;
        HpBar.maxValue = MaxHealth;

        if (manager.pauseDamage > 0)
        {
            var damage = Mathf.Min(manager.pauseDamage, MaxHealth);

            TakeDamage(damage);

            manager.pauseDamage -= damage;
        }

        attribute = (MonsterAttribute)Random.Range(0, 6);
    }

    private void Update()
    {
        HpBar.value = Mathf.CeilToInt(CurrentHealth);
        
        HpText.text = string.Format(CurrentHealth.ToString() + " / " + HpBar.maxValue);

        canvas.gameObject.SetActive(!manager.isMiniGameAcitve);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("CurrentBossHealth", CurrentHealth);
    }
    //

    // public Method
    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    //

    // private Method
    private void Die()
    {
        if (OnDeath != null)
        {
            GameManager.Instance.coin += (int)HpBar.maxValue;

            OnDeath.Invoke();
        }

        Destroy(gameObject);
    }
    //
}
