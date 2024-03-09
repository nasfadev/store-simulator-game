using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class StockInventoryData : MonoBehaviour
{
    [SerializeField] private StoreData _storeData;
    public List<int> stockDisplayData;
    public static StockInventoryData Instance;

    private void Start()
    {
        int StockEntityCount = SellingPlatformProductData.Instance.data.Length;
        Instance = this;

        if (_storeData.data.stockQuantity.Count < StockEntityCount)
        {
            int currentStockCount = _storeData.data.stockQuantity.Count;

            for (int i = 0; i < StockEntityCount - currentStockCount; i++)
            {
                _storeData.data.stockQuantity.Add(0);
            }
        }
        stockDisplayData = _storeData.data.stockQuantity.ToList();






    }

    public void ResetDisplayData()
    {
        stockDisplayData = _storeData.data.stockQuantity.ToList();

    }
    public void DecreaseStock(int IDsp, int stock)
    {
        Debug.Log($" stock di kuringi {stock}");
        stockDisplayData[IDsp - 1] -= stock;
        _storeData.data.stockQuantity[IDsp - 1] -= stock;

    }
}
