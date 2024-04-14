using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public Value
    // 게임 매니저 인스턴스
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

    [Header("스크립트 참조")]
    // 플레이어
    public Player player;
    // 몬스터 스포너 스크립트
    public MonsterSpawner monsterSpawner;

    [Header("재화 및 숫자")]
    // 총 코인
    public int coin;
    // 플레이어 터치 공격 데미지
    public float playerDamage;
    // 플레이어 자동 공격 데미지
    public float playerAutoDamage;
    // 플레이어 자동공격 딜레이
    public float delay;
    // 백그라운드로 내려가 있는 동안 진행된 데미지 총량
    public float pauseDamage;
    // 환생 재화
    public int RebirthCoin;
    // 미니게임 데미지
    public float MiniGamedDamage;

    [Header("데이터")]
    // 동료 리스트
    public List<GameObject> peers = new List<GameObject>();

    [Header("UI")]
    // 코인 표시용 텍스트(향후 a,b,c... 화폐 단위 변경 예정)
    public TextMeshProUGUI CoinText;
    // 스테이지 표시용 텍스트 
    public TextMeshProUGUI StageText;
    // 환생 코인 표시용 텍스트
    public TextMeshProUGUI RebirthCoinText;

    [Header("기타 변수")]
    // 게임 종료 후 재시작까지의 시간 계산
    public TimeSpan QuitTimeToRestartTime;
    // 미니게임씬의 활성화 여부
    public bool isMiniGameAcitve = false;
    #endregion

    #region private Value
    // 게임 매니저 참조를 위한 인스턴스
    private static GameManager instance = null;

    // 백그라운드로 진입한 시간 측정용 변수
    private DateTime _backGroundTime;
    // 다시 포어그라운드로 진입한 시간 측정용 변수
    private DateTime _foreGroundTime;
    #endregion


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
        if (unloadScene.name == "MiniGameScene")
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

            Debug.Log($"{QuitTimeToRestartTime.Hours}시간 {QuitTimeToRestartTime.Minutes}분 {QuitTimeToRestartTime.Seconds}초 만에 접속하셨네요!!");

            if(QuitTimeToRestartTime != null)
            {
                pauseDamage += (float)QuitTimeToRestartTime.TotalSeconds * playerAutoDamage;

                StartCoroutine(TakePauseDamage());
            }
        }
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

    private void Update()
    {
        CoinText.text = coin.ToString();

        StageText.text = $"Stage : {monsterSpawner.Count + 1}";

        RebirthCoinText.text = $"Soul : {RebirthCoin}";
    }

    private bool coroutineRunning = false;

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            pauseDamage = 0;
        }
        else if(!coroutineRunning)
        {
            StartCoroutine(TakePauseDamage());
        }
    }

    IEnumerator TakePauseDamage()
    {
        coroutineRunning = true;

        var monster = monsterSpawner.currentMonster.GetComponent<Monster>();

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

        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.SetFloat("playerDamage", playerDamage);
        PlayerPrefs.SetFloat("playerAutoDamage", playerAutoDamage);
        PlayerPrefs.SetInt("count", monsterSpawner.Count);
        PlayerPrefs.SetInt("rebirthCoin", RebirthCoin);

        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToString());

        PlayerPrefs.Save();
    }

    private void SetLayerMask(int layerIndex)
    {
        Camera.main.cullingMask = ~(1 << layerIndex);
    }

    private void Everything()
    {
        Camera.main.cullingMask = -1;
    }
}
