using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Peer : MonoBehaviour
{
    public enum PeerType
    {
        One = 0,
        Two,
        Three,
        Four
    }

    public PeerType type;
    // Start is called before the first frame update

    private PeerData data;

    public int Level;

    public int damage;

    private System.DateTime _backGroundTime;
    private System.DateTime _foreGroundTime;


    void Start()
    {
        data = GameManager.Instance.peerDatas[(int)type];

        Level = 1;

        SetDamage();

        if(GameManager.Instance.QuitTimeToRestartTime != null && gameObject.activeSelf)
        {
            var sec = GameManager.Instance.QuitTimeToRestartTime.TotalSeconds;

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

        StartCoroutine(AutoDamage());
    }

    public void StartMainCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(AutoDamage());
    }

    public void StartMiniCoroutine()
    {
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
        else
        {
            yield return null;
        }
    }

    IEnumerator MiniGameAutoDamage()
    {
        if (gameObject.activeInHierarchy)
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
        else
        {
            yield return null;
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

            var sec = _foreGroundTime - _backGroundTime;

            switch (type)
            {
                case PeerType.One:
                    GameManager.Instance.pauseDamage += damage * (int)sec.TotalSeconds;
                    break;
                case PeerType.Two:
                    GameManager.Instance.pauseDamage += damage * ((int)sec.TotalSeconds / 2);
                    break;
                case PeerType.Three:
                    GameManager.Instance.pauseDamage += damage * ((int)sec.TotalSeconds / 3);
                    break;
                case PeerType.Four:
                    GameManager.Instance.pauseDamage += damage * ((int)sec.TotalSeconds / 4);
                    break;
                default:
                    break;
            }
        }
    }
}
