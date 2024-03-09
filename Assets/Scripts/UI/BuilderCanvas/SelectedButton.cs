using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using UnityEngine.Events;


public class SelectedButton : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite unSelectedImage;
    [SerializeField] private UnityEvent OnUp;
    [SerializeField] private UnityEvent OnClear;



    public static event Action OnSelected;
    private bool isRun;
    public static bool isChange;
    private Image image;



    private void Awake()
    {
        image = GetComponent<Image>();
        OnSelected += OnUnselected;
    }
    public void OnPointerDown(PointerEventData data)
    {

        if (!isChange)
        {
            isChange = true;
            isRun = true;
        }

    }
    public void OnPointerMove(PointerEventData data)
    {
        if (isRun)
        {

            isRun = false;
            Debug.Log("hit");
            isChange = false;

        }

    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isRun)
        {
            Debug.Log("up");
            OnSelected -= OnUnselected;

            isRun = false;
            isChange = false;
            image.sprite = selectedImage;
            OnUp?.Invoke();
            OnSelected?.Invoke();
            OnClear?.Invoke();

            OnSelected += OnUnselected;

        }

    }
    private void OnUnselected()
    {
        image.sprite = unSelectedImage;
        // cool
    }
}
