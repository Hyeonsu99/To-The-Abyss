using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;

    public GameObject currentMonster;

    private List<GameObject> spawnedMonster = new List<GameObject>();

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
        if(spawnedMonster.Count == 0)
        {
            currentMonster = Instantiate(monsterPrefab, new Vector2(0, 1), Quaternion.identity);

            spawnedMonster.Add(currentMonster);

            Monster monster = currentMonster.GetComponent<Monster>();

            if (monster != null)
            {
                monster.OnDeath += MonsterDeath;
            }
        }      
    }

    public void Rebirth()
    {      
        if (Count >= 5)
        {
            Destroy(currentMonster);

            spawnedMonster.Clear();

            SpawnMonster();
        }
    }


    public void MonsterDeath()
    {
        Count++;

        spawnedMonster.Clear();

        SpawnMonster();
    }
}
