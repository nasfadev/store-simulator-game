using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Localization;

public class ModularToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Requires")]
    [SerializeField] private RectTransform handler;
    [SerializeField] private LeanLocalizedTextMeshProUGUI handlerText;

    [SerializeField] private Sprite spriteHandlerFlagTrue;
    [Header("Configs")]
    [SerializeField] private float moveTweenTime;
    [SerializeField] private string translationNameFlagTrue;
    [SerializeField] private float xHandlerFlagTrue;
    [SerializeField] private float xHandlerFlagFalse;
    [Header("Flag True Event")]
    [SerializeField] private UnityEvent WhenFlagTrue;
    [Header("Flag False Event")]
    [SerializeField] private UnityEvent WhenFlagFalse;
    // hidden
    private Image handlerImage;
    private Sprite handlerDefaultSprite;
    private bool isCancel;
    private bool isClicked;
    private string defaultTranlationName;



    private void Start()
    {
        handlerImage = handler.GetComponent<Image>();
        handlerDefaultSprite = handlerImage.sprite;
        defaultTranlationName = handlerText.TranslationName;
    }

    public void OnPointerDown(PointerEventData data)
    {
        isCancel = false;
        handler.DOKill();
    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isCancel) return;


        if (!isClicked)
        {
            isClicked = true;
            handler.DOAnchorPosX(xHandlerFlagTrue, moveTweenTime).SetEase(Ease.InOutQuad);
            handlerImage.sprite = spriteHandlerFlagTrue;
            handlerText.TranslationName = translationNameFlagTrue;
            WhenFlagTrue?.Invoke();
        }
        else
        {
            isClicked = false;
            handler.DOAnchorPosX(xHandlerFlagFalse, moveTweenTime).SetEase(Ease.InOutQuad);
            handlerImage.sprite = handlerDefaultSprite;
            handlerText.TranslationName = defaultTranlationName;

            WhenFlagFalse?.Invoke();
        }

    }
    public void OnDrag(PointerEventData data)
    {
        isCancel = true;
    }
}
