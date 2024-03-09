using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoMove : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;
    [SerializeField] private UnityEvent sellingPlatformMove;
    [SerializeField] private UnityEvent sellingPlatformMoveDone;
    [SerializeField] private UnityEvent cashierMove;
    [SerializeField] private UnityEvent cashierMoveDone;
    [SerializeField] private UnityEvent variousThingsMove;
    [SerializeField] private UnityEvent variousThingsMoveDone;
    private Coroutine runCoroutine;
    private AutoMoveAble tempAutoMoveAble;

    [HideInInspector] public bool isMoveTouched;
    public event Action onMoveTouched;
    public event Action onMoveTouchedDone;
    public static AutoMove Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Run()
    {
        PlaceButton.Instance.MoveMode();
        runCoroutine = StartCoroutine(RunIE());
    }
    public void Stop()
    {
        if (runCoroutine != null) StopCoroutine(runCoroutine);


        if (tempAutoMoveAble != null) tempAutoMoveAble.autoMoveBox.SetActive(false);
        tempAutoMoveAble = null;
    }

    private IEnumerator RunIE()
    {

        while (!StateLoaded.isLoaded)
        {
            yield return null;
        }
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoMoveAble autoMoveAble = hit.transform.GetComponent<AutoMoveAble>();
                if (autoMoveAble == null)
                {
                    yield return null;
                    continue;
                }
                if (tempAutoMoveAble != autoMoveAble)
                {
                    if (tempAutoMoveAble != null) tempAutoMoveAble.autoMoveBox.SetActive(false);
                    tempAutoMoveAble = autoMoveAble;
                    tempAutoMoveAble.autoMoveBox.SetActive(true);
                }
                Debug.Log($"{tempAutoMoveAble.mode} automove");

                if (tempAutoMoveAble.mode == "SellingPlatformBuilder")
                {
                    yield return WhenTheTypeSellingPlatform(tempAutoMoveAble.mode);
                }
                else if (tempAutoMoveAble.mode == "CashierBuilder")
                {
                    yield return WhenTheTypeCashier(tempAutoMoveAble.mode);

                }
                else if (tempAutoMoveAble.mode == "VariousThingsBuilder")
                {
                    yield return WhenTheTypeVariousThings(tempAutoMoveAble.mode);

                }
                else
                {
                    if (PlaceButton.Instance.isExecute)
                    {
                        PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;

                        PlaceButton.Instance.isExecute = false;
                        PlaceButton.Instance.isRun = false;
                    }

                }


            }
            else
            {
                if (tempAutoMoveAble != null) tempAutoMoveAble.autoMoveBox.SetActive(false);
                tempAutoMoveAble = null;
                if (PlaceButton.Instance.isExecute)
                {
                    PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;

                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                }
            }

            yield return null;
        }

    }
    private IEnumerator WhenTheTypeSellingPlatform(string mode)
    {
        PlaceButton.Instance.ChangeContentID(-1);
        PlaceButton.Instance.ChangeMode(mode);
        sellingPlatformMove?.Invoke();

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (isMoveTouched)
            {
                SellingPlatformBlueprint.Instance.isMovingRun = true;
                onMoveTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isMoveTouched)
                {
                    yield return null;
                }
                onMoveTouchedDone?.Invoke();
                sellingPlatformMoveDone?.Invoke();
                SellingPlatformBlueprint.Instance.isMovingRun = false;

            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoMoveAble autoMoveAble = hit.transform.GetComponent<AutoMoveAble>();
                if (tempAutoMoveAble != autoMoveAble)
                {
                    sellingPlatformMoveDone?.Invoke();
                    PlaceButton.Instance.isDoneMove = true;

                    break;
                }
            }
            else
            {
                sellingPlatformMoveDone?.Invoke();
                PlaceButton.Instance.isDoneMove = true;

                break;
            }
            yield return null;
        }
    }
    private IEnumerator WhenTheTypeCashier(string mode)
    {
        PlaceButton.Instance.ChangeContentID(-1);
        PlaceButton.Instance.ChangeMode(mode);

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (PlaceButton.Instance.isExecute && !isMoveTouched)
            {
                isMoveTouched = true;
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                onMoveTouched?.Invoke();
                cashierMove?.Invoke();
                Debug.Log($"{mode} automove");
                while (isMoveTouched)
                {
                    yield return null;
                }
                onMoveTouchedDone?.Invoke();
                cashierMoveDone?.Invoke();

            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoMoveAble autoMoveAble = hit.transform.GetComponent<AutoMoveAble>();
                if (tempAutoMoveAble != autoMoveAble)
                {
                    cashierMoveDone?.Invoke();
                    break;
                }
            }
            else
            {
                cashierMoveDone?.Invoke();
                break;
            }
            yield return null;
        }
    }
    private IEnumerator WhenTheTypeVariousThings(string mode)
    {
        PlaceButton.Instance.ChangeContentID(-1);
        PlaceButton.Instance.ChangeMode(mode);
        variousThingsMove?.Invoke();

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (isMoveTouched)
            {
                PlaceButton.Instance.isSelectedBeforeMove = true;

                onMoveTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isMoveTouched)
                {
                    yield return null;
                }
                onMoveTouchedDone?.Invoke();
                variousThingsMoveDone?.Invoke();
                PlaceButton.Instance.isPlacedAfterMove = true;


            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoMoveAble autoMoveAble = hit.transform.GetComponent<AutoMoveAble>();
                if (tempAutoMoveAble != autoMoveAble)
                {
                    variousThingsMoveDone?.Invoke();
                    PlaceButton.Instance.isDoneMove = true;

                    break;
                }
            }
            else
            {
                variousThingsMoveDone?.Invoke();
                PlaceButton.Instance.isDoneMove = true;

                break;
            }
            yield return null;
        }
    }


}
