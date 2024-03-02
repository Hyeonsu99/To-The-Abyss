using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;

    private GameObject currentMonster;

    public int Count;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Count"))
        {
            Count = 1;
        }
        else
        {
            Count = PlayerPrefs.GetInt("Count");
        }

        SpawnMonster();
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

    public void MonsterDeath()
    {
        Count++;

        SpawnMonster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
