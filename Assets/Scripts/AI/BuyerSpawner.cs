
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuyerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Configs")]
    [SerializeField] private float timeScale;
    [SerializeField] private int maxBuyerCount;
    [SerializeField] private float spawnBuyerDelay;
    [SerializeField] private float waitingCashierQueueTime;
    [Range(0, 100)]
    [SerializeField] private int chanceBuyerAdviceTrash;
    [Range(0, 100)]
    [SerializeField] private int chanceBuyerAddScore;

    [Header("Chat - Cashier Late - Middle")]
    [SerializeField] private string[] chatCashierLateMiddle;

    [Header("Chat - Cashier Late - Finish")]
    [SerializeField] private string[] chatCashierLateFinish;
    [Header("Chat - Trash Advice")]
    [SerializeField] private string[] chatTrashAdvice;
    [Header("Chat - Cashier Not Available")]
    [SerializeField] private string[] chatCashierNotAvailable;
    [Header("Chat - Cashier Fast")]
    [SerializeField] private string[] chatCashierFast;
    [Header("Chat - Stock Not Available")]
    [SerializeField] private string[] chatStockNotAvailable;
    [Header("Chat - Shelves Not Available")]
    [SerializeField] private string[] chatShelvesNotAvailable;
    [Header("Buyer Male")]
    [SerializeField] private GameObject[] BuyerPrefab;
    [Header("Spawn Point/Exit Point")]
    [SerializeField] private Transform[] spawnTarget;
    [Header("Target to Store")]
    [SerializeField] private Transform[] TargetWhenShelvesNotAvailable;
    [Header("Preview")]
    public int currentBuyerCount;
    public static BuyerSpawner Instance;
    public event System.Action whenDisable;
    private bool isDisable;
    private List<int> buyerIds;
    private void Awake()
    {
        Instance = this;
    }
    public void Disable()
    {
        isDisable = true;
    }
    public void Enable()
    {
        isDisable = false;
    }
    private void Start()
    {
        ReRandomBuyerId();

    }
    private void OnDisable()
    {
        whenDisable?.Invoke();
        CGMPacking cashier = CGMPacking.Instance;
        cashier.spAIDatas = new List<BuyerAI.SPAIData>();

    }
    private void OnEnable()
    {
        StartCoroutine(SpawnBuyer());
    }
    private void ReRandomBuyerId()
    {
        buyerIds = new List<int>();
        for (int i = 0; i < BuyerPrefab.Length; i++)
        {
            buyerIds.Add(i);
        }
        int n = buyerIds.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = buyerIds[i];
            buyerIds[i] = buyerIds[j];
            buyerIds[j] = temp;
        }
    }
    private IEnumerator SpawnBuyer()
    {
        while (true)
        {
            Time.timeScale = timeScale;
            if (BuildNavmesh.navmeshReady)
            {

                Debug.Log("Dapat");
                break;

            }
            Debug.Log("puter puter");
            yield return null;

        }
        while (true)
        {
            if (isDisable)
            {
                yield return null;
                continue;
            }

            if (currentBuyerCount <= maxBuyerCount * StoreData.Instance.data.score)
            {
                int randomCashierLateMiddle = Random.Range(0, chatCashierLateMiddle.Length);
                int randomCashierLateFinish = Random.Range(0, chatCashierLateFinish.Length);
                int randomTrashAdvice = Random.Range(0, chatTrashAdvice.Length);

                int spawnTargetRandomIndex = Random.Range(0, spawnTarget.Length);
                Vector3 spawnTargetPos = spawnTarget[spawnTargetRandomIndex].transform.position;
                BuyerAI buyerAI = BuyerPrefab[0].GetComponent<BuyerAI>();
                buyerAI.exitTarget = spawnTargetPos;
                buyerAI.chatCashierLateMiddle = chatCashierLateMiddle[randomCashierLateMiddle];
                buyerAI.chatCashierLateFinish = chatCashierLateFinish[randomCashierLateFinish];
                buyerAI.waitingQueueTime = waitingCashierQueueTime;
                buyerAI.chatTrashAdvice = chatTrashAdvice[randomTrashAdvice];
                buyerAI.chatCashierNotAvailable = chatCashierNotAvailable[Random.Range(0, chatCashierNotAvailable.Length)];
                buyerAI.targetWhenShelvesNotAvailable = TargetWhenShelvesNotAvailable[Random.Range(0, TargetWhenShelvesNotAvailable.Length)].position;
                if (TrashGenerator.Instance.trashCount >= TrashGenerator.Instance.minTrashToAdvice)
                {
                    buyerAI.isTrashAdvisor = Random.Range(0, 101) <= chanceBuyerAdviceTrash;
                }
                buyerAI.isAddScore = Random.Range(0, 101) <= chanceBuyerAddScore;
                buyerAI.chatCashierFast = chatCashierFast[Random.Range(0, chatCashierFast.Length)];
                Instantiate(BuyerPrefab[buyerIds[0]], spawnTargetPos, Quaternion.identity, transform);
                buyerIds.RemoveAt(0);
                if (buyerIds.Count == 0)
                {
                    ReRandomBuyerId();
                }
                currentBuyerCount++;
                yield return new WaitForSeconds(spawnBuyerDelay);
            }
            yield return null;
        }


    }

}
