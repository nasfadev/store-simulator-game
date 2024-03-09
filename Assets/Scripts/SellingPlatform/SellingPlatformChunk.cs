using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingPlatformChunk : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SellingPlatformBuilder.Instance.Reset += SellingPlatformCollector;
    }
    private void OnDisable()
    {
        SellingPlatformBuilder.Instance.Reset -= SellingPlatformCollector;

    }

    private void SellingPlatformCollector()
    {
        RealtimeDataBuyerSystem.Instance.SellingPlatformsRD = new List<SellingPlatformRealtimeData>();

        for (int i = 0; i < transform.childCount; i++)
        {

            SellingPlatformRealtimeData SPRD = new SellingPlatformRealtimeData();
            SPRD.buyerSlotTranfrom = new List<BuyerSlotTranfrom>();
            SPRD = GetBuyerSlotTranform(i, SPRD);
            SPRD.buyerSlotClaim = new bool[SPRD.buyerSlotTranfrom.Count];

            SellingPlatform sellingPlatform = transform.GetChild(i).GetComponent<SellingPlatform>();
            SPRD.stockManager = sellingPlatform.stockManager;

            SPRD.index = sellingPlatform.index;
            SPRD.IDsp = sellingPlatform.IDsp;
            SPRD.maxStockQuantity = sellingPlatform.maxStockQuantity;

            RealtimeDataBuyerSystem.Instance.SellingPlatformsRD.Add(SPRD);


        }
        RealtimeDataBuyerSystem.Instance.FillSellingPlatformsRDID();

    }
    private SellingPlatformRealtimeData GetBuyerSlotTranform(int num, SellingPlatformRealtimeData SPRD)
    {
        for (int i = 0; i < transform.GetChild(num).GetChild(0).childCount; i++)
        {
            Transform childT = transform.GetChild(num).GetChild(0).GetChild(i).transform;
            Debug.Log($"rot Child {childT.eulerAngles.y}");
            BuyerSlotTranfrom buyerSlotT = new BuyerSlotTranfrom(childT);
            SPRD.buyerSlotTranfrom.Add(buyerSlotT);
        }
        return SPRD;
    }
}
