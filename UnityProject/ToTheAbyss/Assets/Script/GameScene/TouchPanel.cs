using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TouchPanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 0 && !SceneManager.GetSceneByName("MiniGameScene").isLoaded)
        {
            var monster = GameManager.Instance.monsterSpawner.currentMonster.GetComponent<Monster>();

            var damage = GameManager.Instance.playerDamage;

            monster.TakeDamage(damage);
        }
    }   
}
