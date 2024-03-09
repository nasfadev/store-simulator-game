using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShelvesShopPreview : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private SellingPlatformProductData sp;
    [SerializeField] private VariousThingsProductData vt;
    [SerializeField] private AreaLocker areaLocker;
    [SerializeField] private GameObject shopCam;
    [SerializeField] private GameObject playerCam;

    [SerializeField] private Transform shelvesSpawnPos;
    [Header("Configs")]
    [SerializeField] private SellingPlatformProductData.Type type;
    // ---------------Hidden-------------
    public List<int> dataIndexs;
    private GameObject prefab;
    [HideInInspector] public int currentIndex;
    public Mode mode = Mode.SellingPlatform;
    public enum Mode
    {
        SellingPlatform,
        VariousThings
    }
    public void Configs(ShelvesShopConfigs configs)
    {
        transform.position = configs.shopLocation.position;
        transform.rotation = configs.shopLocation.rotation;
        type = configs.type;
        mode = configs.mode;
    }
    public void Enter()
    {
        dataIndexs = new List<int>();
        if (mode == Mode.SellingPlatform)
        {
            for (int i = 0; i < sp.data.Length; i++)
            {
                if (sp.data[i].type == type)
                {
                    dataIndexs.Add(i);
                }
            }
        }
        else if (mode == Mode.VariousThings)
        {
            for (int i = 0; i < vt.data.Length; i++)
            {
                dataIndexs.Add(i);
            }
        }
        currentIndex = 0;
        shopCam.SetActive(true);
        playerCam.SetActive(false);
        InstantPrefab(currentIndex);
    }
    public void Next()
    {
        Debug.Log("debug shop" + currentIndex);
        currentIndex++;
        IndexFormatter();
        InstantPrefab(currentIndex);
    }
    public void Previous()
    {
        currentIndex--;
        IndexFormatter();
        InstantPrefab(currentIndex);
    }
    public void Exit()
    {
        Destroy(prefab);
        shopCam.SetActive(false);
        playerCam.SetActive(true);

    }
    public void Buy()
    {
        StoreData storeData = StoreData.Instance;
        SellingPlatformProductData.Data dataSp = null;
        VariousThingsProductData.Data dataVt = null;
        if (mode == Mode.SellingPlatform)
        {
            dataSp = sp.data[dataIndexs[currentIndex]];

        }
        else if (mode == Mode.VariousThings)
        {
            dataVt = vt.data[dataIndexs[currentIndex]];
        }
        int shelvesBuyAskariNum = 0;
        int shelvesBuyMoriumNum = 0;
        if (mode == Mode.SellingPlatform)
        {
            shelvesBuyAskariNum = dataSp.sellingPlatformPrice * storeData.addLevelMultiplier[dataSp.level];
            shelvesBuyMoriumNum = dataSp.sellingPlatformPriceMorium * storeData.addLevelMultiplier[dataSp.level];
        }
        else if (mode == Mode.VariousThings)
        {
            shelvesBuyAskariNum = dataVt.askariPrice;
        }

        if (!EconomyCurrency.Instance.CanAskariDecrease(shelvesBuyAskariNum))
        {
            Debug.Log("askari noo");
            return;
        }
        if (!EconomyCurrency.Instance.CanMoriumDecrease(shelvesBuyMoriumNum))
        {
            Debug.Log("askari noo");
            return;
        }
        if (mode == Mode.SellingPlatform)
        {
            if (!storeData.data.shelvesDatas[dataIndexs[currentIndex]].isBought)
            {
                storeData.data.shelvesBoughtCount++;
                storeData.data.shelvesDatas[dataIndexs[currentIndex]].isBought = true;
                areaLocker.Render();

            }
            StoreData.Instance.data.sellingPlatform[dataIndexs[currentIndex]].quantity++;
            EconomyNotif.Instance.Append(dataSp.prefabImageSprite, Lean.Localization.LeanLocalization.GetTranslationText(dataSp.titleTranslationName), 1, true);
        }
        else if (mode == Mode.VariousThings)
        {
            StoreData.Instance.data.variousThings[dataIndexs[currentIndex]].quantity++;

            EconomyNotif.Instance.Append(dataVt.imageSprite, Lean.Localization.LeanLocalization.GetTranslationText(dataVt.translationName), 1, true);

        }

        EconomyNotif.Instance.Append("askari", "Askari", shelvesBuyAskariNum, false);
        EconomyNotif.Instance.Append("morium", "Morium", shelvesBuyMoriumNum, false);

        EconomyCurrency.Instance.DecreaseAskari(shelvesBuyAskariNum);
        EconomyCurrency.Instance.DecreaseMorium(shelvesBuyMoriumNum);
    }
    private void IndexFormatter()
    {

        if (currentIndex >= dataIndexs.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = dataIndexs.Count - 1;
        }
    }
    private void InstantPrefab(int index)
    {
        if (prefab != null)
        {
            Destroy(prefab);
        }
        if (mode == Mode.SellingPlatform)
        {
            prefab = Instantiate
            (
                sp.data[dataIndexs[index]].prefab,
                shelvesSpawnPos.position,
                shelvesSpawnPos.rotation,
                shelvesSpawnPos
            );
            prefab.GetComponent<SellingPlatform>().stockManager.FillStockPreview();
        }
        else if (mode == Mode.VariousThings)
        {
            prefab = Instantiate
            (
                vt.data[dataIndexs[index]].prefab,
                shelvesSpawnPos.position,
                Quaternion.Euler(0, 90, 0),
                shelvesSpawnPos
            );


        }
    }
}
