using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            GameManager.Instance.coin += GameManager.Instance.touchGold;         

            var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();

            var damage = GameManager.Instance.playerDamage;

            monster.TakeDamage(damage);
        }
#endif
    }   

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
