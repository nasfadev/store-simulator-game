using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class CanvasSimpleTweenFade : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTweenTime;
    [SerializeField] private bool isAppear;
    [SerializeField] private bool _isDontDoAnythingAtFirst;

    [SerializeField] private UnityEvent BeforeAppear;
    [SerializeField] private UnityEvent AfterAppear;

    [SerializeField] private UnityEvent BeforeDisappear;

    [SerializeField] private UnityEvent AfterDisappear;
    private void Start()
    {
        if (_isDontDoAnythingAtFirst)
        {
            return;
        }
        canvas.enabled = true;
        if (isAppear)
        {
            return;
        }
        canvas.enabled = false;

    }
    public void Appear()
    {
        Debug.Log("appear " + gameObject.name);
        canvasGroup.DOKill();
        BeforeAppear?.Invoke();
        canvasGroup.alpha = 0f;
        canvas.enabled = true;

        canvasGroup.DOFade(1f, fadeTweenTime).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
        {
            AfterAppear?.Invoke();
        });
    }
    public void Disappear()
    {

        canvasGroup.DOKill();

        BeforeDisappear?.Invoke();
        canvasGroup.DOFade(0f, fadeTweenTime).SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            canvas.enabled = false;
            AfterDisappear?.Invoke();
        });
    }
    public void CustomAppear(CanvasSimpleTweenCustomEvent data)
    {
        data.customEvent?.Invoke();
        Appear();
    }
    public void CustomDisappear(CanvasSimpleTweenCustomEvent data)
    {
        Disappear();
        data.customEvent?.Invoke();
    }

}
