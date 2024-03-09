using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class ModularButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Requires")]

    [SerializeField] private Sprite onDownSprite;
    [SerializeField] private Sprite onUpSpriteForFlagButton;
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float scaleTweenSize;
    [SerializeField] private bool flagButton;


    [Header("On Down")]
    [SerializeField] private UnityEvent onDown;
    [Header("On Up")]
    [SerializeField] private UnityEvent onUp;
    [Header("On Up For Flag True")]
    [SerializeField] private UnityEvent onUpFlagTrue;
    // hiden
    private Image image;
    private RectTransform button;
    private Sprite defaultSprite;
    private Vector3 defaultScale;
    private bool isCancel;
    private bool isClicked;
    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<RectTransform>();
        defaultSprite = image.sprite;
        defaultScale = button.localScale;
    }
    public void OnPointerDown(PointerEventData data)
    {
        isCancel = false;
        if (onDownSprite != null)
        {
            image.sprite = onDownSprite;
        }
        button.DOKill();
        button.DOScale(Vector3.one * scaleTweenSize, scaleTweenTime).SetEase(Ease.OutQuad);
        onDown?.Invoke();
    }
    public void OnKeyDown()
    {
        isCancel = false;
        if (onDownSprite != null)
        {
            image.sprite = onDownSprite;
        }
        button.DOKill();
        button.DOScale(Vector3.one * scaleTweenSize, scaleTweenTime).SetEase(Ease.OutQuad);
        onDown?.Invoke();
        if (isCancel) return;
        if (onDownSprite != null)
        {
            image.sprite = defaultSprite;
        }
        if (flagButton)
        {
            if (!isClicked)
            {
                isClicked = true;
                image.sprite = onUpSpriteForFlagButton;
                onUp?.Invoke();
            }
            else
            {
                image.sprite = defaultSprite;
                isClicked = false;
                onUpFlagTrue?.Invoke();
            }
        }
        else
        {
            onUp?.Invoke();

        }
        button.DOScale(defaultScale, scaleTweenTime).SetEase(Ease.OutQuad);
    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isCancel) return;
        if (onDownSprite != null)
        {
            image.sprite = defaultSprite;
        }
        if (flagButton)
        {
            if (!isClicked)
            {
                isClicked = true;
                image.sprite = onUpSpriteForFlagButton;
                onUp?.Invoke();
            }
            else
            {
                image.sprite = defaultSprite;
                isClicked = false;
                onUpFlagTrue?.Invoke();
            }
        }
        else
        {
            onUp?.Invoke();

        }
        button.DOScale(defaultScale, scaleTweenTime).SetEase(Ease.OutQuad);

    }
    public void OnDrag(PointerEventData data)
    {
        button.DOScale(defaultScale, scaleTweenTime).SetEase(Ease.OutQuad);
        isCancel = true;
    }
}
