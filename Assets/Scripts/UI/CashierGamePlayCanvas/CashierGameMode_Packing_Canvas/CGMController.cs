using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CGMController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    public GameObject blank;

    private Vector3 polar;
    [HideInInspector] public bool thisTurn;
    public static CGMController Instance;
    [HideInInspector] public RectTransform cardRect;
    [HideInInspector] public RectTransform thisRect;
    [SerializeField] private UnityEvent whenOnDown;
    // [SerializeField] private LayerMask mask
    [SerializeField] private UnityEvent whenSelectTrue;
    [SerializeField] private UnityEvent whenSelectFalse;
    [HideInInspector] public int arrowRoundId;
    [HideInInspector] public bool isTrue;
    private void Awake()
    {
        thisRect = GetComponent<RectTransform>();
        Instance = this;
    }
    // private IEnumerator RunIE()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit, 1000f, mask))
    //     {

    //     }
    // }
    public void OnPointerDown(PointerEventData data)
    {
        if (!thisTurn)
        {
            return;
        }
        whenOnDown?.Invoke();



    }
    public void OnDrag(PointerEventData data)
    {
        if (!thisTurn)
        {
            return;
        }
        ;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, data.position, null, out Vector2 pos);



        polar = Polar(Vector2.zero, pos);
        Vector2 cartesius = Cartesius(Vector2.zero, polar);
        if (polar.x >= 160f)
        {
            Vector2 cartesiusAlt = Cartesius(Vector2.zero, new Vector2(160f, polar.y));

            cardRect.anchoredPosition = cartesiusAlt;

            return;
        }
        cardRect.anchoredPosition = cartesius;
        Debug.Log($"radius : {polar.x}, angle : {polar.y}, angleplay : {polar.z}");

    }

    public void OnPointerUp(PointerEventData data)
    {
        if (!thisTurn)
        {
            return;
        }
        if (arrowRoundId == 0 && polar.z >= 0 && polar.z <= 90 && polar.x >= 160f)
        {
            Debug.Log("angle atas");
            WhenTrue();


        }
        else if (arrowRoundId == 1 && polar.z > 90 && polar.z < 180 && polar.x >= 160f)
        {
            WhenTrue();


            Debug.Log("angle kiri");

        }
        else if (arrowRoundId == 2 && polar.z >= 180 && polar.z <= 270 && polar.x >= 160f)
        {
            WhenTrue();


            Debug.Log("angle bawah");

        }
        else if (arrowRoundId == 3 && polar.z > 270 && polar.z <= 360 && polar.x >= 160f)
        {
            WhenTrue();


            Debug.Log("angle kanan");

        }
        else
        {
            cardRect.anchoredPosition = Vector2.zero;
            whenSelectFalse?.Invoke();

        }



    }
    private void WhenTrue()
    {
        isTrue = true;
        cardRect.anchoredPosition = Vector2.zero;
        whenSelectTrue?.Invoke();
        Handheld.Vibrate();

    }
    private Vector3 Polar(Vector2 centerPos, Vector2 targetPos)
    {
        float centerX = centerPos.x; // koordinat x pusat
        float centerY = centerPos.y; // koordinat y pusat
        float x = targetPos.x; // koordinat x
        float y = targetPos.y; // koordinat y

        // menghitung nilai jarak dari pusat koordinat
        float deltaX = x - centerX;
        float deltaY = y - centerY;
        float r = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // menghitung sudut dengan menggunakan fungsi atan2
        float theta = Mathf.Atan2(deltaY, deltaX);

        // mengubah sudut dari radian ke derajat
        float angleInDegrees = ((theta * Mathf.Rad2Deg) + 360f) % 360f;
        float anglePlay = ((theta * Mathf.Rad2Deg) + 315) % 360f;

        return new Vector3(r, angleInDegrees, anglePlay);
    }
    private Vector2 Cartesius(Vector2 centerPos, Vector2 polar)
    {
        float centerX = centerPos.x;
        float centerY = centerPos.y;
        float radius = polar.x;
        float angleInDegree = polar.y; // dalam derajat

        float angleInRadian = angleInDegree * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angleInRadian) + centerX;
        float y = radius * Mathf.Sin(angleInRadian) + centerY;
        return new Vector2(x, y);
    }
}
