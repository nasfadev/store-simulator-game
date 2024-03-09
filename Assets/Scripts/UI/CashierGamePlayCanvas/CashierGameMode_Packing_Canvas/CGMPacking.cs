using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGMPacking : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private RectTransform CGMPackingCardLand;
    [SerializeField] private GameObject CGMPackingCardTemplate;
    [SerializeField] private SellingPlatformProductData spProductData;
    [SerializeField] private QuestGenerator questGenerator;
    [Header("Configs")]
    [SerializeField] private int maxCardSlot;
    public RectTransform[] CardSlotRect;
    [Header("Preview")]

    public bool[] cardSlotClaim;
    public int cardSlotQueueNumber;
    public bool isPacking;
    private Coroutine runCoroutine;
    public List<BuyerAI.SPAIData> spAIDatas;
    public static CGMPacking Instance;
    public event Action WhenReset;
    public CashierManager cashierManager;
    private void Awake()
    {
        Instance = this;
        Reset();
        BuyerSpawner.Instance.whenDisable += Reset;
    }
    public void Run()
    {
        runCoroutine = StartCoroutine(RunIE());
    }
    public void Reset()
    {
        WhenReset?.Invoke();
        cardSlotClaim = new bool[maxCardSlot];
        cardSlotQueueNumber = 0;
        isPacking = false;
    }
    private IEnumerator RunIE()
    {



        while (true)
        {
            if (spAIDatas.Count == 0)
            {
                isPacking = false;
                break;
            }
            if (cardSlotQueueNumber < 3 && cardSlotQueueNumber < spAIDatas.Count)
            {
                CheckStockAndInstantiate();

            }
            yield return null;

        }
    }
    private void CheckStockAndInstantiate()
    {
        if (spAIDatas[cardSlotQueueNumber].stockQuantityToBuy <= 0)
        {
            spAIDatas.RemoveAt(cardSlotQueueNumber);
        }
        else
        {
            BuyerAI.SPAIData sPAIData = spAIDatas[cardSlotQueueNumber];
            CGMPackingCard card = CGMPackingCardTemplate.GetComponent<CGMPackingCard>();
            card.thumbnailCard.sprite = spProductData.data[sPAIData.IDsp - 1].imageSprite;
            card.queueNumber = cardSlotQueueNumber;
            card.stockAmount = sPAIData.stockQuantityToBuy;
            card.IDsp = sPAIData.IDsp;
            card.cashierManager = cashierManager;
            card.questGenerator = questGenerator;

            RectTransform templateRect = Instantiate(CGMPackingCardTemplate, CGMPackingCardLand, false).GetComponent<RectTransform>();
            templateRect.anchoredPosition = CardSlotRect[cardSlotQueueNumber].anchoredPosition;

            cardSlotClaim[cardSlotQueueNumber] = true;
            cardSlotQueueNumber++;
        }
    }



}
