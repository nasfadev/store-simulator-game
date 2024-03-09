using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;
public class DoorAndWindowGuide : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask Mask;
    [SerializeField] private LayerMask EditMask;

    [SerializeField] private DoorAndWindowProductData doorAndWindowProductData;
    [SerializeField] private TextMeshProUGUI costPriceText;
    private Vector3 snapPos;
    [HideInInspector] public Vector3 firstSnapPos;
    [HideInInspector] public Vector3 lastSnapPos;

    public Vector3 tempPos;
    private int tempRotateId;
    private int tempIndex;
    public static DoorAndWindowGuide Instance;
    private int rotateId;
    private bool isTouched;
    private bool isRunning;
    private Coroutine runCoroutine;
    private GameObject prefab;
    [HideInInspector] public DoorAndBuilderGuidePrefab doorAndBuilderGuidePrefab;
    [HideInInspector] public int index;
    [HideInInspector] public int rotateIdButton;
    [HideInInspector] public int rotateIdSaved;
    [HideInInspector] public DoorAndWindow tempDoorAndWindow;
    [HideInInspector] public bool isDelete;
    private int wallCount;


    private void Awake()
    {
        Instance = this;
        rotateId = 1;


        Debug.Log("awake cuy");
    }
    public void Run()
    {

        isRunning = true;
        if (PlaceButton.Instance.contentID > 0)
        {
            UpdateCostPriceText(0);
            InstantiatePrefab();
        }


        runCoroutine = StartCoroutine(RunIE());

    }
    public void Stop()
    {
        isRunning = false;
        if (runCoroutine != null)
        {

            StopCoroutine(runCoroutine);

            Destroy(prefab);

        }
    }


    private IEnumerator RunIE()
    {
        while (true)
        {
            if (PlaceButton.Instance.contentID > 0)
            {
                yield return Add();

            }
            else
            {
                yield return Delete();
            }

        }
    }
    private IEnumerator Add()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, Mask))
        {
            if (PlaceButton.Instance.contentID > 0)
            {
                WhenRayTouched(hit);
            }


        }

        yield return null;
    }
    private IEnumerator Delete()
    {
        while (true)
        {
            if (PlaceButton.Instance.isExecute && !isDelete)
            {

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, EditMask))
                {
                    tempDoorAndWindow = hit.transform.GetComponent<DoorAndWindow>();
                    if (tempDoorAndWindow != null)
                    {
                        AutoDelete.Instance.isDeleteTouched = true;

                        Debug.Log("kena delet");
                        isDelete = true;
                    }
                    else
                    {
                        PlaceButton.Instance.isExecute = false;
                        PlaceButton.Instance.isRun = false;
                    }



                    break;
                }
                else
                {
                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                    break;
                }

            }
            yield return null;
        }

    }
    private void InstantiatePrefab()
    {
        PlaceButton placeButton = PlaceButton.Instance;

        DoorAndWindowProductData.Data productData = DoorAndWindowProductData.Instance.data[placeButton.contentID - 1];

        prefab = Instantiate(
                  productData.isMultipleAdded ? productData.prefabBlueprint : productData.prefab,
                 Vector3.zero,
                  Quaternion.Euler(new Vector3(0f, 90 * (RotateButton.rotateID - 1), 0f)),
                  transform);
        doorAndBuilderGuidePrefab = prefab.AddComponent<DoorAndBuilderGuidePrefab>();




    }
    private void WhenRayTouched(RaycastHit hit)
    {
        PlaceButton placeButton = PlaceButton.Instance;
        DoorAndWindowProductData.Data productData = DoorAndWindowProductData.Instance.data[placeButton.contentID - 1];
        snapPos = hit.point;

        int minX = (int)(Mathf.Floor(snapPos.x));
        int maxY = (int)(Mathf.Floor(snapPos.z));
        int index = ((int)((34 * maxY) + minX));
        tempPos = GetPosition(hit.point, productData.longInMeter);

        if (prefab.transform.position != tempPos || tempIndex != index || tempRotateId != RotateButton.rotateID)
        {
            tempIndex = index;
            tempRotateId = rotateId;
            int rotateID = RotateButton.rotateID;

            SetRotateId(rotateID);
            WhenIsTouchedOrNot();

        }

    }
    private void WhenIsTouchedOrNot()
    {
        if (!isTouched)
        {
            prefab.transform.DOMove(tempPos, .1f).SetEase(Ease.InOutQuad);
        }
        else
        {
            WhenTouched();

        }
    }
    private void WhenTouched()
    {
        Vector3 firstPos = GetPosition(firstSnapPos, 1);
        Vector3 scale = prefab.transform.localScale;

        if (rotateId == 1 || rotateId == 3)
        {
            tempPos = new Vector3(firstPos.x, 0f, tempPos.z);

            if (tempPos.z >= firstPos.z)
            {
                Vector3 ind = tempPos - GetPosition(firstSnapPos, 1);

                UpdateCostPriceText(ind.z);
                prefab.transform.localScale = new Vector3(scale.x, 1, 1 * (ind.z + 1));
                prefab.transform.position = tempPos - new Vector3(0, 0, 1 * (ind.z / 2));
            }
            else
            {
                Vector3 ind = GetPosition(firstSnapPos, 1) - tempPos;
                UpdateCostPriceText(ind.z);
                prefab.transform.localScale = new Vector3(scale.x, 1, 1 * (ind.z + 1));
                prefab.transform.position = tempPos + new Vector3(0, 0, 1 * (ind.z / 2));
            }

        }
        else
        {
            tempPos = new Vector3(tempPos.x, 0f, firstPos.z);

            if (tempPos.x >= firstPos.x)
            {
                Vector3 ind = tempPos - GetPosition(firstSnapPos, 1);
                UpdateCostPriceText(ind.x);
                prefab.transform.localScale = new Vector3(scale.x, 1, 1 * (ind.x + 1));
                prefab.transform.position = tempPos - new Vector3(1 * (ind.x / 2), 0, 0);
            }
            else
            {
                Vector3 ind = GetPosition(firstSnapPos, 1) - tempPos;
                UpdateCostPriceText(ind.x);
                prefab.transform.localScale = new Vector3(scale.x, 1, 1 * (ind.x + 1));
                prefab.transform.position = tempPos + new Vector3(1 * (ind.x / 2), 0, 0);
            }
        }

    }
    private void UpdateCostPriceText(float z)
    {
        DoorAndWindowProductData.Data productData = DoorAndWindowProductData.Instance.data[PlaceButton.Instance.contentID - 1];
        costPriceText.text = $"<sprite name=\"askari\"> {productData.askariPrice * (z + 1)}";
        wallCount = (int)(z) + 1;
    }

    private void SetRotateId(int rotateID)
    {
        if (rotateID == 1 || rotateID == 3)
        {
            if (snapPos.x % 1f <= .5)
            {
                Debug.Log($"WindowAndDoor rotete 1");
                rotateId = 1;

            }
            else
            {
                Debug.Log($"WindowAndDoor rotete 3");
                rotateId = 3;


            }
        }
        else
        {
            if (snapPos.z % 1f <= .5)
            {
                Debug.Log($"WindowAndDoor rotete 4");
                rotateId = 4;

            }
            else
            {
                Debug.Log($"WindowAndDoor rotete 2");
                rotateId = 2;

            }
        }
    }
    public void SaveRotateId()
    {
        rotateIdSaved = rotateId;
        rotateIdButton = RotateButton.rotateID;
    }
    public void GetFirstSnapPos()
    {
        if (PlaceButton.Instance.mode != "DoorAndWindowBuilder")
        {
            return;
        }
        if (!isRunning || PlaceButton.Instance.contentID < 1)
        {
            return;
        }
        DoorAndWindowProductData.Data productData = DoorAndWindowProductData.Instance.data[PlaceButton.Instance.contentID - 1];


        if (productData.isMultipleAdded)
        {
            firstSnapPos = snapPos;
            isTouched = true;

            return;
        }
        firstSnapPos = snapPos;

    }
    public void GetLastSnapPos()
    {
        if (PlaceButton.Instance.mode != "DoorAndWindowBuilder")
        {
            return;
        }
        if (!isRunning || PlaceButton.Instance.contentID < 1)
        {
            return;
        }
        DoorAndWindowProductData.Data productData = DoorAndWindowProductData.Instance.data[PlaceButton.Instance.contentID - 1];


        if (productData.isMultipleAdded)
        {
            lastSnapPos = snapPos;
            isTouched = false;
            prefab.transform.localScale = new Vector3(1.0005f, 1f, 1f);
        }
        else
        {
            lastSnapPos = firstSnapPos;

        }
        UpdateCostPriceText(0);
        int askariPrice = productData.askariPrice * wallCount;
        int moriumPrice = productData.moriumPrice * wallCount;
        if (!EconomyCurrency.Instance.CanAskariDecrease(askariPrice))
        {
            Debug.Log("askari noo");
            PlaceButton.Instance.isRun = false;
            PlaceButton.Instance.isExecute = false;
        }
        if (!EconomyCurrency.Instance.CanMoriumDecrease(moriumPrice))
        {
            Debug.Log("askari noo");
            PlaceButton.Instance.isRun = false;
            PlaceButton.Instance.isExecute = false;
        }

    }
    public void Rotate()
    {
        if (PlaceButton.Instance.mode != "DoorAndWindowBuilder")
        {
            return;
        }
        prefab.transform.DORotate(Vector3.up * (90 * (RotateButton.rotateID - 1)), .1f).SetEase(Ease.InOutQuad);

    }
    private Vector3 GetPosition(Vector3 point, int longInMeter)
    {
        if (longInMeter % 2 == 0)
        {

            float x = Mathf.Floor(point.x - .5f) + 1f;
            float y = Mathf.Floor(point.z - .5f) + 1f;

            SetIndex(longInMeter, point);
            return new Vector3(x, 0, y);
        }

        else
        {
            if (RotateButton.rotateID == 1 || RotateButton.rotateID == 3)
            {
                float x = Mathf.Floor(point.x - .5f) + 1f;
                float y = Mathf.Floor(point.z) + .5f;
                int index = ((int)((34 * y) + x));

                SetIndexOdd(longInMeter, point);
                return new Vector3(x, 0, y);

            }
            else
            {
                float x = Mathf.Floor(point.x) + .5f;
                float y = Mathf.Floor(point.z - .5f) + 1f;
                int index = ((int)((34 * y) + x));
                SetIndexOdd(longInMeter, point);
                return new Vector3(x, 0, y);

            }



        }


    }
    private void SetIndex(int longInMeter, Vector3 point)
    {

        float x = Mathf.Floor(point.x - .5f) + 1f;
        float y = Mathf.Floor(point.z - .5f) + 1f;
        if (RotateButton.rotateID == 1 || RotateButton.rotateID == 3)
        {
            index = ((int)((34 * (y - (longInMeter / 2))) + x));
            Debug.Log($"index cek kons {index}");
            Debug.Log($"rotate ID {rotateId}");

        }
        else
        {
            index = ((int)((34 * y) + (x - (longInMeter / 2))));
            Debug.Log($"index cek kons {index}");
            Debug.Log($"rotate ID {rotateId}");

        }
    }
    private void SetIndexOdd(int longInMeter, Vector3 point)
    {

        float x = Mathf.Floor(point.x);
        float y = Mathf.Floor(point.z);
        if (RotateButton.rotateID == 1 || RotateButton.rotateID == 3)
        {
            index = ((int)((34 * (y - (longInMeter / 2))) + x));
            Debug.Log($"index cek kons {index}");
            Debug.Log($"rotate ID {rotateId}");

        }
        else
        {
            index = ((int)((34 * y) + (x - (longInMeter / 2))));
            Debug.Log($"index cek kons {index}");
            Debug.Log($"rotate ID {rotateId}");

        }
    }

}
