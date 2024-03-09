
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StoreData : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private SellingPlatformProductData shelvesData;
    [Header("Configs")]
    [Range(0f, 1f)]
    public float minScore;

    [Header("Level Difficulty")]
    public int askariCostDefault;
    public int moriumCostDefault;
    public float incomeDefault;

    public int[] addLevelMultiplier;
    [Header("Preview")]
    public Data data;
    public static StoreData Instance;
    private void Awake()
    {
        if (!(StoreDataSave.IsExist() && StoreDataSave.instance != null && UnityEngine.PlayerPrefs.GetInt("storePass") == StoreDataSave.instance.pass))
        {
            StateLoaded.initLoaded++;

            StoreDataSave.instance.data = new Data();
            int seed = (int)System.DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int pass = UnityEngine.Random.Range(1000, 3000);
            UnityEngine.PlayerPrefs.SetInt("storePass", pass);
            StoreDataSave.instance.pass = pass;

        }


        Instance = this;
        data = StoreDataSave.instance.data;

        StartCoroutine(CheckStateLoadedRequirement());
        if (data.shelvesDatas.Count < shelvesData.data.Length)
        {
            int offset = shelvesData.data.Length - data.shelvesDatas.Count;
            int first = data.shelvesDatas.Count;
            for (int i = first; i < offset; i++)
            {
                ShelvesData newData = new ShelvesData();
                newData.type = shelvesData.data[i].type;
                newData.isBought = false;
                data.shelvesDatas.Add(newData);
            }
            data.shelvesDatas[0].isBought = true;
            data.shelvesDatas[0].type = SellingPlatformProductData.Type.Snack;
        }



    }
    private IEnumerator CheckStateLoadedRequirement()
    {
        int numThisState = 5;
        while (!(StateLoaded.Loaded + 1 == numThisState))
        {
            yield return null;
        }
        StateLoaded.Loaded++;

    }
    [System.Serializable]
    public class StoreDataSave : ThirtySec.Serializable<StoreDataSave>
    {
        public int pass;
        public Data data;


    }
    [System.Serializable]
    public class Data
    {
        public int askari = 2000;
        public int morium;
        public bool IsTutorial;
        public string name = "";
        public int level = 1;
        public float score;
        public float income;
        public float trashDIfficulty = .2f;
        public float fillShelvesDIfficulty = .2f;
        public List<ShelvesData> shelvesDatas = new List<ShelvesData>();
        public int shelvesBoughtCount = 1;
        public QuestData questData = new QuestData();
        public WorkerData workerData = new WorkerData();
        public LandsSellerData landsSellerData = new LandsSellerData();
        public List<bool> areaLockerData = new List<bool>();
        public List<BagCardGenerator.Data> sellingPlatform = new List<BagCardGenerator.Data>();
        public List<BagCardGenerator.Data> variousThings = new List<BagCardGenerator.Data>();
        public List<BagCardGenerator.Data> trash = new List<BagCardGenerator.Data>();
        public List<int> stockQuantity = new List<int>();
    }
    [System.Serializable]

    public class LandsSellerData
    {
        public bool[] isLandsBought;
        public int landsBoughtCount;
    }

    public enum QuestType
    {
        SellStock
    }
    [System.Serializable]
    public class WorkerData
    {
        public List<Worker> cashiers = new List<Worker>();
        public List<Worker> restocker = new List<Worker>();

    }
    [System.Serializable]

    public class Worker
    {
        public bool isBought;
        public int level;
    }
    [System.Serializable]
    public class ShelvesData
    {
        public bool isBought;
        public SellingPlatformProductData.Type type;
    }

    [System.Serializable]
    public class QuestData
    {
        public int acceptQuestCount;
        public bool isQuestReady;
        public bool isQuestAccept;
        public NPCs.Type npcNameType;
        public int nowCount;
        public int maxCount;
        public QuestType type;
        public int idType;
        public int moriumReward;
        public int askariReward;
    }
    public float GetIncome(int level)
    {
        float income = incomeDefault * level;
        return income;
    }
    public int GetPriceAskari(int level)
    {
        float price = askariCostDefault * (incomeDefault * level);

        return askariCostDefault * addLevelMultiplier[level];
    }
    public int GetPriceMorium(int level)
    {
        return moriumCostDefault;
    }
    public void AddScore(float score)
    {
        data.score = Mathf.Clamp(data.score + score, minScore, 1f);
    }
    public void DeacreseScore(float score)
    {
        data.score = Mathf.Clamp(data.score - score, minScore, 1f);

    }
    public string MoneyString(float number)
    {
        string formattedNumber = "";
        if (number >= 1000000000f)
        {
            formattedNumber = (number / 1000000000f).ToString("0.##") + Lean.Localization.LeanLocalization.GetTranslationText("Billion");

        }
        else if (number >= 1000000f)
        {
            formattedNumber = (number / 1000000f).ToString("0.##") + Lean.Localization.LeanLocalization.GetTranslationText("Million");
        }
        else if (number >= 1000f)
        {
            formattedNumber = (number / 1000f).ToString("0.##") + Lean.Localization.LeanLocalization.GetTranslationText("Thousand");
        }
        else
        {
            formattedNumber = number.ToString();
        }
        return formattedNumber;
    }


}