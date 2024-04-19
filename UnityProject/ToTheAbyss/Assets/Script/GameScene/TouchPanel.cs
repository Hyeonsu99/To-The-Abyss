using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TouchPanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 0 && !SceneManager.GetSceneByName(StringValue.Scene.miniGameScene).isLoaded)
        {
            var monster = GameManager.Instance.monster;

            var damage = GameManager.Instance.playerDamage;

            monster.TakeDamage(damage);
        }
    }   
}
