using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Workers : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public StoreData storeData;
    [SerializeField] private RealtimeDataBuyerSystem realtimeDataBuyerSystem;
    [Header("Required Things")]
    [SerializeField] private Transform spawnerLand;
    public QuestGenerator questGenerator;
    [Header("Required Things - UI")]

    [SerializeField] private GameObject templatePrefab;
    [SerializeField] private GameObject templatePrefabHire;
    [SerializeField] private RectTransform land;
    [SerializeField] private GameObject waitText;
    [SerializeField] private Canvas cardCanvas;
    [SerializeField] private TextMeshProUGUI fieldUpgradeText;
    [Header("Configs")]
    [SerializeField] public int maxLevel;

    [SerializeField] private int defaultMorium;
    [SerializeField] private int defaultAskari;
    [SerializeField] private int defaultAskariUpgradeLevel;

    [SerializeField] private UnityEvent generalOnDown;
    [SerializeField] private UnityEvent generalOnUp;

    [SerializeField] private UnityEvent addWorker;
    [SerializeField] private UnityEvent unlockAds;
    [SerializeField] private UnityEvent upgradeWithAds;

    [Header("Cashier")]
    [SerializeField] private int maxCashier;
    [SerializeField] public float cashierSefaultSpeed;
    [SerializeField] private GameObject cashierTranslation;
    [SerializeField] private int cashierFreeAds;
    [SerializeField] private int[] adsInCashierLevel;
    [SerializeField] private GameObject[] cashierPrefab;
    [Header("Restocker")]
    [SerializeField] private int maxRestocker;
    [SerializeField] public float RestockerDefaultSpeed;
    [SerializeField] private GameObject restockerTranslation;
    [SerializeField] private int restockerFreeAds;
    [SerializeField] private int[] adsInrestockerLevel;
    [SerializeField] private GameObject[] restockerPrefab;
    //hide
    private GameObject[] tempDelete;
    private List<GameObject> tempAdd;
    [Header("Previews")]
    public Type typeNow = Type.Cashier;
    public Type typeNowSimpleUi;
    public bool isGenerating;
    public int bookIdWorker;
    public bool isSimpleUI;
    private bool adsFree;
    private int cashierCount;
    private int restockerCount;
    public enum Type
    {
        Cashier,
        Restocker
    }
    private void Start()
    {
        WorkerSpawner();
    }
    public void ChangeType(WorkerChangeCardType bagChangeType)
    {
        typeNow = bagChangeType.type;

        Run();
    }
    public void Run()
    {
        if (isGenerating)
        {
            return;
        }
        isGenerating = true;


        if (tempAdd != null)
        {
            tempDelete = tempAdd.ToArray();
        }

        tempAdd = new List<GameObject>();

        StartCoroutine(Generate());

    }
    public void UnlockAdsEvents()
    {
        unlockAds?.Invoke();
    }
    private IEnumerator Generate()
    {
        StoreData.WorkerData workerData = storeData.data.workerData;
        int itemHaveQuantity = 0;
        cardCanvas.enabled = false;
        waitText.SetActive(true);
        if (tempDelete != null)
        {
            for (int i = 0; i < tempDelete.Length; i++)
            {
                Destroy(tempDelete[i]);
                yield return null;

            }
        }
        List<StoreData.Worker> datas = null;
        string translation = null;
        int freeAds = 0;
        int maxWorker = 0;
        if (typeNow == Type.Cashier)
        {
            datas = workerData.cashiers;
            translation = cashierTranslation.name;
            freeAds = cashierFreeAds;
            maxWorker = maxCashier;
        }
        else if (typeNow == Type.Restocker)
        {
            datas = workerData.restocker;
            translation = restockerTranslation.name;
            freeAds = restockerFreeAds;
            maxWorker = maxRestocker;
        }


        GameObject prefab;
        for (int i = 0; i < datas.Count; i++)
        {
            StoreData.Worker data = datas[i];
            WorkersCard bagCard = templatePrefab.GetComponentInChildren<WorkersCard>();
            bagCard.maxLevel = maxLevel;
            bagCard.titleTranslationName = translation;
            bagCard.level = data.level;
            bagCard.id = i;
            bagCard.OnDown = generalOnDown;
            bagCard.OnUp = generalOnUp;
            bagCard.workers = this;
            bagCard.isLock = false;

            if (!adsFree && i >= freeAds)
            {
                bagCard.isLock = true;
            }
            prefab = Instantiate(templatePrefab, Vector3.zero, Quaternion.identity, land);
            tempAdd.Add(prefab);
            itemHaveQuantity++;
            yield return null;

        }
        if (datas.Count < maxWorker)
        {
            WorkersHireCard hireCard = templatePrefabHire.GetComponentInChildren<WorkersHireCard>();
            hireCard.typeTranslationName = translation;
            hireCard.storeData = storeData;
            hireCard.moriumPrice = defaultMorium;
            hireCard.askariPrice = GetHirePrice(defaultAskari, itemHaveQuantity);
            hireCard.OnDown = generalOnDown;
            hireCard.OnUp = addWorker;
            hireCard.workers = this;
            hireCard.isLock = false;
            if (datas.Count >= freeAds && !adsFree)
            {
                hireCard.isLock = true;
            }
            prefab = Instantiate(templatePrefabHire, Vector3.zero, Quaternion.identity, land);
            tempAdd.Add(prefab);
        }



        waitText.SetActive(false);
        cardCanvas.enabled = true;
        isGenerating = false;





    }
    private int GetHirePrice(int costPrice, int workerCount)
    {
        return costPrice * storeData.addLevelMultiplier[workerCount];
    }
    public void AddWorker()
    {
        StoreData.WorkerData workerData = storeData.data.workerData;
        int moriumPrice, askariPrice;

        moriumPrice = defaultMorium;
        List<StoreData.Worker> workers = null;
        if (typeNow == Type.Cashier)
        {
            workers = workerData.cashiers;


        }
        else if (typeNow == Type.Restocker)
        {
            workers = workerData.restocker;

        }
        askariPrice = GetHirePrice(defaultAskari, workers.Count);

        if (!EconomyCurrency.Instance.CanAskariDecrease(askariPrice))
        {
            return;
        }
        if (!EconomyCurrency.Instance.CanMoriumDecrease(moriumPrice))
        {
            return;
        }
        StoreData.Worker worker = new StoreData.Worker();
        worker.isBought = true;
        workers.Add(worker);
        InstanceCashierWorker(workers.Count - 1, typeNow);
        AddWrokerCount(typeNow);
        EconomyCurrency.Instance.DecreaseAskari(askariPrice);
        EconomyNotif notif = EconomyNotif.Instance;
        notif.Append("morium", "Morium", moriumPrice, false);
        notif.Append("askari", "Askari", askariPrice, false);
        Run();

    }
    private void WorkerSpawner()
    {
        StartCoroutine(WorkerSpawnerIE());
    }
    public IEnumerator WorkerSpawnerIE()
    {
        Type type = typeNow;
        int freeAds = 0;
        int lenghtWorkerType = System.Enum.GetValues(typeof(Type)).Length;
        List<StoreData.Worker> datas = null;


        for (int i = 0; i < lenghtWorkerType; i++)
        {
            type = (Type)i;
            if (type == Type.Cashier)
            {
                freeAds = cashierFreeAds;
                datas = storeData.data.workerData.cashiers;

            }
            else if (type == Type.Restocker)
            {
                freeAds = restockerFreeAds;
                datas = storeData.data.workerData.restocker;


            }

            for (int j = GetWorkerCount(type); j < datas.Count; j++)
            {
                if (!adsFree && j >= freeAds)
                {
                    break;
                }
                AddWrokerCount(type);
                InstanceCashierWorker(j, type);

                yield return null;
            }
        }

    }
    private int GetWorkerCount(Type type)
    {
        if (type == Type.Cashier)
        {
            return cashierCount;
        }
        else if (type == Type.Restocker)
        {
            return restockerCount;


        }
        return 0;
    }
    private void AddWrokerCount(Type type)
    {
        if (type == Type.Cashier)
        {
            cashierCount++;
        }
        else if (type == Type.Restocker)
        {
            restockerCount++;

        }

    }
    public void UnlockAds()
    {
        adsFree = true;
        StartCoroutine(WorkerSpawnerIE());
        Run();
    }
    public void InstanceCashierWorker(int id, Type type)
    {
        if (type == Type.Cashier)
        {
            CashierWorker cashierWorker = cashierPrefab[0].GetComponent<CashierWorker>();
            cashierWorker.workers = this;
            cashierWorker.realtimeDataBuyerSystem = realtimeDataBuyerSystem;
            cashierWorker.idCashierWorker = id;
            cashierWorker.questGenerator = questGenerator;
            Instantiate(cashierPrefab[0], spawnerLand.position, Quaternion.identity, spawnerLand);

        }
        else if (type == Type.Restocker)
        {


        }

    }
    public void UpdateUpgradeUI()
    {
        Type type;
        type = typeNow;
        if (isSimpleUI)
        {
            type = typeNowSimpleUi;
        }
        for (int i = 0; i < GetAdsInWorkerLevel(type).Length; i++)
        {
            if (GetWorkerData(type)[bookIdWorker].level == GetAdsInWorkerLevel(type)[i] - 1)
            {
                fieldUpgradeText.text =
                    Lean.Localization.LeanLocalization.GetTranslationText("Worker_UpgradeWithAds") + " Level "
                    + (GetWorkerData(type)[bookIdWorker].level + 2);
                return;
            }
        }
        int askariPrice = defaultAskariUpgradeLevel * (GetWorkerData(type)[bookIdWorker].level + 1);
        fieldUpgradeText.text = "<sprite name=\"askari\"> " + storeData.MoneyString(askariPrice)
        + " " + Lean.Localization.LeanLocalization.GetTranslationText("Worker_FieldText") + " Level "
        + (GetWorkerData(type)[bookIdWorker].level + 2);
    }
    private int[] GetAdsInWorkerLevel(Type type)
    {
        if (type == Type.Cashier)
        {
            return adsInCashierLevel;
        }
        else if (type == Type.Restocker)
        {
            return adsInrestockerLevel;

        }
        return null;
    }
    private List<StoreData.Worker> GetWorkerData(Type type)
    {
        StoreData.WorkerData workerData = storeData.data.workerData;

        if (type == Type.Cashier)
        {
            return workerData.cashiers;
        }
        else if (type == Type.Restocker)
        {
            return workerData.restocker;

        }
        return null;
    }
    public void Upgrade()
    {
        Type type;
        type = typeNow;
        if (isSimpleUI)
        {
            type = typeNowSimpleUi;
        }
        for (int i = 0; i < GetAdsInWorkerLevel(type).Length; i++)
        {
            if (GetWorkerData(type)[bookIdWorker].level == GetAdsInWorkerLevel(type)[i] - 1)
            {
                upgradeWithAds?.Invoke();
                return;
            }
        }

        int askariPrice = defaultAskariUpgradeLevel * (GetWorkerData(type)[bookIdWorker].level + 1);
        EconomyCurrency economyCurrency = EconomyCurrency.Instance;
        EconomyNotif economyNotif = EconomyNotif.Instance;
        if (!economyCurrency.CanAskariDecrease(askariPrice))
        {
            return;
        }
        economyCurrency.DecreaseAskari(askariPrice);
        economyNotif.Append("askari", "Askari", askariPrice, false);
        GetWorkerData(type)[bookIdWorker].level++;
        Run();
    }
    public void UpgradeWithAds()
    {
        Type type;
        type = typeNow;
        if (isSimpleUI)
        {
            type = typeNowSimpleUi;
        }

        GetWorkerData(type)[bookIdWorker].level++;
        Run();
    }
    public void DIsableSimpleUiTrigger()
    {
        isSimpleUI = false;
    }


}

