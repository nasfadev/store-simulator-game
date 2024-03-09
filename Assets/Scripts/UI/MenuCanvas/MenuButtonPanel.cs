using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MenuButtonPanel : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private UnityEvent OnAppear;

    [SerializeField] private UnityEvent OnDisappear;



    public void Appear()
    {
        OnAppear?.Invoke();
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().DOAnchorPosY(0, duration).SetEase(Ease.InOutQuad);
    }
    public void Disappear()
    {
        GetComponent<RectTransform>().DOAnchorPosY(150, duration).SetEase(Ease.InOutQuad).OnComplete(() => { OnDisappear?.Invoke(); });

    }
}
