using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class SellingPlatformBlueprint : MonoBehaviour

{
    [SerializeField] private LayerMask placingMask;
    [SerializeField] private LayerMask deletingMask;
    [SerializeField] private LayerMask prefabMask;

    [HideInInspector] public GameObject prefab;
    [HideInInspector] public Vector2 boundsSize;
    [HideInInspector] public Vector3 currentPosition;
    public bool isDeleting;
    public bool isMoving;
    public int tempStock;
    public bool isMovingRun;
    public SellingPlatformData dataTransfer;

    private GameObject blueprint;
    private GameObject blueprintCollider;


    public static SellingPlatformBlueprint Instance;
    [HideInInspector]
    public SellingPlatform sellingPlatformInfo;
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnBlueprint()
    {
        StartCoroutine(RunBlueprint());
    }
    private IEnumerator RunBlueprint()
    {
        yield return null;
        if (PlaceButton.Instance.contentID > 0)
        {

            InstantiateBlueprint(tempStock);

        }
        while (true)
        {
            if (PlaceButton.Instance.contentID > 0)
            {
                yield return PlacingOperation();

            }
            else if (PlaceButton.Instance.contentID == 0)
            {
                yield return DeletingOperation();
            }
            else if (PlaceButton.Instance.contentID == -1)
            {
                yield return MovingOperation();
            }
        }
    }
    private void InstantiateBlueprint(int stock)
    {
        blueprint = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f), transform);
        blueprintCollider = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f), transform);
        Destroy(blueprintCollider.GetComponentInChildren<SellingPlatformStockManager>().gameObject);
        Destroy(blueprintCollider.GetComponent<MeshRenderer>());
        Destroy(blueprintCollider.GetComponent<Rigidbody>());
        Destroy(blueprint.GetComponent<Rigidbody>());
        Destroy(blueprintCollider.GetComponentInChildren<SellingPlatformBuyerStand>().gameObject);
        blueprint.GetComponentInChildren<SellingPlatformStockManager>().InitStocks(stock);



        foreach (BoxCollider item in blueprintCollider.GetComponents<BoxCollider>())
        {
            item.isTrigger = true;
        }
        SellingPlatformBlueprintPrefab sellingPlatformBlueprintPrefab = blueprintCollider.AddComponent<SellingPlatformBlueprintPrefab>();
        sellingPlatformBlueprintPrefab.animatedPrefab = blueprint;
        sellingPlatformBlueprintPrefab.layerMask = prefabMask;
    }
    private IEnumerator PlacingOperation()
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, placingMask))
        {
            currentPosition = GetPosition(RotateButton.rotateID, hit.point, boundsSize);

            blueprintCollider.transform.position = currentPosition;
            blueprint.transform.DOMove(currentPosition + (Vector3.up * .001f), .15f).SetEase(Ease.OutSine);

        }
        yield return null;
    }
    private IEnumerator DeletingOperation()
    {

        if (PlaceButton.Instance.isExecute && PlaceButton.Instance.contentID == 0 && !isDeleting)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, deletingMask))
            {
                AutoDelete.Instance.isDeleteTouched = true;
                sellingPlatformInfo = hit.transform.GetComponent<SellingPlatform>();
                isDeleting = true;
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
            }
            else
            {
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
            }


        }


        yield return null;
    }
    private IEnumerator MovingOperation()
    {

        if (PlaceButton.Instance.isExecute && PlaceButton.Instance.contentID == -1)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, deletingMask))
            {
                AutoMove.Instance.isMoveTouched = true;

                sellingPlatformInfo = hit.transform.GetComponent<SellingPlatform>();
                dataTransfer = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[sellingPlatformInfo.index];
                Debug.Log("sp bluprint run");
                isMoving = true;
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                PlaceButton.Instance.isSelectedBeforeMove = true;

                while (true)
                {
                    if (!isMoving)
                    {
                        InstantiateBlueprint(dataTransfer.stockQuantity);
                        isMoving = true;
                        break;
                    }
                    yield return null;
                }
                while (true)
                {
                    if (!isMoving)
                    {
                        Destroy(blueprint);
                        Destroy(blueprintCollider);
                        break;
                    }
                    yield return PlacingOperation();
                }
            }
            else
            {
                PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;

                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
            }


        }


        yield return null;
    }

    private Vector3 GetPosition(int rotateID, Vector3 point, Vector2 bounds)
    {

        // bounds = bounds - Vector2.one;
        switch (rotateID)
        {
            case 1 or 3:
                // ganjil genap
                return new Vector3(EvenOddGetPos(bounds.x, point.x), 0, EvenOddGetPos(bounds.y, point.z));

            default:
                return new Vector3(EvenOddGetPos(bounds.y, point.x), 0, EvenOddGetPos(bounds.x, point.z));
        }
    }
    private float EvenOddGetPos(float num, float pos)
    {
        if (num % 2 == 0)
        {
            return math.ceil(pos - .5f) - 0f;

        }

        return math.ceil(pos - 0f) - .5f;


    }
    private float getRotate(int id)
    {
        return 90 * (id - 1);
    }
    public void Rotate()
    {
        if (blueprint)
        {
            if (PlaceButton.Instance.mode == "SellingPlatformBuilder")
            {
                blueprint.transform.DORotate(new Vector3(0, getRotate(RotateButton.rotateID), 0), .5f).SetEase(Ease.OutSine);
                blueprintCollider.transform.Rotate(new Vector3(0, 90f, 0));
            }
        }

    }
    public void DestroyBlueprint()
    {
        StopAllCoroutines();
        Destroy(blueprint);
        Destroy(blueprintCollider);
    }

}
