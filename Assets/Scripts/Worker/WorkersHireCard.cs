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
using Unity.Mathematics;
public class WorkersHireCard : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [Header("Requires")]
    [SerializeField] private Image thisImage;
    [SerializeField] private GameObject lockBox;
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI askariPriceText;
    [Header("Configs")]
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float scaleTweenSize;
    public static bool isSingleDown;
    private float defaultSize;
    [Header("Previews")]
    public int moriumPrice;
    public int askariPrice;
    public bool isLock;
    public string typeTranslationName;
    public StoreData storeData;
    public UnityEvent OnDown;
    public UnityEvent OnUp;
    public Workers workers;

    private bool isReadyToUp;
    private void Start()
    {
        defaultSize = thisRect.localScale.x;
        askariPriceText.text = "<sprite name=\"askari\"> " + storeData.MoneyString(askariPrice);
        titleText.text += " " + LeanLocalization.GetTranslationText(typeTranslationName);
        if (isLock)
        {
            thisImage.raycastTarget = false;
            lockBox.SetActive(true);
            lockBox.GetComponent<AdsLockCard>().onUp += workers.UnlockAdsEvents;
        }

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
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
        isSingleDown = false;


        // StockShop.Instance.Run(IDspReady + 1);


    }
}
