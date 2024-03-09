using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.Mathematics;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class PlaceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool isExecute;
    [HideInInspector]
    public int contentID;
    [HideInInspector]
    public string mode;
    [HideInInspector]
    public bool isRun;
    public bool isMoving;
    public bool isTouchedWhenMoveMode;
    public bool isTouchedWhenMoveModeLoaded;
    public bool isMovingAddedWhenMoveMode;
    public bool isMovingAddedCancelWhenMoveMode;
    public bool isMoveWhenMoveMode;
    public event Action TouchedWhenModeMode;
    public bool isSelectedBeforeMove;
    public bool isPlacedAfterMove;
    public bool isDoneMove;
    [SerializeField] private Image image;
    [SerializeField] private GameObject slotButton;
    [SerializeField] private Sprite placeImage;
    [SerializeField] private Sprite moveImage;
    [SerializeField] private Sprite deleteImage;
    [SerializeField] private UnityEvent OnDown;
    [SerializeField] private UnityEvent OnUp;
    [SerializeField] private UnityEvent OnTouchedWhenMoveMode;
    public static PlaceButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Update()
    {
#if UNITY_EDITOR
        OnPointerDownForWindows();

#elif UNITY_STANDALONE_WIN
                    OnPointerDownForWindows();


#elif UNITY_ANDROID
      return;
#endif

    }
    private void OnPointerDownForWindows()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!isRun)
            {

                OnDown?.Invoke();

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isRun)
            {

                isExecute = true;


                isRun = true;
                OnUp?.Invoke();



            }
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (!isRun)
        {

            OnDown?.Invoke();

        }


    }
    public void OnPointerUp(PointerEventData data)
    {
        if (!isRun)
        {

            isExecute = true;


            isRun = true;
            OnUp?.Invoke();



        }


        // Debug.Log($"flooor : {builderId.x} , {builderId.y}");
    }
    public void MoveMode()
    {
        image.sprite = moveImage;

    }
    public void ChangeContentID(int newContentID)
    {
        contentID = newContentID;
        ChangeImage();
        Debug.Log($"flooor : {contentID}");
        WhenMoveMode();


    }
    public void ChangeMode(string newMode)
    {
        mode = newMode;
        Debug.Log($"flooor : {mode}");


    }

    private void ChangeImage()
    {
        Debug.Log($"egbug content : {contentID}");

        switch (contentID)
        {
            case -1:
                image.sprite = moveImage;
                break;
            case 0:
                image.sprite = deleteImage;
                break;
            default:
                image.sprite = placeImage;
                break;
        }

    }
    private IEnumerator ChangeImageWhenMove()
    {

        while (true)
        {
            if (isSelectedBeforeMove)
            {
                image.sprite = placeImage;
                isSelectedBeforeMove = false;
                slotButton.SetActive(false);
                while (true)
                {
                    if (isPlacedAfterMove)
                    {
                        image.sprite = moveImage;
                        isPlacedAfterMove = false;
                        slotButton.SetActive(true);
                        break;
                    }
                    yield return null;

                }
            }
            if (isDoneMove)
            {
                isDoneMove = false;
                break;

            }
            Debug.Log("isMovingAddedWhenMoveMode");

            yield return null;
        }
        // if (isMoving)
        // {
        //     image.sprite = moveImage;
        //     isMoving = false;
        //     PlacedWhenMoveMode?.Invoke();

        // }
        // else
        // {
        //     yield return WhenTouchedMoveModeLoaded();
        // }

    }
    private IEnumerator WhenTouchedMoveModeLoaded()
    {

        while (true)
        {
            if (isTouchedWhenMoveModeLoaded)
            {
                isTouchedWhenMoveModeLoaded = false;
                break;
            }
            Debug.Log("isTouchedWhenMoveModeLoaded");
            yield return null;
        }



        if (!isMoving & contentID == -1 && isTouchedWhenMoveMode)
        {
            image.sprite = placeImage;
            TouchedWhenModeMode?.Invoke();
            isTouchedWhenMoveMode = false;

        }


    }
    private void WhenMoveMode()
    {
        if (contentID == -1)
        {
            StartCoroutine(ChangeImageWhenMove());

        }
    }

}
