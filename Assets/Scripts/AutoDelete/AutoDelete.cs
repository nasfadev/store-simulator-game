using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoDelete : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;
    [SerializeField] private Material autoDeleteBoxMaterial;

    [SerializeField] private UnityEvent sellingPlatformDelete;
    [SerializeField] private UnityEvent sellingPlatformDeleteDone;

    [SerializeField] private UnityEvent wallDelete;
    [SerializeField] private UnityEvent wallDeleteDone;
    [SerializeField] private UnityEvent variousThingsDelete;
    [SerializeField] private UnityEvent variousThingsDeleteDone;
    [SerializeField] private UnityEvent FloorDelete;
    [SerializeField] private UnityEvent FloorDeleteDone;
    [SerializeField] private UnityEvent RoofFloorDelete;
    [SerializeField] private UnityEvent RoofFloorDeleteDone;
    private Coroutine runCoroutine;
    private AutoDeleteAble tempAutoDeleteAble;
    private Material tempMaterial;
    [HideInInspector] public bool isDeleteTouched;
    public event Action onDeleteTouched;
    public event Action onDeleteTouchedDone;
    public static AutoDelete Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Run()
    {
        runCoroutine = StartCoroutine(RunIE());
    }
    public void Stop()
    {
        if (runCoroutine != null) StopCoroutine(runCoroutine);


        if (tempAutoDeleteAble != null)
        {
            if (tempAutoDeleteAble.autoDeleteBox != null)

            {
                tempAutoDeleteAble.autoDeleteBox.SetActive(false);

                tempAutoDeleteAble.meshRenderer.sharedMaterial = tempMaterial;
            }
        }
        tempAutoDeleteAble = null;
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
                AutoDeleteAble autoDeleteAble = hit.transform.GetComponent<AutoDeleteAble>();
                if (autoDeleteAble == null)
                {
                    Debug.Log($"type the del {autoDeleteAble}");
                    yield return null;

                    continue;
                }
                if (tempAutoDeleteAble != autoDeleteAble)
                {
                    if (tempAutoDeleteAble != null)
                    {
                        if (tempAutoDeleteAble.autoDeleteBox != null)

                        {
                            tempAutoDeleteAble.autoDeleteBox.SetActive(false);

                            tempAutoDeleteAble.meshRenderer.sharedMaterial = tempMaterial;
                        }

                    }
                    tempAutoDeleteAble = autoDeleteAble;
                    if (tempAutoDeleteAble.autoDeleteBox != null)
                    {
                        tempAutoDeleteAble.autoDeleteBox.SetActive(true);
                        tempMaterial = tempAutoDeleteAble.meshRenderer.sharedMaterial;
                        tempAutoDeleteAble.meshRenderer.sharedMaterial = autoDeleteBoxMaterial;
                    }
                }

                Debug.Log($"{tempAutoDeleteAble.mode} automove");

                if (tempAutoDeleteAble.mode == "SellingPlatformBuilder")
                {
                    yield return WhenTheTypeSellingPlatform(tempAutoDeleteAble.mode);
                }
                else if (tempAutoDeleteAble.mode == "DoorAndWindowBuilder")
                {
                    yield return WhenTheTypeWall(tempAutoDeleteAble.mode);

                }
                else if (tempAutoDeleteAble.mode == "VariousThingsBuilder")
                {
                    yield return WhenTheTypeVariousThings(tempAutoDeleteAble.mode);

                }
                else if (tempAutoDeleteAble.mode == "FloorBuilder" || tempAutoDeleteAble.mode == "FloorRoofBuilder")
                {
                    yield return WhenTheTypeFloor(tempAutoDeleteAble.mode);

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
                if (tempAutoDeleteAble != null)
                {
                    if (tempAutoDeleteAble.autoDeleteBox != null)

                    {
                        tempAutoDeleteAble.autoDeleteBox.SetActive(false);

                        tempAutoDeleteAble.meshRenderer.sharedMaterial = tempMaterial;
                    }
                }
                tempAutoDeleteAble = null;
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
        PlaceButton.Instance.ChangeContentID(0);
        PlaceButton.Instance.ChangeMode(mode);
        sellingPlatformDelete?.Invoke();

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (isDeleteTouched)
            {
                onDeleteTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isDeleteTouched)
                {
                    yield return null;
                }
                onDeleteTouchedDone?.Invoke();
                sellingPlatformDeleteDone?.Invoke();

            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoDeleteAble autoDeleteAble = hit.transform.GetComponent<AutoDeleteAble>();
                if (tempAutoDeleteAble != autoDeleteAble)
                {
                    sellingPlatformDeleteDone?.Invoke();
                    break;
                }
            }
            else
            {
                sellingPlatformDeleteDone?.Invoke();
                break;
            }
            yield return null;
        }
    }
    private IEnumerator WhenTheTypeWall(string mode)
    {
        PlaceButton.Instance.ChangeContentID(0);
        PlaceButton.Instance.ChangeMode(mode);
        wallDelete?.Invoke();

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            Debug.Log("wall");
            if (isDeleteTouched)
            {
                onDeleteTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isDeleteTouched)
                {
                    yield return null;
                }
                onDeleteTouchedDone?.Invoke();
                yield return new WaitForSeconds(.1f);

            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoDeleteAble autoDeleteAble = hit.transform.GetComponent<AutoDeleteAble>();
                if (tempAutoDeleteAble != autoDeleteAble)
                {
                    wallDeleteDone?.Invoke();
                    break;
                }
            }
            else
            {
                wallDeleteDone?.Invoke();
                break;
            }
            yield return null;
        }

    }
    private IEnumerator WhenTheTypeVariousThings(string mode)
    {
        PlaceButton.Instance.ChangeContentID(0);
        PlaceButton.Instance.ChangeMode(mode);
        variousThingsDelete?.Invoke();

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            Debug.Log("wall");
            if (isDeleteTouched)
            {
                onDeleteTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isDeleteTouched)
                {
                    yield return null;
                }
                onDeleteTouchedDone?.Invoke();
                variousThingsDeleteDone?.Invoke();

            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoDeleteAble autoDeleteAble = hit.transform.GetComponent<AutoDeleteAble>();
                if (tempAutoDeleteAble != autoDeleteAble)
                {
                    variousThingsDeleteDone?.Invoke();
                    break;
                }
            }
            else
            {
                variousThingsDeleteDone?.Invoke();
                break;
            }
            yield return null;
        }

    }
    private IEnumerator WhenTheTypeFloor(string mode)
    {
        PlaceButton.Instance.ChangeContentID(0);
        PlaceButton.Instance.ChangeMode(mode);
        if (FloorBuilder.Instance.floorLevel == 1)
        {
            RoofFloorDelete?.Invoke();
        }
        else
        {
            FloorDelete?.Invoke();

        }

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            Debug.Log("wall");
            if (isDeleteTouched)
            {
                onDeleteTouched?.Invoke();
                Debug.Log($"{mode} automove");
                while (isDeleteTouched)
                {
                    Debug.Log($"{mode} delete");

                    yield return null;
                }
                onDeleteTouchedDone?.Invoke();



            }

            else if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                AutoDeleteAble autoDeleteAble = hit.transform.GetComponent<AutoDeleteAble>();
                if (tempAutoDeleteAble != autoDeleteAble)
                {
                    RoofFloorDeleteDone?.Invoke();
                    FloorDeleteDone?.Invoke();
                    break;
                }
            }
            else
            {
                RoofFloorDeleteDone?.Invoke();
                FloorDeleteDone?.Invoke();
                break;
            }
            yield return null;
        }

    }




}
