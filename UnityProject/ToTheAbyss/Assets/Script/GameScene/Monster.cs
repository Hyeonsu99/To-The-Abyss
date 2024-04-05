using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    // public Variables
    public int MaxHealth;

    public int CurrentHealth;

    public UnityAction OnDeath;

    public Slider HpBar;

    public TextMeshProUGUI HpText;

    public Canvas canvas;

    // private Variables
    private GameManager manager;



    // Mono Method
    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.Instance;

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
                CurrentHealth = PlayerPrefs.GetInt("CurrentBossHealth");

                HpBar.value = CurrentHealth;


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
    }

    private void Update()
    {
        HpBar.value = CurrentHealth;

        HpText.text = string.Format(CurrentHealth.ToString() + " / " + HpBar.maxValue);

        canvas.gameObject.SetActive(!manager.isMiniGameAcitve);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CurrentBossHealth", CurrentHealth);
    }
    //

    // public Method
    public void TakeDamage(int Damage)
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
