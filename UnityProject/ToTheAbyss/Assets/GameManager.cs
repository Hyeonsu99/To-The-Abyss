using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ���� �Ŵ��� ������ ���� �ν��Ͻ�
    private static GameManager instance = null;

    // �� ����
    public int coin;

    // �÷��̾� ��ġ ���� ������
    public int playerDamage;
    // �÷��̾� �ڵ� ���� ������
    public int playerAutoDamage;

    // ���� ǥ�ÿ� �ؽ�Ʈ(���� a,b,c... ȭ�� ���� ���� ����)
    public TextMeshProUGUI CoinText;
    // �������� ǥ�ÿ� �ؽ�Ʈ 
    public TextMeshProUGUI StageText;
    // ���� ������ ��ũ��Ʈ
    public MonsterSpawner monsterSpawner;

    public float delay;

    // ��׶���� ������ �ð� ������ ����
    private DateTime _backGroundTime;
    // �ٽ� ����׶���� ������ �ð� ������ ����
    private DateTime _foreGroundTime;

    // ��׶���� ������ �ִ� ���� ����� ������ �ѷ�
    public int pauseDamage;

    // ���� ����Ʈ
    [SerializeField]
    private List<GameObject> peers = new List<GameObject>();

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameState();
    }

    private void LoadGameState()
    {
        if(PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        else
        {
            coin = 0;
        }

        if(!PlayerPrefs.HasKey("playerDamage"))
        {
            playerDamage = 10;
        }
        else
        {
            playerDamage = PlayerPrefs.GetInt("playerDamage");
        }

        if (!PlayerPrefs.HasKey("playerAutoDamage"))
        {
            playerAutoDamage = 5;
        }
        else
        {
            playerAutoDamage = PlayerPrefs.GetInt("playerDamage");
        }

        for(int i = 0; i < peers.Count; i++)
        {
            int state = PlayerPrefs.GetInt("Peer_" + i, 0);

            peers[i].SetActive(state == 1);
        }

        // �� ������ ������
        delay = 1;
    }

    private void Update()
    {
        CoinText.text = coin.ToString();

        StageText.text = $"Stage : {monsterSpawner.Count + 1}";
    }

    // ������ �Ͻ����� �Ǿ��ִ� �ð��� ����Ͽ� ���ο� �߰�
    // ��� �������� �������Ѿ� ��.
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            _backGroundTime = DateTime.Now;
        }
        else
        {
            pauseDamage = 0;

            _foreGroundTime = DateTime.Now;

            var sec = _foreGroundTime.Subtract(_backGroundTime).TotalSeconds;

            pauseDamage += playerAutoDamage * (int)sec;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    private void SaveGameState()
    {
        for(int i = 0; i < peers.Count; i++)
        {
            PlayerPrefs.SetInt("Peer_" + i, peers[i].activeSelf ? 1 : 0);
        }

        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.SetInt("playerDamage", playerDamage);
        PlayerPrefs.SetInt("playerAutoDamage", playerAutoDamage);
        PlayerPrefs.SetInt("count", monsterSpawner.Count);

        PlayerPrefs.Save();
    }
}
