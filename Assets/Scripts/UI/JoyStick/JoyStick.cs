using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Unity.Mathematics;
using UnityEngine.Rendering;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private int2 touchIndex;
    private RectTransform joyStick;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private GameObject handler;
    private RectTransform wrapperRect;
    private RectTransform handlerRect;
    public static bool isMove;
    public static float degrees;
    public static float2 delta;

    private int joyStickId;
    private void Awake()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
    private void Start()
    {
        isMove = false;
        touchIndex = int2.zero;
        joyStick = GetComponent<RectTransform>();
        wrapperRect = wrapper.GetComponent<RectTransform>();
        handlerRect = handler.GetComponent<RectTransform>();
        wrapper.SetActive(false);
        handler.SetActive(false);
        joyStickId = 1;
    }
    public void OnPointerDown(PointerEventData data)
    {

        if (touchIndex.y != joyStickId)
        {

            wrapper.SetActive(true);
            touchIndex = new int2(data.pointerId, joyStickId);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(joyStick, data.position, null, out Vector2 posJoy);
            wrapperRect.anchoredPosition = posJoy;
        }
    }
    public void OnDrag(PointerEventData data)

    {

        if (data.pointerId == touchIndex.x)
        {

            if (!handler.activeSelf)
            {
                isMove = true;
                handler.SetActive(true);
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(wrapperRect, data.position, null, out Vector2 pos);
            delta = data.delta;
            float3 polar = Polar(float2.zero, pos);
            handlerRect.rotation = Quaternion.Euler(0f, 0f, polar.y);
            degrees = polar.z;


        }

    }

    public void OnPointerUp(PointerEventData data)
    {

        if (data.pointerId == touchIndex.x)
        {
            isMove = false;
            touchIndex = int2.zero;
            handler.SetActive(false);
            wrapper.SetActive(false);
        }

    }
    private float3 Polar(float2 centerPos, float2 targetPos)
    {
        float centerX = centerPos.x; // koordinat x pusat
        float centerY = centerPos.y; // koordinat y pusat
        float x = targetPos.x; // koordinat x
        float y = targetPos.y; // koordinat y

        // menghitung nilai jarak dari pusat koordinat
        float deltaX = x - centerX;
        float deltaY = y - centerY;
        float r = math.sqrt(deltaX * deltaX + deltaY * deltaY);

        // menghitung sudut dengan menggunakan fungsi atan2
        float theta = math.atan2(deltaY, deltaX);

        // mengubah sudut dari radian ke derajat
        float angleInDegrees = ((theta * Mathf.Rad2Deg) + 270f) % 360f;
        float angleInDegreesPlayer = ((theta * Mathf.Rad2Deg) + 360f) % 360f;
        return new float3(r, angleInDegrees, angleInDegreesPlayer);
    }

    public void KillJoyStick()
    {
        isMove = false;
        touchIndex = int2.zero;
        handler.SetActive(false);
        wrapper.SetActive(false);
    }


}

// [BurstCompile]
// public struct PolarJob : IJob
// {
//     public float2 centerPos;
//     public float2 targetPos;
//     public NativeArray<float3> result;
//     public void Execute()
//     {
//         float centerX = centerPos.x; // koordinat x pusat
//         float centerY = centerPos.y; // koordinat y pusat
//         float x = targetPos.x; // koordinat x
//         float y = targetPos.y; // koordinat y

//         // menghitung nilai jarak dari pusat koordinat
//         float deltaX = x - centerX;
//         float deltaY = y - centerY;
//         float r = math.sqrt(deltaX * deltaX + deltaY * deltaY);

//         // menghitung sudut dengan menggunakan fungsi atan2
//         float theta = math.atan2(deltaY, deltaX);

//         // mengubah sudut dari radian ke derajat
//         float angleInDegrees = ((theta * Mathf.Rad2Deg) + 270f) % 360f;
//         float angleInDegreesPlayer = ((theta * Mathf.Rad2Deg) + 360f) % 360f;
//         result[0] = new float3(r, angleInDegrees, angleInDegreesPlayer);
//     }

// }
