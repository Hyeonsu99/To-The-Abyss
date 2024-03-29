using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private System.DateTime _backGroundTime;
    private System.DateTime _foreGroundTime;


    void Start()
    {
        Level = 1;

        SetDamage();

        StartCoroutine(AutoDamage());
    }

    public void StartMainCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(AutoDamage());
    }

    public void StartMiniCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(MiniGameAutoDamage());
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

    IEnumerator MiniGameAutoDamage()
    {
        if (this.gameObject.activeSelf)
        {
            while (true)
            {
                switch (type)
                {
                    case PeerType.One:
                        GameManager.Instance.MiniGamedDamage += damage;
                        yield return new WaitForSeconds(1f);
                        break;
                    case PeerType.Two:
                        GameManager.Instance.MiniGamedDamage += damage;
                        yield return new WaitForSeconds(2f);
                        break;
                    case PeerType.Three:
                        GameManager.Instance.MiniGamedDamage += damage;
                        yield return new WaitForSeconds(3f);
                        break;
                    case PeerType.Four:
                        GameManager.Instance.MiniGamedDamage += damage;
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
        var monster = GameManager.Instance.monsterSpawner.currentMonster;
        var mon = monster.GetComponent<Monster>();

        if(mon.enabled)
        {
            mon.TakeDamage(damage);
        }     
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _backGroundTime = System.DateTime.Now;
        }
        else
        {
            _foreGroundTime = System.DateTime.Now;

            var sec = _foreGroundTime.Subtract(_backGroundTime).TotalSeconds;

            switch (type)
            {
                case PeerType.One:
                    GameManager.Instance.pauseDamage += damage * (int)sec;
                    break;
                case PeerType.Two:
                    GameManager.Instance.pauseDamage += damage * ((int)sec / 2);
                    break;
                case PeerType.Three:
                    GameManager.Instance.pauseDamage += damage * ((int)sec / 3);
                    break;
                case PeerType.Four:
                    GameManager.Instance.pauseDamage += damage * ((int)sec / 4);
                    break;
                default:
                    break;
            }
        }
    }
}
