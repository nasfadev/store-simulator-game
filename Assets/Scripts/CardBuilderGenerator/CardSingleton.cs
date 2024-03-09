using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using Lean.Localization;
using TMPro;




public class CardSingleton : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform cardBuilder;
    [SerializeField] private float duration;
    [SerializeField] private float scale;
    [SerializeField] private UnityEvent OnUp;

    private bool isRun;
    public static CardSingleton Instance;
    [HideInInspector] public bool isTouched;
    private void Awake()
    {
        Instance = this;
    }



    public void OnPointerDown(PointerEventData data)
    {

        if (!CardBuilder.isChange)
        {
            CardBuilder.isChange = true;
            cardBuilder.DOScale(Vector3.one * scale, duration);
            isRun = true;
        }

    }
    public void OnPointerMove(PointerEventData data)
    {
        if (isRun)
        {
            cardBuilder.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);
            isRun = false;
            Debug.Log("hit");
            CardBuilder.isChange = false;

        }

    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isRun)
        {
            if (!PlacerPanel.isDisappearing && !BuilderPanel.Instance.isDisappearing)
            {
                isTouched = true;

                OnUp?.Invoke();
            }
            Debug.Log("up");
            cardBuilder.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);

            isRun = false;
            CardBuilder.isChange = false;

        }

    }

}
