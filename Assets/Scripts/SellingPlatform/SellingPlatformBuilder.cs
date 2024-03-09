using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using ThirtySec;
using TMPro;

public class SellingPlatformBuilder : MonoBehaviour
{
    [SerializeField] private int dimension;
    [SerializeField] private TemplateData templateData;
    [SerializeField] private StoreData storeData;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int maxSellingPlatformPerChunk;
    [SerializeField] private SellingPlatformProductData spProductData;
    [SerializeField] private TextMeshProUGUI costPriceText;
    private int maxChunk;
    private bool Loaded;
    public static SellingPlatformBuilder Instance;
    public event Action OnRendering;
    public event Action OnRendered;
    public event Action Run;
    public event Action Stop;
    public event Action Reset;
    [HideInInspector]
    public bool isCollide;
    private int[] chunkCount;
    private GameObject[] chunkPrefabs;
    private int tempMoveIndex;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        maxChunk = (((dimension * dimension) / 2) / maxSellingPlatformPerChunk) + 5;
        Debug.Log(maxChunk);
        chunkCount = new int[maxChunk];
        chunkPrefabs = new GameObject[maxChunk];
        if (SellingPlatformDataSave.IsExist() && SellingPlatformDataSave.instance != null && UnityEngine.PlayerPrefs.GetInt("shelvesPass") == SellingPlatformDataSave.instance.pass)
        {
            StartCoroutine(LoadData());
        }
        else
        {
            StateLoaded.initLoaded++;

            // SellingPlatformDataSave.instance.spData = new SPData();
            // SellingPlatformDataSave.instance.spData.datas = new SellingPlatformData[dimension * dimension];
            // SellingPlatformData[] templates = JsonUtility.FromJson<SellingPlatformData[]>(template.sellingPlatform);
            // Debug.Log(templates.Length + "template panjang");

            SellingPlatformDataSave.instance.spData = JsonUtility.FromJson<SPData>(templateData.sellingPlatform);
            int seed = (int)System.DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int pass = UnityEngine.Random.Range(1000, 3000);
            UnityEngine.PlayerPrefs.SetInt("shelvesPass", pass);
            SellingPlatformDataSave.instance.pass = pass;
            Debug.Log(SellingPlatformDataSave.instance.spData.datas.Length + "leng seng" + "\":2,");
            StartCoroutine(LoadData());
            // if (!Loaded)
            // {
            //     Loaded = true;
            //     StartCoroutine(CheckStateLoadedRequirement());

            // }


        }

    }
    public class SellingPlatformDataSave : ThirtySec.Serializable<SellingPlatformDataSave>
    {
        public int pass;
        public SPData spData;

    }
    [System.Serializable]
    public class SPData
    {
        public SellingPlatformData[] datas;
    }

    public void RunSellingPlatformBuilder()
    {
        Run?.Invoke();
        gameObject.SetActive(true);
        SetBlueprint(PlaceButton.Instance.contentID - 1);
        StartCoroutine(EnableSellingPlatformBuilder());

    }
    public void ResetData()
    {
        Reset?.Invoke();
    }
    private IEnumerator EnableSellingPlatformBuilder()
    {
        if (PlaceButton.Instance.contentID > 0)
        {
            costPriceText.text = $"{storeData.data.sellingPlatform[PlaceButton.Instance.contentID - 1].quantity}x";

        }

        while (true)
        {

            yield return AddingOperation();
            yield return DeletingOperation();
            if (PlaceButton.Instance.contentID == -1 && SellingPlatformBlueprint.Instance.isMovingRun)
            {
                yield return MovingOperation();

            }


        }
    }
    private IEnumerator AddingOperation()
    {
        PlaceButton place = PlaceButton.Instance;

        if (place.isExecute && place.contentID > 0 && place.mode == "SellingPlatformBuilder")
        {
            Debug.Log("hit it");
            if (storeData.data.sellingPlatform[place.contentID - 1].quantity == 0)
            {
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
            }

            else if (isCollide)
            {
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                Debug.Log("collide");

            }
            else
            {
                SellingPlatformBlueprint blueprint = SellingPlatformBlueprint.Instance;
                SellingPlatformData data = new SellingPlatformData(
                    id: place.contentID,
                    stock: spProductData.data[place.contentID - 1].maxStockQuantity,
                    rot: getRotate(RotateButton.rotateID),
                    pos: blueprint.currentPosition
                    );

                int index = ((int)((dimension * blueprint.currentPosition.z) + blueprint.currentPosition.x));
                SellingPlatformDataSave.instance.spData.datas[index] = data;

                RenderChunk(InstantiateSellingPlatform(data, index));
                Debug.Log($" index : {index}");

                Debug.Log($"location {blueprint.currentPosition}");
                storeData.data.sellingPlatform[place.contentID - 1].quantity--;
                costPriceText.text = $"{storeData.data.sellingPlatform[place.contentID - 1].quantity}x";

                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;

            }

        }
        yield return null;
    }
    private IEnumerator MovingAddingOperation()
    {
        PlaceButton place = PlaceButton.Instance;

        if (place.isExecute && SellingPlatformBlueprint.Instance.isMoving && place.mode == "SellingPlatformBuilder" && SellingPlatformBlueprint.Instance.isMovingRun)
        {
            PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;

            Debug.Log("hit it");

            if (isCollide)
            {
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                PlaceButton.Instance.isMovingAddedCancelWhenMoveMode = true;
                PlaceButton.Instance.isMovingAddedWhenMoveMode = true;

                Debug.Log("collide");

            }
            else
            {
                SellingPlatformBlueprint blueprint = SellingPlatformBlueprint.Instance;

                SellingPlatformData dataTransfer = blueprint.dataTransfer;
                SellingPlatformData data = new SellingPlatformData(
                    id: dataTransfer.IDsp,
                    stock: dataTransfer.stockQuantity,
                    rot: getRotate(RotateButton.rotateID),
                    pos: blueprint.currentPosition
                    );
                int index = ((int)((dimension * blueprint.currentPosition.z) + blueprint.currentPosition.x));

                SellingPlatformDataSave.instance.spData.datas[blueprint.sellingPlatformInfo.index].IDsp = 0;

                SellingPlatformDataSave.instance.spData.datas[index] = data;

                RenderChunk(InstantiateSellingPlatform(data, index));



                Debug.Log($"index {index}");
                SellingPlatformBlueprint.Instance.isMoving = false;
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                PlaceButton.Instance.isMoving = true;
                PlaceButton.Instance.isMovingAddedWhenMoveMode = true;
                AutoMove.Instance.isMoveTouched = false;
                PlaceButton.Instance.isPlacedAfterMove = true;




            }

        }
        else
        {
            if (PlaceButton.Instance.isExecute)
            {
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                PlaceButton.Instance.isMovingAddedCancelWhenMoveMode = true;
                PlaceButton.Instance.isMovingAddedWhenMoveMode = true;
            }

        }
        yield return null;
    }
    private IEnumerator DeletingOperation()
    {



        if (SellingPlatformBlueprint.Instance.isDeleting)
        {
            SellingPlatformBlueprint blueprint = SellingPlatformBlueprint.Instance;
            SellingPlatform info = blueprint.sellingPlatformInfo;
            int IDsp = SellingPlatformDataSave.instance.spData.datas[info.index].IDsp;
            int stockQuantity = SellingPlatformDataSave.instance.spData.datas[info.index].stockQuantity;
            storeData.data.stockQuantity[IDsp - 1] += stockQuantity;

            SellingPlatformDataSave.instance.spData.datas[info.index].IDsp = 0;

            Destroy(info.gameObject);
            yield return null;
            RenderChunk(info.chunkID);

            chunkCount[info.chunkID]--;
            if (chunkCount[info.chunkID] == 0)
            {
                Destroy(chunkPrefabs[info.chunkID]);
            }

            SellingPlatformBlueprint.Instance.isDeleting = false;
            AutoDelete.Instance.isDeleteTouched = false;

        }

        yield return null;
    }
    private IEnumerator MovingOperation()
    {



        if (SellingPlatformBlueprint.Instance.isMoving && SellingPlatformBlueprint.Instance.isMovingRun)
        {
            PlaceButton.Instance.isMovingAddedWhenMoveMode = true;
            PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;
            PlaceButton.Instance.isMoveWhenMoveMode = true;

            SellingPlatformBlueprint blueprint = SellingPlatformBlueprint.Instance;
            SellingPlatform info = blueprint.sellingPlatformInfo;

            Destroy(info.gameObject);
            yield return null;
            RenderChunk(info.chunkID);

            chunkCount[info.chunkID]--;
            if (chunkCount[info.chunkID] == 0)
            {
                Destroy(chunkPrefabs[info.chunkID]);
            }

            SetBlueprint(blueprint.dataTransfer.IDsp - 1);


            SellingPlatformBlueprint.Instance.isMoving = false;

            while (true)
            {
                if (SellingPlatformBlueprint.Instance.isMoving)
                {
                    break;
                }
                yield return null;
            }

            while (SellingPlatformBlueprint.Instance.isMovingRun)
            {
                yield return MovingAddingOperation();
            }


        }

        yield return null;
    }
    private void SetBlueprint(int contentID)
    {
        Debug.Log($"contentID now {contentID}");

        if (contentID >= 0 || SellingPlatformBlueprint.Instance.isMoving)
        {

            SellingPlatformBlueprint.Instance.boundsSize = spProductData.data[contentID].EvenOddSize;
            SellingPlatformBlueprint.Instance.prefab = spProductData.data[contentID].prefab;
            SellingPlatformBlueprint.Instance.tempStock = spProductData.data[contentID].maxStockQuantity;
        }


    }
    private IEnumerator LoadData()
    {
        Run?.Invoke();

        for (int i = 0; i < dimension * dimension; i++)
        {
            SellingPlatformData spds = SellingPlatformDataSave.instance.spData.datas[i];

            if (spds.IDsp > 0)
            {
                InstantiateSellingPlatform(spds, i);
                yield return null;
            }

        }
        yield return null;

        RenderAllChunk();
        Stop?.Invoke();
        Reset?.Invoke();
        if (!Loaded)
        {
            Loaded = true;
            StartCoroutine(CheckStateLoadedRequirement());

        }

    }
    private IEnumerator CheckStateLoadedRequirement()
    {
        int numThisState = 4;
        while (!(StateLoaded.Loaded + 1 == numThisState))
        {
            yield return null;
        }
        StateLoaded.Loaded++;

    }
    private int InstantiateSellingPlatform(SellingPlatformData spds, int index)
    {
        for (int j = 0; j < chunkCount.Length; j++)
        {
            if (chunkCount[j] < maxSellingPlatformPerChunk)
            {
                if (chunkCount[j] == 0)
                {
                    chunkPrefabs[j] =
                    Instantiate(
                        chunkPrefab,
                        Vector3.zero,
                        Quaternion.identity,
                        transform);


                }
                GameObject prefab = spProductData.data[spds.IDsp - 1].prefab;
                SellingPlatform data = prefab.GetComponent<SellingPlatform>();
                data.index = index;
                data.chunkID = j;
                data.IDsp = spds.IDsp;
                data.maxStockQuantity = spProductData.data[spds.IDsp - 1].maxStockQuantity;
                Instantiate(
                    prefab,
                   spds.position.ToVector3(),
                    Quaternion.Euler(0f, spds.rotation, 0f),
                    chunkPrefabs[j].transform);



                chunkCount[j]++;

                return j;

            }
        }
        return 0;

    }
    private void RenderChunk(int index)
    {

        OnRendering?.Invoke();
        chunkPrefabs[index].GetComponent<MeshCombiner>().CombineMeshes(false);
        OnRendered?.Invoke();


    }
    private void RenderAllChunk()
    {
        OnRendering?.Invoke();
        for (int i = 0; i < chunkPrefabs.Length; i++)
        {
            if (chunkCount[i] > 0)
            {
                chunkPrefabs[i].GetComponent<MeshCombiner>().CombineMeshes(false);

            }

        }
        OnRendered?.Invoke();



    }
    public void DisableSellingPlatformBuilder()
    {
        Stop?.Invoke();
        StopAllCoroutines();
    }
    // public void Render
    private float getRotate(int id)
    {
        return 90 * (id - 1);
    }

}
