using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Puppet;

    public GameObject currentPuppet;

    public TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += sceneLoaded;

        currentPuppet = Instantiate(Puppet, new Vector2(0, 1), Quaternion.identity);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= sceneLoaded;

        Destroy(currentPuppet);
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.MiniGamedDamage.ToString();
    }

    public void sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("미니 게임 씬 로드!");
    }

    public void UnLoadMiniGameScene()
    {
        SceneManager.UnloadSceneAsync("MiniGameScene");
    }
}
