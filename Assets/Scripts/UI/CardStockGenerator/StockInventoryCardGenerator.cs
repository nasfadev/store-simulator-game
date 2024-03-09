using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StockInventoryCardGenerator : MonoBehaviour
{
    [SerializeField] private StoreData _storeData;
    [SerializeField] private RectTransform cardStockLand;
    [SerializeField] private GameObject CardTemplatePrefab;
    [SerializeField] private SellingPlatformProductData productData;
    [SerializeField] private UnityEvent WhenCardOnDown;
    [SerializeField] private UnityEvent WhenCardOnUp;
    private List<GameObject> cardStockSave;


    private void Start()
    {
        Generate();
    }
    public void Generate()
    {
        StartCoroutine(GenerateIE(_storeData.data.stockQuantity));
    }
    public void GenerateDisplay()
    {
        StartCoroutine(GenerateIE(StockInventoryData.Instance.stockDisplayData));

    }
    private IEnumerator GenerateIE(List<int> stockQuantity)
    {
        while (!StateLoaded.isLoaded)
        {
            yield return null;
        }
        if (cardStockSave != null)
        {
            for (int i = 0; i < cardStockSave.Count; i++)
            {
                Destroy(cardStockSave[i]);
                Debug.Log("del");
                yield return null;

            }
        }
        Debug.Log("del");

        cardStockSave = new List<GameObject>();

        for (int i = 0; i < productData.data.Length; i++)
        {
            SellingPlatformProductData.Data cardStockData = productData.data[i];

            if (cardStockData.level <= StoreData.StoreDataSave.instance.data.shelvesBoughtCount)
            {
                CardStock cardStock = CardTemplatePrefab.GetComponentInChildren<CardStock>();
                cardStock.imageSprite = cardStockData.imageSprite;
                cardStock.titleTranslationName = cardStockData.titleTranslationName;
                cardStock.StockQuantity = stockQuantity[i];
                cardStock.IDspReady = i;
                cardStock.OnDown = WhenCardOnDown;
                cardStock.OnUp = WhenCardOnUp;


                GameObject prefab = Instantiate(CardTemplatePrefab, Vector3.zero, Quaternion.identity, cardStockLand);

                cardStockSave.Add(prefab);

                yield return null;
            }
            else
            {
                break;
            }

        }
    }
}
