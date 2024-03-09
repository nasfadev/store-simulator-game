using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class StockInventoryPanel : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private UnityEvent BeforeAppear;
    [SerializeField] private UnityEvent AfterAppear;

    [SerializeField] private UnityEvent BeforeDisappear;

    [SerializeField] private UnityEvent AfterDisappear;

    private void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
    public void Appear()
    {
        BeforeAppear?.Invoke();
        GetComponent<CanvasGroup>().DOFade(1f, duration).SetEase(Ease.InOutQuad).OnComplete(() => { AfterAppear?.Invoke(); });
    }
    public void Disappear()
    {
        BeforeDisappear?.Invoke();
        GetComponent<CanvasGroup>().DOFade(0f, duration).SetEase(Ease.InOutQuad).OnComplete(() => { AfterDisappear?.Invoke(); });

    }
}
