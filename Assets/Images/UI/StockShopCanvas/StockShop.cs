using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockShop : MonoBehaviour
{
    [SerializeField] private StoreData _storeData;
    [SerializeField] private LeanLocalizedTextMeshProUGUI stockName;
    [SerializeField] private Image stockImage;
    [SerializeField] private TextMeshProUGUI stockQuantity;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private TextMeshProUGUI buyPriceMoriumText;
    [SerializeField] private TextMeshProUGUI sellPriceMoriumText;
    [SerializeField] private TextMeshProUGUI priceTotalToBuyAskari;
    [SerializeField] private TextMeshProUGUI priceTotalToBuyMorium;
    [SerializeField] private GameObject linebreakSellPreview;
    [SerializeField] private GameObject linebreakBuyPreview;
    [SerializeField] private GameObject linebreakPrice;
    [SerializeField] private TMP_InputField customStockAmountToBuy;
    [SerializeField] private SellingPlatformProductData productData;
    [Header("Simple UI")]
    [SerializeField] private TextMeshProUGUI confirmationText;
    private int stockToBuy;
    private int askariToSpend;
    private int moriumToSpend;

    private int thisIDsp;
    public static StockShop Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Run(int IDsp)
    {
        thisIDsp = IDsp - 1;
        StartCoroutine(RunIE());
    }
    public void BuyStock()
    {
        if (!EconomyCurrency.Instance.CanAskariDecrease(askariToSpend))
        {
            Debug.Log("askari noo");
            return;
        }
        if (!EconomyCurrency.Instance.CanMoriumDecrease(moriumToSpend))
        {
            Debug.Log("askari noo");
            return;

        }
        List<int> stockDisplayData = StockInventoryData.Instance.stockDisplayData;

        stockDisplayData[thisIDsp] += stockToBuy;
        _storeData.data.stockQuantity[thisIDsp] += stockToBuy;
        stockQuantity.text = $"{StoreData.Instance.MoneyString(stockDisplayData[thisIDsp])}x";

        EconomyNotif.Instance.Append("askari", "Askari", askariToSpend, false);
        EconomyNotif.Instance.Append("morium", "Morium", moriumToSpend, false);

        EconomyCurrency.Instance.DecreaseAskari(askariToSpend);
        EconomyCurrency.Instance.DecreaseMorium(moriumToSpend);


    }
    public void CheckCustomStockAmountToBuy()
    {
        SellingPlatformProductData.Data data = productData.data[thisIDsp];

        stockToBuy = int.Parse(customStockAmountToBuy.text);
        StoreData storeData = StoreData.Instance;
        int buyProductPriceAskari = data.buyProductPrice * storeData.addLevelMultiplier[data.level];
        int buyProductPriceMorium = data.buyProductPriceMorium * storeData.addLevelMultiplier[data.level];

        askariToSpend = buyProductPriceAskari * stockToBuy;
        moriumToSpend = buyProductPriceMorium * stockToBuy;
        confirmationText.text = LeanLocalization.GetTranslationText("StockShopTitle")
        + " " + LeanLocalization.GetTranslationText(data.titleTranslationName) + " x" + stockToBuy + " <sprite name=\"askari\"> "
        + StoreData.Instance.MoneyString(askariToSpend);
        priceTotalToBuyAskari.text = $"<sprite name=\"askari\"> {StoreData.Instance.MoneyString(askariToSpend)}";
        priceTotalToBuyMorium.text = $"<sprite name=\"morium\"> {StoreData.Instance.MoneyString(moriumToSpend)}";

    }
    public void ResetStockToZero()
    {
        customStockAmountToBuy.text = $"0";

    }
    public void addStockToBuy(int stock)
    {
        if ((int.Parse(customStockAmountToBuy.text) + stock).ToString().Length > customStockAmountToBuy.characterLimit)
        {
            return;
        }
        customStockAmountToBuy.text = $"{stockToBuy + stock}";
    }
    private IEnumerator RunIE()
    {

        linebreakSellPreview.SetActive(false);
        linebreakBuyPreview.SetActive(false);
        linebreakPrice.SetActive(false);
        buyPriceMoriumText.gameObject.SetActive(false);
        priceTotalToBuyMorium.gameObject.SetActive(false);
        sellPriceMoriumText.gameObject.SetActive(false);


        SellingPlatformProductData.Data data = productData.data[thisIDsp];
        List<int> stockDisplayData = StockInventoryData.Instance.stockDisplayData;
        stockName.TranslationName = data.titleTranslationName;



        if (data.buyProductPriceMorium > 0)
        {
            linebreakBuyPreview.SetActive(true);
            linebreakPrice.SetActive(true);
            buyPriceMoriumText.gameObject.SetActive(true);
            priceTotalToBuyMorium.gameObject.SetActive(true);

        }
        if (data.sellProductPriceMorium > 0)
        {
            linebreakSellPreview.SetActive(true);

            sellPriceMoriumText.gameObject.SetActive(true);
        }


        StoreData storeData = StoreData.Instance;
        int buyProductPriceAskari = data.buyProductPrice * storeData.addLevelMultiplier[data.level];
        int buyProductPriceMorium = data.buyProductPriceMorium * storeData.addLevelMultiplier[data.level];
        int sellProductPriceAskari = data.sellProductPrice * storeData.addLevelMultiplier[data.level];
        int sellProductPriceMorium = data.sellProductPriceMorium * storeData.addLevelMultiplier[data.level];
        yield return null;
        stockImage.sprite = data.imageSprite;
        yield return null;
        stockQuantity.text = $"{StoreData.Instance.MoneyString(stockDisplayData[thisIDsp])}x";
        yield return null;

        buyPriceText.text = $"<sprite name=\"askari\"> {StoreData.Instance.MoneyString(buyProductPriceAskari)}";
        yield return null;
        sellPriceText.text = $"<sprite name=\"askari\"> {StoreData.Instance.MoneyString(sellProductPriceAskari)}";
        yield return null;

        buyPriceMoriumText.text = $"<sprite name=\"morium\"> {StoreData.Instance.MoneyString(buyProductPriceMorium)}";
        yield return null;
        sellPriceMoriumText.text = $"<sprite name=\"morium\"> {StoreData.Instance.MoneyString(sellProductPriceMorium)}";

    }
}
