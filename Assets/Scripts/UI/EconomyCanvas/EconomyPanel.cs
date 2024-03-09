using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CoinPanel : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float YAppear;
    [SerializeField] private float YDisappear;
    [SerializeField] private UnityEvent BeforeAppear;
    [SerializeField] private UnityEvent AfterAppear;

    [SerializeField] private UnityEvent BeforeDisappear;

    [SerializeField] private UnityEvent AfterDisappear;
    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, YDisappear);
    }

    public void Appear()
    {
        BeforeAppear?.Invoke();
        GetComponent<RectTransform>().DOAnchorPosY(YAppear, duration).SetEase(Ease.InOutQuad).OnComplete(() => { AfterAppear?.Invoke(); });
    }
    public void Disappear()
    {
        BeforeDisappear?.Invoke();
        GetComponent<RectTransform>().DOAnchorPosY(YDisappear, duration).SetEase(Ease.InOutQuad).OnComplete(() => { AfterDisappear?.Invoke(); });

    }
}
