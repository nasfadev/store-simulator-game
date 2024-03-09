using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MenuDistanceButton : MonoBehaviour, IPointerDownHandler
{
    public event Action OnDown;


    public void OnPointerDown(PointerEventData data)
    {
        OnDown?.Invoke();
        Destroy(gameObject);
    }
}
