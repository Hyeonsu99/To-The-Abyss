using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;

    public GameObject currentMonster;

    public int Count;
    // Start is called before the first frame update
    private void Awake()
    {
        SpawnMonster();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("count"))
        {
            Count = 0;
        }
        else
        {
            Count = PlayerPrefs.GetInt("count");
        }
    }

    private void SpawnMonster()
    {
        currentMonster = Instantiate(monsterPrefab, new Vector2(0, 1), Quaternion.identity);

        Monster monster = currentMonster.GetComponent<Monster>();

        if (monster != null)
        {
            monster.OnDeath += MonsterDeath;
        }    
    }

    public void Rebirth()
    {      
        if (Count >= 5)
        {
            Destroy(currentMonster);

            SpawnMonster();
        }
    }


    public void MonsterDeath()
    {
        Count++;

        SpawnMonster();
    }
}
