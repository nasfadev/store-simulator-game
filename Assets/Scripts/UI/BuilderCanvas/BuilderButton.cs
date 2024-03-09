using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class BuilderButton : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Vector2 offsetPos;
    private Vector2 defaultPos;
    [SerializeField] private float duration;


    [SerializeField] private UnityEvent disappearBuilderButtonEvent;
    [SerializeField] private UnityEvent OnUp;


    [SerializeField] private GameObject[] GOs;



    // Start is called before the first frame update
    public static bool isShow;
    // Update is called once per frame
    private void Awake()
    {
        defaultPos = GetComponent<RectTransform>().anchoredPosition;
    }
    public void OnPointerDown(PointerEventData data)
    {
        OnUp?.Invoke();

        Disappear();

    }
    public void Appear()
    {

        foreach (var item in GOs)
        {
            item.SetActive(true);

        }
        GetComponent<RectTransform>().DOAnchorPos(defaultPos, duration).SetEase(Ease.InOutQuad);
    }
    public void Disappear()
    {
        GetComponent<RectTransform>().DOAnchorPos(offsetPos, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            foreach (var item in GOs)
            {
                item.SetActive(false);

            }

            disappearBuilderButtonEvent?.Invoke();
        });
    }

}
