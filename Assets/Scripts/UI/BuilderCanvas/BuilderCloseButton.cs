using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;


public class BuilderCloseButton : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private UnityEvent disappearBuilderCloseButton;
    [SerializeField] private UnityEvent OnUp;


    public void OnPointerDown(PointerEventData data)
    {
        OnUp?.Invoke();

        Debug.Log("hit");
        Disappear();

    }


    public void Disappear()
    {

        disappearBuilderCloseButton?.Invoke();

    }
}
