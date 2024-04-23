using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public Value
    // ���� �Ŵ��� �ν��Ͻ�
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    [Header("��ũ��Ʈ ����")]
    // �÷��̾�
    public Player player;
    // ���� ������ ��ũ��Ʈ
    public MonsterSpawner monsterSpawner;
    //
    public AttributeTest atTest;

    public Monster monster;

    [Header("��ȭ �� ����")]
    // �� ����
    public int coin;
    // �÷��̾� ��ġ ���� ������
    public float playerDamage;
    // �÷��̾� �ڵ� ���� ������
    public float playerAutoDamage;
    // �÷��̾� �ڵ����� ������
    public float delay;
    // ��׶���� ������ �ִ� ���� ����� ������ �ѷ�
    public float pauseDamage;
    // ȯ�� ��ȭ
    public int RebirthCoin;
    // �̴ϰ��� ������
    public float MiniGamedDamage;

    [Header("������")]
    // ���� ����Ʈ
    public List<GameObject> peers = new List<GameObject>();

    [Header("UI")]
    // ���� ǥ�ÿ� �ؽ�Ʈ(���� a,b,c... ȭ�� ���� ���� ����)
    public TextMeshProUGUI CoinText;
    // �������� ǥ�ÿ� �ؽ�Ʈ 
    public TextMeshProUGUI StageText;
    // ȯ�� ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI RebirthCoinText;

    [Header("��Ÿ ����")]
    // ���� ���� �� ����۱����� �ð� ���
    public TimeSpan QuitTimeToRestartTime;
    // �̴ϰ��Ӿ��� Ȱ��ȭ ����
    public bool isMiniGameAcitve = false;
    #endregion

    #region private Value
    // ���� �Ŵ��� ������ ���� �ν��Ͻ�
    private static GameManager instance = null;

    // ��׶���� ������ �ð� ������ ����
    private DateTime _backGroundTime;
    // �ٽ� ����׶���� ������ �ð� ������ ����
    private DateTime _foreGroundTime;
    #endregion


    // Mono Method

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
        if (scene.name == StringValue.Scene.miniGameScene)
        {
            SetLayerMask(7);

            isMiniGameAcitve = !isMiniGameAcitve;

            CoinText.gameObject.SetActive(false);
            StageText.gameObject.SetActive(false);
            RebirthCoinText.gameObject.SetActive(false);

            MiniGamedDamage = 0;

            player.StartMiniGameCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                if(peers[i].activeSelf)
                {
                    if(peers[i].TryGetComponent(out Peer peer))
                    {
                        peer.StartMiniCoroutine();
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    void OnSceneUnload(Scene unloadScene)
    {
        if (unloadScene.name == StringValue.Scene.miniGameScene)
        {
            Everything();

            isMiniGameAcitve = !isMiniGameAcitve;

            CoinText.gameObject.SetActive(true);
            StageText.gameObject.SetActive(true);
            RebirthCoinText.gameObject.SetActive(true);

            player.StartMainCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                if (peers[i].activeSelf)
                {
                    if (peers[i].activeSelf)
                    {
                        if (peers[i].TryGetComponent(out Peer peer))
                        {
                            peer.StartMainCoroutine();
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            coin += (int)MiniGamedDamage;

            MiniGamedDamage = 0;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameState();   
        
        if(PlayerPrefs.HasKey("QuitTime"))
        {
            DateTime quitTime = DateTime.Parse(PlayerPrefs.GetString("QuitTime"));

            QuitTimeToRestartTime = DateTime.Now - quitTime;

            Debug.Log($"{QuitTimeToRestartTime.Hours}�ð� {QuitTimeToRestartTime.Minutes}�� {QuitTimeToRestartTime.Seconds}�� ���� �����ϼ̳׿�!!");

            if(QuitTimeToRestartTime != null)
            {
                pauseDamage += (float)QuitTimeToRestartTime.TotalSeconds * playerAutoDamage;

                IncreasePauseDamageByPeerDamage(QuitTimeToRestartTime);

                StartCoroutine(TakePauseDamage());
            }
        }
    }

    private void Update()
    {
        CoinText.text = coin.ToString();

        StageText.text = $"Stage : {monsterSpawner.Count + 1}";

        RebirthCoinText.text = $"Soul : {RebirthCoin}";
    }

    // 
    
    // public Method
    // 

    // private Method

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
            playerDamage = 10f;
        }
        else
        {
            playerDamage = PlayerPrefs.GetFloat("playerDamage");
        }

        if (!PlayerPrefs.HasKey("playerAutoDamage"))
        {
            playerAutoDamage = 5f;
        }
        else
        {
            playerAutoDamage = PlayerPrefs.GetFloat("playerDamage");
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

        pauseDamage = 0;

        delay = 1;
    }

    private bool coroutineRunning = false;

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            pauseDamage = 0;

            _backGroundTime = DateTime.Now;
        }
        else if(!coroutineRunning)
        {
            _foreGroundTime = DateTime.Now;

            TimeSpan amount = _foreGroundTime - _backGroundTime;

            pauseDamage += playerAutoDamage * (int)amount.TotalSeconds;

            IncreasePauseDamageByPeerDamage(amount);

            StartCoroutine(TakePauseDamage());
        }
    }

    private void IncreasePauseDamageByPeerDamage(TimeSpan amount)
    {
        foreach(GameObject obj in peers)
        {
            if (obj.TryGetComponent(out Peer peer))
            {
                var damage = atTest.GetAttributeDamage(peer.type.ToString(), monsterSpawner.currentMonster.GetComponent<Monster>().attribute.ToString());
                switch (peer.type)
                {                  
                    case Peer.PeerType.One:
                        pauseDamage += peer.damage * damage * (int)amount.TotalSeconds;
                        break;
                    case Peer.PeerType.Two:
                        pauseDamage += peer.damage * damage * ((int)amount.TotalSeconds / 2);
                        break;
                    case Peer.PeerType.Three:
                        pauseDamage += peer.damage * damage * ((int)amount.TotalSeconds / 3);
                        break;
                    case Peer.PeerType.Four:
                        pauseDamage += peer.damage * damage * ((int)amount.TotalSeconds / 4);
                        break;
                }

            }
        }
    }

    IEnumerator TakePauseDamage()
    {
        coroutineRunning = true;

        var waitUntil = new WaitUntil(() => pauseDamage > 0);
        
        yield return waitUntil;

        var damage = Mathf.Min(monster.CurrentHealth, pauseDamage);

        monster.CurrentHealth -= damage;

        pauseDamage -= damage;

        coroutineRunning = false;
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

        SaveIntData("coin", coin);
        SaveIntData("count", monsterSpawner.Count);
        SaveIntData("rebirthCoin", RebirthCoin);

        SaveFloatData("CurrentBossHealth", monster.CurrentHealth);
        SaveFloatData("playerDamage", playerDamage);
        SaveFloatData("playerAutoDamage", playerAutoDamage);

        SaveStringData("QuitTime", DateTime.Now.ToString());

        PlayerPrefs.Save();
    }

    private void SaveIntData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    private void SaveFloatData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private void SaveStringData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    private void SetLayerMask(int layerIndex)
    {
        Camera.main.cullingMask = ~(1 << layerIndex);
    }

    private void Everything()
    {
        Camera.main.cullingMask = -1;
    }
    //

}
