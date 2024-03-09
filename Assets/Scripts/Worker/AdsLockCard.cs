
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
public class AdsLockCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public event Action onUp;

    private bool isCancel;

    public void OnPointerDown(PointerEventData data)
    {
        isCancel = false;

    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isCancel) return;

        onUp?.Invoke();
        Debug.Log("adsclicked");
    }
    public void OnPointerMove(PointerEventData data)
    {
        isCancel = true;
    }
}
