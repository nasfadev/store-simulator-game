using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierCollector : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        VariousThingsBuilder.Instance.WhenStop += Collect;
        BuyerSpawner.Instance.whenDisable += Collect;
    }
    private void OnDisable()
    {
        VariousThingsBuilder.Instance.WhenStop -= Collect;
        BuyerSpawner.Instance.whenDisable -= Collect;
    }
    private void Collect()
    {
        RealtimeDataBuyerSystem.Instance.cashierRD = new List<CashierRealtimeData>();

        for (int i = 0; i < transform.childCount; i++)
        {

            RealtimeDataBuyerSystem.Instance.cashierRD.Add(GetData(transform.GetChild(i)));
        }

    }
    private CashierRealtimeData GetData(Transform data)
    {
        CashierRealtimeData cashierRD = new CashierRealtimeData();
        cashierRD.buyerSlotTranfrom = new List<BuyerSlotTranfrom>();
        int childCount = data.GetChild(0).childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childT = data.GetChild(0).GetChild(i).transform;
            BuyerSlotTranfrom buyerSlotT = new BuyerSlotTranfrom(childT);
            cashierRD.buyerSlotTranfrom.Add(buyerSlotT);
        }
        cashierRD.buyerSlotClaim = new bool[childCount];
        cashierRD.cashierManager = data.GetComponent<CashierManager>();
        return cashierRD;
    }

}
