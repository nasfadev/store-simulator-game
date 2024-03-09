using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class VariousThingsGuide : MonoBehaviour
{
    [SerializeField] private LayerMask Mask;
    [SerializeField] private LayerMask EditMask;
    [SerializeField] private float moveTweenTime;
    [SerializeField] private float rotateTweenTime;

    private Coroutine runCoroutine;
    [HideInInspector] public GameObject prefab;

    private int tempRotateId;
    [HideInInspector] public VariousThingsGuidePrefab variousThingsGuidePrefab;
    [HideInInspector] public TempVariousThing tempVariousThings;
    [HideInInspector] public VariousThings variousThings;
    [HideInInspector] public Vector3 posForBuilder;
    [HideInInspector] public bool isDelete;
    [HideInInspector] public bool isMove;
    private Vector3 tempPos;

    public static VariousThingsGuide Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Run()
    {
        if (runCoroutine != null)
        {
            StopCoroutine(runCoroutine);
        }

        if (PlaceButton.Instance.contentID > 0)
        {
            InstantiatePrefab(PlaceButton.Instance.contentID);
        }
        runCoroutine = StartCoroutine(RunIE());

    }
    public struct TempVariousThing
    {
        public int id;
        public int index;
    }
    public void Stop()
    {
        if (runCoroutine != null)
        {
            StopCoroutine(runCoroutine);

            Destroy(prefab);

        }
    }
    private void InstantiatePrefab(int id, int index)
    {
        PlaceButton placeButton = PlaceButton.Instance;

        VariousThingsProductData.Data productData = VariousThingsProductData.Instance.data[id - 1];
        VariousThingsBuilder.VariousThingsData dataSave = VariousThingsBuilder.VariousThingsDataSave.instance.data[index];
        RotateButton.rotateID = dataSave.rotateId;
        posForBuilder = dataSave.position.ToVector3();
        prefab = Instantiate(
                   productData.prefab,
                 dataSave.position.ToVector3(),
                  Quaternion.Euler(new Vector3(0f, 90 * (dataSave.rotateId - 1), 0f)),
                  transform);
        Cashier cashier = prefab.GetComponent<Cashier>();
        if (cashier != null)
        {
            Destroy(cashier);
        }
        variousThingsGuidePrefab = prefab.AddComponent<VariousThingsGuidePrefab>();

    }
    private void InstantiatePrefab(int id)
    {
        PlaceButton placeButton = PlaceButton.Instance;

        VariousThingsProductData.Data productData = VariousThingsProductData.Instance.data[id - 1];
        prefab = Instantiate(
                   productData.prefab,
                 Vector3.zero,
                  Quaternion.Euler(new Vector3(0f, 90 * (RotateButton.rotateID - 1), 0f)),
                  transform);
        Cashier cashier = prefab.GetComponent<Cashier>();
        if (cashier != null)
        {
            Destroy(cashier);
        }
        variousThingsGuidePrefab = prefab.AddComponent<VariousThingsGuidePrefab>();

    }
    private IEnumerator RunIE()
    {
        while (true)
        {
            if (PlaceButton.Instance.contentID > 0)
            {
                yield return Add(PlaceButton.Instance.contentID);

            }
            else if (PlaceButton.Instance.contentID == 0)
            {
                yield return Delete();
            }
            else if (PlaceButton.Instance.contentID == -1)
            {
                yield return Move();
            }

        }
    }
    private IEnumerator Add(int id)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, Mask))
        {

            WhenRayTouched(hit, id);

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
                    AutoDelete.Instance.isDeleteTouched = true;

                    variousThings = hit.transform.GetComponent<VariousThings>();
                    isDelete = true;


                }
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                break;
            }


            yield return null;
        }

    }
    private IEnumerator Move()
    {
        while (true)
        {
            // tunggu sampe di pencet tombol placebuton
            if (PlaceButton.Instance.isExecute)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, EditMask))
                {

                    PlaceButton.Instance.isTouchedWhenMoveMode = true;

                    VariousThings tempVT = hit.transform.GetComponent<VariousThings>();
                    // buat salinan dari variousthings
                    tempVariousThings = new TempVariousThing();
                    tempVariousThings.id = tempVT.id;
                    tempVariousThings.index = tempVT.index;

                    Destroy(hit.transform.gameObject);

                    yield return null;
                    // ini untuk recombine mesh, soalnya kalo gak di gituaan mesh lama masih terlihat bahkan jika sudah di hapus pun
                    VariousThingsBuilder.Instance.CombineMeshsis();

                    // ini ngebuat objek untuk guide
                    InstantiatePrefab(tempVariousThings.id, tempVariousThings.index);
                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                    AutoMove.Instance.isMoveTouched = true;
                    while (true)
                    {
                        // nah ini tuh untuk pencet yang kedua kali atau untuk tempat pindahan baru
                        if (PlaceButton.Instance.isExecute && !isMove)
                        {
                            if (variousThingsGuidePrefab.isCantPlace)
                            {
                                PlaceButton.Instance.isExecute = false;
                                PlaceButton.Instance.isRun = false;
                                yield return null;

                                continue;
                            }
                            Debug.Log("udah gw exut cuy");
                            isMove = true;
                            PlaceButton.Instance.isExecute = false;
                            PlaceButton.Instance.isRun = false;

                            // disini bakal matiin fungsi move sih
                            break;
                        }
                        yield return Add(tempVariousThings.id);
                        yield return null;

                    }

                }
                else
                {
                    PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;

                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                }

                break;
            }


            yield return null;
        }

    }
    private void WhenRayTouched(RaycastHit hit, int id)
    {
        VariousThingsProductData.Data productData = VariousThingsProductData.Instance.data[id - 1];

        if (tempRotateId != RotateButton.rotateID)
        {
            prefab.transform.DORotate(Vector3.up * (90 * (RotateButton.rotateID - 1)), rotateTweenTime).SetEase(Ease.InOutQuad);
            tempRotateId = RotateButton.rotateID;
            posForBuilder = GetPosition(RotateButton.rotateID, hit.point, productData.evenOdd);
            prefab.transform.DOMove(posForBuilder, moveTweenTime).SetEase(Ease.InOutQuad);


        }
        if (tempPos == hit.point)
        {
            return;
        }



        tempPos = hit.point;
        posForBuilder = GetPosition(RotateButton.rotateID, hit.point, productData.evenOdd);
        prefab.transform.DOMove(posForBuilder, moveTweenTime).SetEase(Ease.InOutQuad);

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
            return Mathf.Ceil(pos - .5f) - 0f;

        }

        return Mathf.Ceil(pos - 0f) - .5f;


    }
}
