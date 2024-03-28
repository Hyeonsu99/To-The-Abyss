using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ���� �Ŵ��� ������ ���� �ν��Ͻ�
    private static GameManager instance = null;

    // �÷��̾�
    public Player player;

    // �� ����
    public int coin;

    // �÷��̾� ��ġ ���� ������
    public int playerDamage;
    // �÷��̾� �ڵ� ���� ������
    public int playerAutoDamage;

    /// <summary>
    /// ���� UI �Ŵ��� ������ ��ȯ�ؾ� ��.
    /// </summary>
    // ���� ǥ�ÿ� �ؽ�Ʈ(���� a,b,c... ȭ�� ���� ���� ����)
    public TextMeshProUGUI CoinText;
    // �������� ǥ�ÿ� �ؽ�Ʈ 
    public TextMeshProUGUI StageText;
    // ȯ�� ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI RebirthCoinText;

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
    public List<GameObject> peers = new List<GameObject>();

    // ȯ�� ��ȭ
    public int RebirthCoin;

    // �̴ϰ��� ������
    public int MiniGamedDamage;
    
    // ���� �� �̺�Ʈ �ý���
    public GameObject EventSystem;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MiniGameScene")
        {
            EventSystem.SetActive(false);

            player.StartMiniGameCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                peers[i].GetComponent<Peer>().StartMiniCoroutine();
            }
        }
    }

    void OnSceneUnload(Scene unloadScene)
    {
        if (unloadScene.name == "MiniGameScene")
        {
            EventSystem.SetActive(true);

            player.StartMainCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                peers[i].GetComponent<Peer>().StartMainCoroutine();
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnload;
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

        if(!PlayerPrefs.HasKey("rebirthCoin"))
        {
            RebirthCoin = 0;
        }
        else
        {
            RebirthCoin = PlayerPrefs.GetInt("rebirthCoin");
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

        RebirthCoinText.text = $"Soul : {RebirthCoin}";
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

        PlayerPrefs.SetInt("rebirthCoin", RebirthCoin);

        PlayerPrefs.Save();
    }
}
