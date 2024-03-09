using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
public class CardStock : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
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
    public UnityEvent OnDown;
    public UnityEvent OnUp;
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
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
        isSingleDown = false;
        StockShop.Instance.Run(IDspReady + 1);
        OnUp?.Invoke();



    }
}
