using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeDataBuyerSystem : MonoBehaviour
{
    public static RealtimeDataBuyerSystem Instance;
    public List<SellingPlatformRealtimeData> SellingPlatformsRD;
    public List<int> SellingPlatformsRDID;
    public List<CashierRealtimeData> cashierRD;
    public List<Transform> slot;
    private GameObject cashierPrefab;

    private void Start()
    {
        Instance = this;
        SellingPlatformBuilder.Instance.Run += ResetSellingPlatformData;
        StartCoroutine(Init());
        // BuyerSpawner.Instance.whenDisable += ResetCashier;
    }

    private void OnDisable()
    {
        // BuyerSpawner.Instance.whenDisable -= ResetCashier;
        SellingPlatformBuilder.Instance.Run -= ResetSellingPlatformData;
    }
    private IEnumerator Init()
    {
        while (StateLoaded.Loaded != 5)
        {
            yield return null;
        }
        FillSellingPlatformsRDID();

    }
    public void ResetSellingPlatformData()
    {
        SellingPlatformsRD = new List<SellingPlatformRealtimeData>();
    }
    // public void ResetCashier()
    // {
    //     CashierInit(cashierPrefab);
    // }
    // public void CashierInit(GameObject cashier)
    // {
    //     cashierPrefab = cashier;
    //     cashierRD = new CashierRealtimeData();
    //     cashierRD.buyerSlotTranfrom = new List<BuyerSlotTranfrom>();
    //     int childCount = cashier.transform.GetChild(0).childCount;
    //     for (int i = 0; i < childCount; i++)
    //     {
    //         Transform childT = cashier.transform.GetChild(0).GetChild(i).transform;
    //         BuyerSlotTranfrom buyerSlotT = new BuyerSlotTranfrom(childT);
    //         cashierRD.buyerSlotTranfrom.Add(buyerSlotT);
    //     }
    //     cashierRD.buyerSlotClaim = new bool[childCount];
    // }
    public void ReRandomSellingPlatformsRDID()
    {

        int n = SellingPlatformsRDID.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = SellingPlatformsRDID[i];
            SellingPlatformsRDID[i] = SellingPlatformsRDID[j];
            SellingPlatformsRDID[j] = temp;
        }

    }
    public void FillSellingPlatformsRDID()
    {
        SellingPlatformsRDID = new List<int>();

        int n = SellingPlatformsRD.Count;
        for (int i = 0; i < n; i++)
        {
            SellingPlatformsRDID.Add(i);
        }
        ReRandomSellingPlatformsRDID();
    }

}