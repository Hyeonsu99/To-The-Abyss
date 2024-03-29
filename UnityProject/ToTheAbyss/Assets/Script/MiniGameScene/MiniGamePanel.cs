using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGamePanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.MiniGamedDamage += GameManager.Instance.playerDamage;
            }
        }
    }

}
