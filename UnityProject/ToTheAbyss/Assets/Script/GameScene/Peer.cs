using System;
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

    public enum PeerAttribute
    {
        None,
        Fire,
        Water,
        Earth,
        Light,
        Dark
    }

    public PeerAttribute peerAttribute;


    public PeerType type;
    // Start is called before the first frame update

    public int Level;

    public float damage;

    void Start()
    {
        if(PlayerPrefs.HasKey($"Peer_{(int)type}_Level"))
        {
            Level = PlayerPrefs.GetInt($"Peer_{(int)type}_Level");
        }
        else
        {
            Level = 1;
        }

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
        var waitUntil = new WaitUntil(() => GameManager.Instance.monsterSpawner != null);

        yield return waitUntil;
    
        if(gameObject.activeSelf)
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

    void Damage(float damage)
    {
        var mon = GameManager.Instance.monster;

        float mul = GameManager.Instance.atTest.GetAttributeDamage(peerAttribute.ToString(), mon.attribute.ToString());

        if(mon.enabled)
        {
            mon.TakeDamage(damage * mul);
        }     
    }

    private void SavePrefByint(string key)
    {
        PlayerPrefs.SetInt(key, Level);
    }

    private void OnApplicationQuit()
    {
        if(gameObject.activeSelf)
        {
            switch (type)
            {
                case PeerType.One:
                    SavePrefByint($"Peer_{(int)type}_Level");
                    break;
                case PeerType.Two:
                    SavePrefByint($"Peer_{(int)type}_Level");
                    break;
                case PeerType.Three:
                    SavePrefByint($"Peer_{(int)type}_Level");
                    break;
                case PeerType.Four:
                    SavePrefByint($"Peer_{(int)type}_Level");
                    break;
                default:
                    break;
            }
        }
    }
}
