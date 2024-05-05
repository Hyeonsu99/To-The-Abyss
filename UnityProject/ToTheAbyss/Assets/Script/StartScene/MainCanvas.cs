using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _backgroundimg;

    [SerializeField] private Text _logotext;

    [SerializeField] private Text _starttext;
    
    public void OnClick()
    {
        
    }

    private void Start()
    {
        StartCoroutine(FadeLogoText());
    }

    IEnumerator FadeLogoText()
    {
        Color color = _logotext.color;

        while(_logotext.color.a < 1)
        {
            color.a += Time.deltaTime * 1.5f;

            _logotext.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (_logotext.color.a >= 0)
        {
            color.a -= Time.deltaTime * 1.5f;

            _logotext.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _backgroundimg.gameObject.SetActive(false);
    }

    IEnumerator FadeInOutStartText()
    {
        yield return null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_backgroundimg.gameObject.activeSelf == false && Input.touchCount > 0)
        {
            SceneManager.LoadScene(StringValue.Scene.loadingScene);
        }
    }
}
