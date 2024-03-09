using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
public class AreaLocker : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [SerializeField] private SellingPlatformProductData sp;
    [Header("Configs")]
    [SerializeField] private AreaLockerData[] areaLockerDatas;
    [Header("Previews")]
    public int BookAreaLockerId;
    [System.Serializable]
    public class AreaLockerData
    {
        public GameObject locker;
        public int shelvesIdRequired;
        public int askariCost;
    }
    private void Start()
    {
        List<bool> areaLockerStoreData = storeData.data.areaLockerData;
        if (areaLockerStoreData.Count < areaLockerDatas.Length)
        {
            for (int i = areaLockerStoreData.Count; i < areaLockerDatas.Length; i++)
            {
                areaLockerStoreData.Add(false);
            }
        }
        Render();
    }
    public void Render()
    {
        for (int i = 0; i < areaLockerDatas.Length; i++)
        {

            if (storeData.data.areaLockerData[i])
            {
                areaLockerDatas[i].locker.SetActive(false);
            }
            else
            {
                Locker locker = areaLockerDatas[i].locker.GetComponent<Locker>();
                AreaLockerData areaLockerData = areaLockerDatas[i];
                locker.id = i;
                int idShelves = areaLockerDatas[i].shelvesIdRequired;
                SellingPlatformProductData.Data shelvesData = sp.data[idShelves];
                int askariPrice = shelvesData.sellingPlatformPrice * storeData.addLevelMultiplier[shelvesData.level];
                locker.priceText.text = "<sprite name=\"askari\"> " + storeData.MoneyString(askariPrice);
                locker.lockRequireBox.SetActive(false);
                areaLockerData.locker.layer = 7;
                locker.GetComponent<RayPointerAble>().mode = RayPointerAble.Type.AreaLocker;

                if (!storeData.data.shelvesDatas[areaLockerData.shelvesIdRequired].isBought)
                {
                    locker.GetComponent<RayPointerAble>().mode = RayPointerAble.Type.Ignore;

                    SellingPlatformProductData.Data dataLock = sp.data[areaLockerData.shelvesIdRequired];

                    string lockerString = LeanLocalization.GetTranslationText("ShelvesShop_Locker");
                    string nameString = LeanLocalization.GetTranslationText(dataLock.titleTranslationName);
                    locker.lockRequireBox.SetActive(true);
                    locker.lockRequireText.text = lockerString + " " + nameString + " <sprite name=\"lock\">";
                }
            }
        }
    }
    public void UnlockArea()
    {
        int idShelves = areaLockerDatas[BookAreaLockerId].shelvesIdRequired;
        SellingPlatformProductData.Data shelvesData = sp.data[idShelves];
        int askariPrice = shelvesData.sellingPlatformPrice * storeData.addLevelMultiplier[shelvesData.level];
        EconomyCurrency economyCurrency = EconomyCurrency.Instance;
        if (!economyCurrency.CanAskariDecrease(askariPrice))
        {
            return;
        }
        storeData.data.areaLockerData[BookAreaLockerId] = true;
        economyCurrency.DecreaseAskari(askariPrice);
        EconomyNotif economyNotif = EconomyNotif.Instance;
        economyNotif.Append("askari", "Askari", askariPrice, false);
        Render();
    }
}
