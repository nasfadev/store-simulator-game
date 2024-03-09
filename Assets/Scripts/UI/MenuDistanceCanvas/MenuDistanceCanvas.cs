using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MenuDistanceCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] FloatingButtons;
    public static GameObject[] floatingButtons;
    public static Transform thisTransform;
    [SerializeField] private UnityEvent OnAppear;


    private void Awake()
    {
        thisTransform = transform;
        floatingButtons = FloatingButtons;

    }

}
