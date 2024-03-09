using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class BuilderPanel : MonoBehaviour
{
    [SerializeField] private GameObject builderCanvas;

    [SerializeField] private float offset;
    [SerializeField] private float duration;
    [SerializeField] private UnityEvent appearBuilderPanelEvent;
    [SerializeField] private UnityEvent disappearBuilderPanelEvent;
    public static BuilderPanel Instance;
    [HideInInspector] public bool isDisappearing;

    private void Awake()
    {
        Instance = this;
    }
    public void Appear()
    {
        builderCanvas.SetActive(true);
        RectTransform rect = GetComponent<RectTransform>();

        rect.anchoredPosition -= Vector2.up * offset;
        rect.DOAnchorPosY(0f, duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            appearBuilderPanelEvent?.Invoke();
        });

    }
    public void Disappear(string arg)
    {
        isDisappearing = true;
        RectTransform rect = GetComponent<RectTransform>();

        rect.DOAnchorPosY(-offset, duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            isDisappearing = false;

            builderCanvas.SetActive(false);
            if (arg == "Close")
            {
                disappearBuilderPanelEvent?.Invoke();


            }

        });
    }
}
