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

    // 플레이어
    public Player player;

    // 총 코인
    public int coin;

    // 플레이어 터치 공격 데미지
    public int playerDamage;
    // 플레이어 자동 공격 데미지
    public int playerAutoDamage;

    /// <summary>
    /// 향후 UI 매니저 등으로 전환해야 함.
    /// </summary>
    // 코인 표시용 텍스트(향후 a,b,c... 화폐 단위 변경 예정)
    public TextMeshProUGUI CoinText;
    // 스테이지 표시용 텍스트 
    public TextMeshProUGUI StageText;
    // 환생 코인 표시용 텍스트
    public TextMeshProUGUI RebirthCoinText;

    // 몬스터 스포너 스크립트
    public MonsterSpawner monsterSpawner;
    // 플레이어 자동공격 딜레이
    public float delay;

    // 백그라운드로 내려가 있는 동안 진행된 데미지 총량
    public int pauseDamage;

    // 동료 리스트
    public List<GameObject> peers = new List<GameObject>();

    // 환생 재화
    public int RebirthCoin;

    // 미니게임 데미지
    public int MiniGamedDamage;
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

            MiniGamedDamage = 0;

            player.StartMiniGameCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                if(peers[i].activeSelf)
                {
                    peers[i].GetComponent<Peer>().StartMiniCoroutine();
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

            player.StartMainCoroutine();

            for (int i = 0; i < peers.Count; i++)
            {
                if (peers[i].activeSelf)
                {
                    peers[i].GetComponent<Peer>().StartMainCoroutine();
                }
                else
                {
                    break;
                }
            }

            coin += MiniGamedDamage;

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

        delay = 1;
    }

    private void Update()
    {
        CoinText.text = coin.ToString();

        StageText.text = $"Stage : {monsterSpawner.Count + 1}";

        RebirthCoinText.text = $"Soul : {RebirthCoin}";
    }

    // 게임이 일시정지 되어있는 시간을 계산하여 코인에 추가
    // 모든 데미지도 누적시켜야 함.
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

    private void SetLayerMask(int layerIndex)
    {
        Camera.main.cullingMask = ~(1 << layerIndex);
    }

    private void Everything()
    {
        Camera.main.cullingMask = -1;
    }
}
