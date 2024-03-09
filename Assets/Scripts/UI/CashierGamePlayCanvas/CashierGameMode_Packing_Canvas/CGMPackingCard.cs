using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class CGMPackingCard : MonoBehaviour
{
    [SerializeField] private Image loadingFill;
    [SerializeField] private TextMeshProUGUI stockAmountText;
    [SerializeField] private RectTransform rect;
    [SerializeField] private float fillLoadingFillDifficulty;
    [SerializeField] private float realeaseLoadingFillDifficulty;

    [SerializeField] private float moveTweenTime;
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float defaultScale;
    [SerializeField] private float turnScale;
    public QuestGenerator questGenerator;
    public Image thumbnailCard;
    public int IDsp;
    public int queueNumber;
    public List<BuyerAI.SPAIData> sPAIDatas;
    public int stockAmount;
    public CashierManager cashierManager;
    private bool onTouching;
    private void Start()
    {
        UpdateStockAmountText(stockAmount);
        rect.localScale = Vector3.zero;
        StartCoroutine(Run());
        BuyerSpawner.Instance.whenDisable += DestroyThis;
        CGMPacking.Instance.WhenReset += DestroyThis;
    }
    private void OnDisable()
    {
        BuyerSpawner.Instance.whenDisable -= DestroyThis;
        CGMPacking.Instance.WhenReset -= DestroyThis;

    }

    private void DestroyThis()
    {


        Destroy(gameObject);
    }
    private void UpdateStockAmountText(int amount)
    {

        stockAmountText.text = $"{amount}x";

    }
    private IEnumerator Run()
    {
        CGMPacking data = CGMPacking.Instance;

        rect.DOScale(Vector3.one * defaultScale, scaleTweenTime).SetEase(Ease.OutBack);

        while (true)
        {
            if (queueNumber == 0)
            {





                rect.DOScale(Vector3.one * turnScale, scaleTweenTime).SetEase(Ease.OutBack);
                loadingFill.gameObject.SetActive(true);



                yield return WhenOnPackingRound(data);



            }
            else if (!data.cardSlotClaim[queueNumber - 1])
            {
                data.cardSlotClaim[queueNumber] = false;


                data.cardSlotClaim[queueNumber - 1] = true;
                queueNumber -= 1;

                DoMoveTween(data.CardSlotRect[queueNumber].anchoredPosition);
            }
            yield return null;

        }

    }
    private IEnumerator WhenOnPackingRound(CGMPacking data)
    {
        CGMPackingController controller = CGMPackingController.Instance;
        while (true)
        {
            CGMPacking cashier = CGMPacking.Instance;

            if (controller.isTrue)
            {
                controller.isTrue = false;

                cashier.spAIDatas[0].stockQuantityToBuy--;

                StoreData.StoreDataSave.instance.data.stockQuantity[IDsp - 1]--;
                UpdateStockAmountText(cashier.spAIDatas[0].stockQuantityToBuy);

                StoreData storeData = StoreData.Instance;
                SellingPlatformProductData.Data dataSp = SellingPlatformProductData.Instance.data[IDsp - 1];
                int sellProductPrice = dataSp.sellProductPrice * storeData.addLevelMultiplier[dataSp.level];


                VariousThingsBuilder.VariousThingsDataSave.instance.cashierAskari[cashierManager.variousThings.index] += sellProductPrice;
                cashierManager.EnableAskariCoins();
                questGenerator.AddQuestCount(cashier.spAIDatas[0].IDsp - 1, StoreData.QuestType.SellStock);


                if (cashier.spAIDatas[0].stockQuantityToBuy == 0)
                {
                    bool isFinish = false;
                    rect.DOScale(Vector3.zero, scaleTweenTime).SetEase(Ease.InBack)
                    .OnComplete(() => { isFinish = true; });
                    while (!isFinish)
                    {
                        yield return null;
                    }
                    cashier.spAIDatas.RemoveAt(0);
                    data.cardSlotClaim[queueNumber] = false;
                    cashier.cardSlotQueueNumber--;
                    Destroy(gameObject);
                    break;

                }


            }
            yield return null;
        }
    }
    private void DoMoveTween(Vector2 pos)
    {
        rect.DOAnchorPos(pos, moveTweenTime).SetEase(Ease.InOutQuad);
    }
}
