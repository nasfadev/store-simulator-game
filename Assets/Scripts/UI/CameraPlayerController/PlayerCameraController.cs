using UnityEngine;
using UnityEngine.EventSystems;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine.Rendering;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;

public class PlayerCameraController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private int2 touchIndex;
    public static float2 delta;
    public static bool isRotate;
    private float2 deltaStored;
    private int joyStickId;
    private bool isRun;
    private void Start()
    {
        touchIndex = int2.zero;
        joyStickId = 1;
        isRotate = false;
        isRun = false;
    }
    private void Update()
    {
        if (isRun)
        {
            if (!deltaStored.Equals(delta))
            {
                isRotate = true;
                deltaStored = delta;
            }
            else
            {
                isRotate = false;
            }
        }

    }
    public void OnPointerDown(PointerEventData data)
    {

        if (touchIndex.y != joyStickId)
        {
            touchIndex = new int2(data.pointerId, joyStickId);
            isRun = true;

        }
    }
    public void OnDrag(PointerEventData data)

    {

        if (data.pointerId == touchIndex.x)
        {
            delta = data.delta;

        }

    }

    public void OnPointerUp(PointerEventData data)
    {

        if (data.pointerId == touchIndex.x)
        {
            touchIndex = int2.zero;
            isRotate = false;

            isRun = false;

        }

    }
    public void KillPlayerCameraController()
    {
        touchIndex = int2.zero;
        isRotate = false;

        isRun = false;
    }
}