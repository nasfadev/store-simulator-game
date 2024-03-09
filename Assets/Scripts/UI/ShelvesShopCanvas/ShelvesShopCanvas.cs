using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;

public class ShelvesShopCanvas : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private ShelvesShopPreview preview;
    [SerializeField] private SellingPlatformProductData sp;
    [SerializeField] private VariousThingsProductData vt;
    [SerializeField] private GameObject linebreakSellPreview;
    [SerializeField] private GameObject linebreakBuyPreview;
    [SerializeField] private GameObject linebreakPrice;
    [SerializeField] private GameObject locker;
    [SerializeField] private GameObject explainBox;


    [Header("Data Changes")]
    [SerializeField] private TextMeshProUGUI productSellAskari;
    [SerializeField] private TextMeshProUGUI productSellMorium;
    [SerializeField] private TextMeshProUGUI productBuyAskari;
    [SerializeField] private TextMeshProUGUI productBuyMorium;
    [SerializeField] private TextMeshProUGUI shelvesBuyAskari;
    [SerializeField] private TextMeshProUGUI shelvesBuyMorium;
    [SerializeField] private LeanLocalizedTextMeshProUGUI tName;
    [SerializeField] private TextMeshProUGUI lockerText;


    public void UpdateTexts()
    {

        linebreakSellPreview.SetActive(false);
        linebreakBuyPreview.SetActive(false);
        linebreakPrice.SetActive(false);
        productSellMorium.gameObject.SetActive(false);
        productBuyMorium.gameObject.SetActive(false);
        shelvesBuyMorium.gameObject.SetActive(false);
        locker.SetActive(false);

        StoreData storeData = StoreData.Instance;

        int productSellAskariNum = 0;
        int productSellMoriumNum = 0;
        int productBuyAskariNum = 0;
        int productBuyMoriumNum = 0;
        int shelvesBuyAskariNum = 0;
        int shelvesBuyMoriumNum = 0;
        if (preview.mode == ShelvesShopPreview.Mode.SellingPlatform)
        {
            SellingPlatformProductData.Data dataSp = sp.data[preview.dataIndexs[preview.currentIndex]];

            explainBox.SetActive(true);
            bool isLocked = preview.dataIndexs[preview.currentIndex] - 1 < 0 ? false : !storeData.data.shelvesDatas[preview.dataIndexs[preview.currentIndex] - 1].isBought;
            if (isLocked)
            {
                SellingPlatformProductData.Data dataLock = sp.data[preview.dataIndexs[preview.currentIndex] - 1];

                string lockerString = LeanLocalization.GetTranslationText("ShelvesShop_Locker");
                string nameString = LeanLocalization.GetTranslationText(dataLock.titleTranslationName);
                lockerText.text = lockerString + " " + nameString + " <sprite name=\"lock\">";
                locker.SetActive(true);

            }

            productSellAskariNum = dataSp.sellProductPrice * storeData.addLevelMultiplier[dataSp.level];
            productSellMoriumNum = dataSp.sellProductPriceMorium * storeData.addLevelMultiplier[dataSp.level];
            productBuyAskariNum = dataSp.buyProductPrice * storeData.addLevelMultiplier[dataSp.level];
            productBuyMoriumNum = dataSp.buyProductPriceMorium * storeData.addLevelMultiplier[dataSp.level];
            shelvesBuyAskariNum = dataSp.sellingPlatformPrice * storeData.addLevelMultiplier[dataSp.level];
            shelvesBuyMoriumNum = dataSp.sellingPlatformPriceMorium * storeData.addLevelMultiplier[dataSp.level];
            tName.TranslationName = dataSp.titleTranslationName;

        }
        else if (preview.mode == ShelvesShopPreview.Mode.VariousThings)
        {
            VariousThingsProductData.Data dataVt = vt.data[preview.dataIndexs[preview.currentIndex]];

            explainBox.SetActive(false);

            shelvesBuyAskariNum = dataVt.askariPrice;
            tName.TranslationName = dataVt.translationName;


        }

        productSellAskari.text = $"<sprite name=\"askari\"> {storeData.MoneyString(productSellAskariNum)}";
        if (productSellMoriumNum > 0)
        {
            linebreakSellPreview.SetActive(true);
            productSellMorium.text = $"<sprite name=\"morium\"> {storeData.MoneyString(productSellMoriumNum)}";
        }
        productBuyAskari.text = $"<sprite name=\"askari\"> {storeData.MoneyString(productBuyAskariNum)}";
        if (productBuyMoriumNum > 0)
        {
            linebreakBuyPreview.SetActive(true);
            productBuyMorium.text = $"<sprite name=\"morium\"> {storeData.MoneyString(productBuyMoriumNum)}";

        }
        shelvesBuyAskari.text = $"<sprite name=\"askari\"> {storeData.MoneyString(shelvesBuyAskariNum)}";
        if (shelvesBuyMoriumNum > 0)
        {
            linebreakPrice.SetActive(true);
            shelvesBuyMorium.text = $"<sprite name=\"morium\"> {storeData.MoneyString(shelvesBuyMoriumNum)}";
        }


    }








}
