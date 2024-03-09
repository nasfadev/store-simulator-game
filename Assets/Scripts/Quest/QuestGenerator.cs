using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class QuestGenerator : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private SellingPlatformProductData sp;
    [SerializeField] private StoreData storeData;
    [SerializeField] private NoticableIcon noticableIcon;
    [Header("Global Configs")]
    [SerializeField] private UnityEvent whenQuestNoReady;
    [SerializeField] private UnityEvent whenQuestReady;
    [SerializeField] private UnityEvent whenQuestAccepted;
    [SerializeField] private UnityEvent whenAfterAddQuestCount;
    [SerializeField] private UnityEvent whenQuestComplete;

    [Header("Sell Stock Configs")]
    [SerializeField] private int SellStockMinCount;
    [SerializeField] private int SellStockMaxCount;
    [SerializeField] private int SellStockDefaultAskari;
    [SerializeField] private int MultipleRewardByUpgradeLevel;
    [SerializeField] private GameObject SellStocktranslation;
    [SerializeField] private UnityEvent sellStockWhenQuestAccept;
    //hide
    [Header("Preview")]
    public string questTranslation;
    public string objectiveTranslation;
    public UnityEvent acceptedQuestEvent;
    private void Awake()
    {
        int seed = (int)System.DateTime.Now.Ticks;
        Random.InitState(seed);
    }
    private void Start()
    {


        StoreData.QuestData questData = storeData.data.questData;
        if (questData.nowCount >= questData.maxCount == questData.isQuestAccept)
        {
            GetReward(1);
        }

        if (!questData.isQuestReady && !questData.isQuestAccept)
        {
            GetQuest();
            whenQuestNoReady?.Invoke();
        }
        else if (questData.type == StoreData.QuestType.SellStock && !questData.isQuestAccept)
        {
            UpdateTranslation();
            UpdateAcceptedQuestEvents();
            noticableIcon.target = GetSellingPlatformNPCTarget();
            whenQuestReady?.Invoke();

        }
        else
        {
            UpdateTranslation();
            UpdateAcceptedQuestEvents();
            whenQuestAccepted?.Invoke();
        }





    }
    private void UpdateTranslation()
    {
        StoreData.QuestData questData = storeData.data.questData;

        if (questData.type == StoreData.QuestType.SellStock)
        {
            questTranslation = SellStocktranslation.name;
            objectiveTranslation = sp.data[questData.idType].titleTranslationName;
        }

    }
    public void GetQuest()
    {

        int questType = Random.Range(0, System.Enum.GetValues(typeof(StoreData.QuestType)).Length);
        if ((StoreData.QuestType)questType == StoreData.QuestType.SellStock)
        {
            GetSellStockQuest();
        }
    }
    private void GetSellStockQuest()
    {
        StoreData.QuestData questData = storeData.data.questData;
        questData.isQuestReady = true;
        questData.isQuestAccept = false;
        questData.type = StoreData.QuestType.SellStock;
        questData.acceptQuestCount++;
        questData.nowCount = 0;
        questData.maxCount = GetMaxCountQuest(SellStockMinCount, SellStockMaxCount);
        questData.moriumReward = storeData.GetPriceMorium(storeData.data.level) / MultipleRewardByUpgradeLevel;
        questData.askariReward = (SellStockDefaultAskari * storeData.addLevelMultiplier[Mathf.Clamp(storeData.data.shelvesBoughtCount + 1, 0, storeData.addLevelMultiplier.Length - 1)]) / MultipleRewardByUpgradeLevel;
        questData.idType = GetSellingPlatformId();
        noticableIcon.target = GetSellingPlatformNPCTarget();
        questData.npcNameType = GetSellingPlatformNPCNameType();
        UpdateTranslation();
        UpdateAcceptedQuestEvents();
    }
    public int GetMaxCountQuest(int min, int max)
    {
        // StoreData.QuestData questData = storeData.data.questData;
        // if (questData.acceptQuestCount > MultipleRewardByUpgradeLevel)
        // {
        //     int multiplier = questData.acceptQuestCount - MultipleRewardByUpgradeLevel + 1;
        //     return Random.Range(min * multiplier, max * multiplier + 1);
        // }
        return Random.Range(min, max + 1);
    }
    private int GetSellingPlatformId()
    {
        StoreData.Data data = storeData.data;

        List<int> ids = new List<int>();
        for (int i = 0; i < sp.data.Length; i++)
        {
            if (sp.data[i].level <= data.shelvesBoughtCount)
            {
                ids.Add(i);
            }
        }
        Debug.Log("ids count" + ids.Count);
        Debug.Log("level count" + data.level);

        return ids[Random.Range(0, ids.Count)];
    }
    private Transform GetSellingPlatformNPCTarget()
    {
        StoreData.QuestData questData = storeData.data.questData;
        return sp.npcDatas[(int)sp.data[questData.idType].type].questTarget;
    }
    private NPCs.Type GetSellingPlatformNPCNameType()
    {
        StoreData.QuestData questData = storeData.data.questData;
        return sp.npcDatas[(int)sp.data[questData.idType].type].npcNameType;
    }
    public void UpdateAcceptedQuestEvents()
    {
        StoreData.QuestData questData = storeData.data.questData;

        if (questData.type == StoreData.QuestType.SellStock)
        {
            acceptedQuestEvent = sellStockWhenQuestAccept;
        }
    }
    public void AddQuestCount(int idType, StoreData.QuestType questType)
    {
        StoreData.QuestData questData = storeData.data.questData;
        if (questData.nowCount >= questData.maxCount)
        {
            return;
        }
        if (!questData.isQuestAccept)
        {
            return;
        }
        if (questType != questData.type)
        {
            return;
        }
        if (idType != questData.idType)
        {
            return;
        }
        questData.nowCount++;
        whenAfterAddQuestCount?.Invoke();
        if (questData.nowCount >= questData.maxCount)
        {
            whenQuestComplete?.Invoke();
        }

    }
    public void GetReward(int multiplier)
    {

        StoreData.QuestData questData = storeData.data.questData;
        if (!questData.isQuestAccept)
        {
            return;
        }
        // EconomyCurrency.Instance.AddMorium(questData.moriumReward * multiplier);
        // EconomyNotif.Instance.Append("morium", "Morium", questData.moriumReward * multiplier, true);
        EconomyCurrency.Instance.AddAskari(questData.askariReward * multiplier);
        EconomyNotif.Instance.Append("askari", "Askari", questData.askariReward * multiplier, true);
        GetQuest();


    }

}
