using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class LandsSeller : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [Header("Requires - UI")]
    [SerializeField] private TextMeshProUGUI priceText;
    [Header("Configs")]
    [SerializeField] private int defaultLandPrice;
    [SerializeField] private int dimension;
    [SerializeField] private GameObject[] lands;
    [SerializeField] private GameObject[] landsBuilderGuide;
    [SerializeField] private GameObject[] landsGround;
    [SerializeField] private GameObject[] landsGroundRoof;
    [Header("Previews")]
    public int bookLandId;

    public void BuyLand()
    {
        StoreData.LandsSellerData landsSellerData = storeData.data.landsSellerData;

        int landPrice = defaultLandPrice * landsSellerData.landsBoughtCount;
        EconomyCurrency economyCurrency = EconomyCurrency.Instance;
        EconomyNotif economyNotif = EconomyNotif.Instance;
        if (!economyCurrency.CanAskariDecrease(landPrice))
        {
            return;
        }
        economyCurrency.DecreaseAskari(landPrice);
        economyNotif.Append("askari", "Askari", landPrice, false);
        landsSellerData.isLandsBought[bookLandId] = true;
        landsSellerData.landsBoughtCount++;
        Render();

    }
    public void UpdateUI()
    {
        StoreData.LandsSellerData landsSellerData = storeData.data.landsSellerData;
        int landPrice = defaultLandPrice * landsSellerData.landsBoughtCount;
        priceText.text = "<sprite name=\"askari\"> " + storeData.MoneyString(landPrice);
    }
    private void Start()
    {
        StoreData.LandsSellerData landsSellerData = storeData.data.landsSellerData;
        if (landsSellerData.landsBoughtCount == 0)
        {
            landsSellerData.isLandsBought = new bool[dimension * dimension];
            landsSellerData.isLandsBought[0] = true;
            landsSellerData.landsBoughtCount = 1;
        }
        Render();
    }
    public void Render()
    {
        StoreData.LandsSellerData landsSellerData = storeData.data.landsSellerData;
        for (int i = 0; i < landsSellerData.isLandsBought.Length; i++)
        {
            Lands land = lands[i].GetComponent<Lands>();


            land.landSign.SetActive(false);
            landsBuilderGuide[i].SetActive(false);
            landsGround[i].SetActive(false);
            landsGroundRoof[i].SetActive(false);
            land.id = i;


        }
        for (int i = 0; i < landsSellerData.isLandsBought.Length; i++)
        {

            if (landsSellerData.isLandsBought[i])
            {
                Lands land = lands[i].GetComponent<Lands>();
                land.gameObject.SetActive(false);
                landsBuilderGuide[i].SetActive(true);
                landsGround[i].SetActive(true);
                landsGroundRoof[i].SetActive(true);



                //lands data by block
                bool isLand1 = false, isLand2 = false, isLand3 = false, isLand4 = false;
                // ------------------------
                //          land3
                //   land2           land1
                //          land4

                //land4 checker
                if (i - dimension >= 0)
                {
                    isLand4 = true;
                }
                //land3 checker

                if (i + dimension < dimension * dimension)
                {
                    isLand3 = true;
                }
                //land2 checker

                if (i % dimension != 0)
                {
                    isLand2 = true;
                }
                //land1 checker

                if (i % dimension != dimension - 1)
                {
                    isLand1 = true;
                }

                //landApply
                if (isLand1)
                {
                    Lands land1 = lands[i + 1].GetComponent<Lands>();

                    land1.landSign.SetActive(true);
                }
                if (isLand2)
                {
                    Lands land2 = lands[i - 1].GetComponent<Lands>();

                    land2.landSign.SetActive(true);
                }
                if (isLand3)
                {
                    Lands land3 = lands[i + dimension].GetComponent<Lands>();

                    land3.landSign.SetActive(true);

                }
                if (isLand4)
                {
                    Lands land4 = lands[i - dimension].GetComponent<Lands>();

                    land4.landSign.SetActive(true);
                }

            }
        }
    }
}
