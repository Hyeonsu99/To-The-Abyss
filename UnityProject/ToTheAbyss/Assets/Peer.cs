using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peer : MonoBehaviour
{
    public enum PeerType
    {
        None,
        One,
        Two,
        Three,
        Four
    }

    public PeerType type = PeerType.None;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoDamage());
    }

    IEnumerator AutoDamage()
    {
        yield return new WaitUntil(() => GameManager.Instance.monsterSpawner != null);

        var a = new WaitForSeconds(GameManager.Instance.delay);

        if(this.gameObject.activeSelf)
        {
            while (true)
            {
                switch (type)
                {
                    case PeerType.One:
                        Damage(1);
                        yield return new WaitForSeconds(1f);
                        break;
                    case PeerType.Two:
                        Damage(2);
                        yield return new WaitForSeconds(2f);
                        break;
                    case PeerType.Three:
                        Damage(4);
                        yield return new WaitForSeconds(3f);
                        break;
                    case PeerType.Four:
                        Damage(8);
                        yield return new WaitForSeconds(4f);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Damage(int damage)
    {
        var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();
        monster.TakeDamage(damage);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
