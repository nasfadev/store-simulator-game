using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PlacerPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectSubject;

    [SerializeField] private float duration;
    public Mode mode = Mode.Builder;

    [SerializeField] private UnityEvent disappear;
    [SerializeField] private UnityEvent disappearBagMode;

    [SerializeField] private UnityEvent appear;
    [HideInInspector] public static bool isDisappearing;
    public enum Mode
    {
        Bag,
        Builder
    }
    public void Appear()
    {
        foreach (var item in gameObjectSubject)
        {
            item.SetActive(true);

        }


        CanvasGroup canGroup = GetComponent<CanvasGroup>();
        canGroup.alpha = 0;
        canGroup.DOFade(1, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            appear?.Invoke();

        });
    }
    public void ChangeMode(PlacerChangeMode placerChangeMode)
    {
        mode = placerChangeMode.mode;
    }

    public void Disappear()
    {
        isDisappearing = true;
        GetComponent<CanvasGroup>().DOFade(0, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            isDisappearing = false;

            foreach (var item in gameObjectSubject)
            {
                item.SetActive(false);
                if (mode == Mode.Builder)
                {
                    disappear?.Invoke();

                }
                else
                {
                    disappearBagMode?.Invoke();
                }

            }
        });
    }

}
