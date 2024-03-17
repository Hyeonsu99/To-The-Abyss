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

    public int Level;

    public int damage;


    void Start()
    {
        Level = 1;

        SetDamage();

        StartCoroutine(AutoDamage());
    }

    public void SetDamage()
    {
        switch (type)
        {
            case PeerType.One:
                damage = Level * 1;
                break;
            case PeerType.Two:
                damage = Level * 2;
                break;
            case PeerType.Three:
                damage = Level * 4;
                break;
            case PeerType.Four:
                damage = Level * 8;
                break;
            default:
                break;
        }
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
                        Damage(damage);
                        yield return new WaitForSeconds(1f);
                        break;
                    case PeerType.Two:
                        Damage(damage);
                        yield return new WaitForSeconds(2f);
                        break;
                    case PeerType.Three:
                        Damage(damage);
                        yield return new WaitForSeconds(3f);
                        break;
                    case PeerType.Four:
                        Damage(damage);
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
}
