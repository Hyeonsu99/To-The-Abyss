using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.touchCount > 0)
        {
            GameManager.Instance.coin += GameManager.Instance.touchGold;

            GameManager.Instance.isTouch = true;
        }
    }   

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.Instance.isTouch = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
