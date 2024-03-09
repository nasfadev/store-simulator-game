using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class BagCardGenerator : MonoBehaviour
{
    [Header("Required Things")]

    [SerializeField] private GameObject templatePrefab;
    [SerializeField] private RectTransform land;
    [SerializeField] private GameObject emptyText;
    [SerializeField] private GameObject waitText;
    [SerializeField] private StoreData storeData;

    [SerializeField] private Canvas cardCanvas;
    [Header("Product Data")]

    [SerializeField] private SellingPlatformProductData sellingPlatform;
    [SerializeField] private VariousThingsProductData variousThings;
    [SerializeField] private TrashProductData trash;
    [Header("generalOnDown")]

    [SerializeField] private UnityEvent generalOnDown;
    [Header("Selling Platform Events")]
    [SerializeField] private UnityEvent sellingPlatformOnUp;
    [SerializeField] private UnityEvent sellingPlatformAfterOnUp;

    [Header("Various Things Events")]
    [SerializeField] private UnityEvent variousThingssOnUp;
    [SerializeField] private UnityEvent variousThingssAfterOnUp;

    [Header("Trash Events")]
    [SerializeField] private UnityEvent trashOnUp;
    [Header("Bag Data")]

    [SerializeField] private List<Data> data;
    public static BagCardGenerator Instance;
    private Type typeNow = Type.SellingPlatform;
    private GameObject[] tempDelete;
    private List<GameObject> tempAdd;
    [SerializeField] public bool isGenerating;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CheckDataSave();
        Run();
    }
    private void CheckDataSave()
    {
        data = storeData.data.sellingPlatform;
        int productDataCount = sellingPlatform.data.Length;
        UpdateDataSave(productDataCount);
        data = storeData.data.variousThings;
        productDataCount = variousThings.data.Length;
        UpdateDataSave(productDataCount);
        data = storeData.data.trash;
        productDataCount = trash.dataGrabAbles.Length;
        UpdateDataSave(productDataCount);


    }
    private void UpdateDataSave(int productDataCount)
    {
        if (data.Count < productDataCount)
        {
            int offsetDataCount = productDataCount - data.Count;
            for (int i = 0; i < offsetDataCount; i++)
            {
                Data dataItem = new Data();
                dataItem.quantity = 0;
                data.Add(dataItem);
            }
        }
    }
    private void AddDataSave(int productDataCount)
    {
        for (int i = 0; i < productDataCount; i++)
        {
            Data dataItem = new Data();
            dataItem.quantity = 0;
            data.Add(dataItem);
        }
    }
    // private void InitDataSave()
    // {
    //     BagDataSave.instance.sellingPlatform = new List<Data>();
    //     data = BagDataSave.instance.sellingPlatform;
    //     int productDataCount = sellingPlatform.data.Length;
    //     AddDataSave(productDataCount);
    //     BagDataSave.instance.variousThings = new List<Data>();
    //     data = BagDataSave.instance.variousThings;
    //     productDataCount = variousThings.data.Length;
    //     AddDataSave(productDataCount);
    //     BagDataSave.instance.trash = new List<Data>();
    //     data = BagDataSave.instance.trash;
    //     productDataCount = trash.dataGrabAbles.Length;
    //     AddDataSave(productDataCount);

    // }

    [System.Serializable]
    public class Data
    {
        public int quantity;
    }
    public enum Type
    {
        SellingPlatform,
        WorkerThings,
        Accessories,
        Trash
    }
    public void ChangeType(BagChangeType bagChangeType)
    {
        typeNow = bagChangeType.type;

        Run();
    }
    public void Run()
    {
        if (isGenerating)
        {
            return;
        }
        isGenerating = true;


        if (tempAdd != null)
        {
            tempDelete = tempAdd.ToArray();
        }

        tempAdd = new List<GameObject>();

        StartCoroutine(Generate());

    }
    private IEnumerator Generate()
    {
        int itemHaveQuantity = 0;
        cardCanvas.enabled = false;
        emptyText.SetActive(false);
        waitText.SetActive(true);
        if (tempDelete != null)
        {
            for (int i = 0; i < tempDelete.Length; i++)
            {
                Destroy(tempDelete[i]);
                yield return null;

            }
        }
        if (typeNow == Type.SellingPlatform)
        {
            data = storeData.data.sellingPlatform;
            for (int i = 0; i < data.Count; i++)
            {

                if (data[i].quantity > 0)
                {
                    SellingPlatformProductData.Data spData = sellingPlatform.data[i];
                    BagCard bagCard = templatePrefab.GetComponentInChildren<BagCard>();
                    bagCard.imageSprite = spData.prefabImageSprite;
                    bagCard.titleTranslationName = spData.titleTranslationName;
                    bagCard.StockQuantity = data[i].quantity;
                    bagCard.IDspReady = i;
                    bagCard.mode = "SellingPlatformBuilder";
                    bagCard.OnDown = generalOnDown;
                    bagCard.OnUp = sellingPlatformOnUp;
                    bagCard.AfterOnUp = sellingPlatformAfterOnUp;
                    GameObject prefab = Instantiate(templatePrefab, Vector3.zero, Quaternion.identity, land);
                    tempAdd.Add(prefab);
                    itemHaveQuantity++;
                    yield return null;
                }

            }

        }
        else if (typeNow == Type.WorkerThings)
        {
            data = storeData.data.variousThings;
            for (int i = 0; i < data.Count; i++)
            {

                if (data[i].quantity > 0 && (variousThings.data[i].type == VariousThingsProductData.Type.WorkerThings || variousThings.data[i].type == VariousThingsProductData.Type.Cashier))
                {
                    VariousThingsProductData.Data pData = variousThings.data[i];
                    BagCard bagCard = templatePrefab.GetComponentInChildren<BagCard>();
                    bagCard.imageSprite = pData.imageSprite;
                    bagCard.titleTranslationName = pData.translationName;
                    bagCard.StockQuantity = data[i].quantity;
                    bagCard.IDspReady = i;
                    bagCard.mode = "VariousThingsBuilder";
                    bagCard.AfterOnUp = variousThingssAfterOnUp;

                    bagCard.OnDown = generalOnDown;
                    bagCard.OnUp = variousThingssOnUp;
                    GameObject prefab = Instantiate(templatePrefab, Vector3.zero, Quaternion.identity, land);
                    tempAdd.Add(prefab);
                    itemHaveQuantity++;
                    yield return null;
                }

            }

        }
        else if (typeNow == Type.Accessories)
        {
            data = storeData.data.variousThings;
            for (int i = 0; i < data.Count; i++)
            {

                if (data[i].quantity > 0 && variousThings.data[i].type == VariousThingsProductData.Type.Accessories)
                {
                    VariousThingsProductData.Data pData = variousThings.data[i];
                    BagCard bagCard = templatePrefab.GetComponentInChildren<BagCard>();
                    bagCard.imageSprite = pData.imageSprite;
                    bagCard.titleTranslationName = pData.translationName;
                    bagCard.StockQuantity = data[i].quantity;
                    bagCard.IDspReady = i;
                    bagCard.mode = "VariousThingsBuilder";
                    bagCard.AfterOnUp = variousThingssAfterOnUp;

                    bagCard.OnDown = generalOnDown;
                    bagCard.OnUp = variousThingssOnUp;
                    GameObject prefab = Instantiate(templatePrefab, Vector3.zero, Quaternion.identity, land);
                    tempAdd.Add(prefab);
                    itemHaveQuantity++;
                    yield return null;
                }

            }


        }
        else if (typeNow == Type.Trash)
        {
            data = storeData.data.trash;
            for (int i = 0; i < data.Count; i++)
            {

                if (data[i].quantity > 0)
                {
                    TrashProductData.DataGrabAble pData = trash.dataGrabAbles[i];
                    BagCard bagCard = templatePrefab.GetComponentInChildren<BagCard>();
                    bagCard.imageSprite = pData.thumbnail;
                    bagCard.titleTranslationName = pData.translationName;
                    bagCard.StockQuantity = data[i].quantity;
                    bagCard.IDspReady = i;
                    bagCard.OnDown = generalOnDown;
                    bagCard.OnUp = trashOnUp;
                    GameObject prefab = Instantiate(templatePrefab, Vector3.zero, Quaternion.identity, land);
                    tempAdd.Add(prefab);
                    itemHaveQuantity++;
                    yield return null;
                }

            }


        }
        waitText.SetActive(false);
        EmptyChecker(itemHaveQuantity);
        cardCanvas.enabled = true;
        isGenerating = false;





    }
    private void EmptyChecker(int itemHaveQuantity)
    {
        if (itemHaveQuantity == 0)
        {
            emptyText.SetActive(true);
        }
        else
        {
            emptyText.SetActive(false);

        }
    }
}
