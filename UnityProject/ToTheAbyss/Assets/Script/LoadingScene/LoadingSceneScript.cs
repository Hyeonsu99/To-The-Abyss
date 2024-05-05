using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour
{
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(StringValue.Scene.gameScene);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            if(op.progress < 0.9f)
            {
                progressText.text = $"{op.progress * 100f}%";
            }
            else
            {
                var waitSeconds = new WaitForSeconds(1f);

                progressText.text = "100%";

                yield return waitSeconds;

                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
