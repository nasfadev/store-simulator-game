using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CashierGamePlayCanvas : MonoBehaviour
{
    public static CashierGamePlayCanvas Instance;
    public CanvasSimpleTweenFade canvasSimpleTweenFade;
    public CanvasSimpleTweenFade menuCanvas;

    private void Awake()
    {
        Instance = this;
    }
}
