using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SellingPlatformStockManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stockGOs;
    [SerializeField] private SellingPlatform sellingPlatform;
    private bool isShuffleying;
    private bool isReady;
    private int currentStockCount;
    private List<int> decreseStockQueues;

    private void Awake()
    {
        StaticBatchingUtility.Combine(gameObject);
        DisableAllStocks();
        SellingPlatformBuilder.Instance.OnRendering += Deactive;
        SellingPlatformBuilder.Instance.OnRendered += Active;

    }

    private void Deactive()
    {
        gameObject.SetActive(false);
    }
    private void Active()
    {
        gameObject.SetActive(true);
        int stockQuantity = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[sellingPlatform.index].stockQuantity;

        Debug.Log($"stoc q{stockQuantity}");
        InitStocks(stockQuantity);

    }
    public void InitStocks(int stock)
    {
        if (!isReady)
        {
            ShuffleStockGOs();
            isReady = true;
            FillAllStocks(stock);
        }
    }
    private void OnDestroy()
    {
        SellingPlatformBuilder.Instance.OnRendering -= Deactive;
        SellingPlatformBuilder.Instance.OnRendered -= Active;
    }
    public int FillStock()
    {
        StartCoroutine(FillStockIE());
        int stockInventory = StockInventoryData.Instance.stockDisplayData[sellingPlatform.IDsp - 1];
        return stockInventory;

    }
    public void FillStockPreview()
    {
        StartCoroutine(FillStockPreviewIE());


    }
    private IEnumerator FillStockIE()
    {
        int stockInventory = StockInventoryData.Instance.stockDisplayData[sellingPlatform.IDsp - 1];
        int stockQuantity = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[sellingPlatform.index].stockQuantity;
        int maxStockQuantity = sellingPlatform.maxStockQuantity;
        int stockAbleToFill = maxStockQuantity - stockQuantity;
        int stockReadyToFill = Mathf.Clamp(stockAbleToFill, 0, stockInventory);
        int stockAdded = 0;
        StockInventoryData.Instance.DecreaseStock(sellingPlatform.IDsp, stockReadyToFill);
        SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[sellingPlatform.index].stockQuantity += stockReadyToFill;

        if (stockReadyToFill > 0)
        {
            for (int i = 0; i < sellingPlatform.maxStockQuantity; i++)
            {
                if (!stockGOs[i].activeSelf)
                {
                    stockGOs[i].SetActive(true);
                    stockAdded++;
                    if (stockAdded == stockReadyToFill)
                    {
                        break;
                    }

                }
                yield return null;
            }
        }

    }
    private IEnumerator FillStockPreviewIE()
    {
        for (int i = 0; i < sellingPlatform.maxStockQuantity; i++)
        {

            stockGOs[i].SetActive(true);

            yield return null;
        }


    }
    public void FillAllStocks(int stock)
    {
        StartCoroutine(FillAllStocksIE(stock));
    }
    private IEnumerator FillAllStocksIE(int stock)
    {
        yield return null;

        while (true)
        {
            if (isShuffleying && isReady)
            {
                break;
            }
            yield return null;
        }
        for (int i = 0; i < stock; i++)
        {
            stockGOs[i].SetActive(true);


            yield return null;
        }


    }
    private void DisableAllStocks()
    {

        for (int i = 0; i < stockGOs.Length; i++)
        {
            stockGOs[i].SetActive(false);

        }


    }
    public void DecreaseStock(int stock)
    {
        StartCoroutine(DecreaseStockIE(stock));
    }
    private IEnumerator DecreaseStockIE(int stock)
    {
        SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[sellingPlatform.index].stockQuantity -= stock;
        int stockDecrease = 0;
        for (int i = 0; i < sellingPlatform.maxStockQuantity; i++)
        {
            if (stockGOs[i].activeSelf)
            {
                stockGOs[i].SetActive(false);
                stockDecrease++;
                if (stockDecrease == stock)
                {
                    break;
                }

            }
            yield return null;
        }

    }
    public void ShuffleStockGOs()
    {
        isShuffleying = false;
        StartCoroutine(ShuffleStockGOsIE());
        Debug.Log($"shufflecuy");

    }
    private IEnumerator ShuffleStockGOsIE()
    {

        int n = stockGOs.Length;
        yield return null;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = stockGOs[i];
            stockGOs[i] = stockGOs[j];
            stockGOs[j] = temp;
        }
        isShuffleying = true;


    }
}
