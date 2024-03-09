using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SellingPlatformRealtimeData
{
    public List<BuyerSlotTranfrom> buyerSlotTranfrom;
    public bool[] buyerSlotClaim;
    public int index;
    public int IDsp;
    public int maxStockQuantity;
    public SellingPlatformStockManager stockManager;

}