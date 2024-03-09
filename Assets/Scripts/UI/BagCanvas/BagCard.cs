using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using System;
public class BagCard : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private Image cardImage;

    [SerializeField] private LeanLocalizedTextMeshProUGUI titleTranslationNameLean;
    [SerializeField] private TextMeshProUGUI StockQuantityText;
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float scaleTweenSize;
    public static bool isSingleDown;
    private float defaultSize;
    public Sprite imageSprite;

    public string titleTranslationName;
    public int StockQuantity;
    public int IDspReady;
    public string mode;
    public UnityEvent OnDown;
    public UnityEvent OnUp;
    public UnityEvent AfterOnUp;

    private bool isReadyToUp;
    private void Start()
    {
        defaultSize = thisRect.localScale.x;
        cardImage.sprite = imageSprite;
        titleTranslationNameLean.TranslationName = titleTranslationName;
        StockQuantityText.text = $"{StockQuantity}x";
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (isSingleDown)
        {
            return;
        }
        OnDown?.Invoke();
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * scaleTweenSize, scaleTweenTime);
        isReadyToUp = true;
        isSingleDown = true;
    }
    public void OnPointerMove(PointerEventData data)
    {
        isReadyToUp = false;
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
        isSingleDown = false;
    }
    public void OnPointerUp(PointerEventData data)
    {
        if (!isReadyToUp)
        {
            return;
        }


        OnUp?.Invoke();
        PlaceButton.Instance.ChangeMode(mode);
        PlaceButton.Instance.ChangeContentID(IDspReady + 1);
        AfterOnUp?.Invoke();
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
        isSingleDown = false;


        // StockShop.Instance.Run(IDspReady + 1);


    }
}
