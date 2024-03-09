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
public class WorkersCard : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [Header("Requires")]
    [SerializeField] private Image thisImage;
    [SerializeField] private GameObject lockBox;
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private GameObject button;
    [Header("Configs")]
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float scaleTweenSize;
    public static bool isSingleDown;
    private float defaultSize;
    [Header("Previews")]
    public int level;
    public int maxLevel;
    public int id;
    public bool isLock;
    public Workers workers;
    public string titleTranslationName;
    public UnityEvent OnDown;
    public UnityEvent OnUp;

    private bool isReadyToUp;
    private void Start()
    {
        defaultSize = thisRect.localScale.x;
        titleText.text =
        Lean.Localization.LeanLocalization.GetTranslationText(titleTranslationName) + " - " + (id + 1);
        explainText.text = "Level " + (level + 1);
        if (level == maxLevel - 1)
        {
            button.SetActive(false);


        }
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
        if (!isReadyToUp || level == maxLevel - 1)
        {
            thisRect.DOKill();
            thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
            return;
        }

        workers.bookIdWorker = id;
        workers.UpdateUpgradeUI();
        OnUp?.Invoke();
        thisRect.DOKill();
        thisRect.DOScale(Vector3.one * defaultSize, scaleTweenTime);
        isSingleDown = false;


        // StockShop.Instance.Run(IDspReady + 1);


    }
}
