using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class CashierRealtimeData
{
    public int index;
    public int id;
    public int queueNumber = 0;
    public List<BuyerSlotTranfrom> buyerSlotTranfrom;
    public bool[] buyerSlotClaim;
    public CashierManager cashierManager;
}
[System.Serializable]

public class BuyerSlotTranfrom
{
    public Vector3 position;
    public float rotationY;
    public BuyerSlotTranfrom(Transform tfm)
    {
        position = tfm.position;
        rotationY = tfm.eulerAngles.y;
    }
}
