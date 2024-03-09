using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Lean.Localization;

public class BuyerAI : MonoBehaviour
{
    [SerializeField] private Canvas bubbbleChatCAnvas;
    [SerializeField] private LeanLocalizedTextMeshProUGUI bubbleChatTranlation;
    private SPAIData[] spAIDatas;
    public int maxSellingPlatform = 2;
    public float maxBuySellingPlatformInPercent = 10;
    private int sellingPlatformNextId;
    private bool isStart;
    private float speed;
    private bool isCancelBuy;
    private bool isExitCashier;
    private bool isExiting;
    [SerializeField] float moveTweenTime;
    public NavMeshAgent agent;
    public Animator anime;
    [SerializeField] private AIStuckResolver aIStuckResolver;
    private Vector3 currentDestination;
    public Vector3 exitTarget;
    public Vector3 targetWhenShelvesNotAvailable;
    public bool isTrashAdvisor;
    public bool isAddScore;

    public float waitingQueueTime;
    public string chatCashierLateMiddle;
    public string chatCashierLateFinish;
    public string chatTrashAdvice;
    public string chatCashierNotAvailable;
    public string chatCashierFast;
    [System.Serializable]
    public class SPAIData
    {
        public int index;
        public int stockQuantityToBuy;
        public int IDsp;
        public SPAIData(RealtimeDataBuyerSystem RDBSystem, int newIndex, float maxBuySellingPlatformInPercent)
        {
            index = RDBSystem.SellingPlatformsRDID[newIndex];
            IDsp = RDBSystem.SellingPlatformsRD[index].IDsp;
            int percent = (int)((maxBuySellingPlatformInPercent / 100f) * (float)RDBSystem.SellingPlatformsRD[index].maxStockQuantity); ;
            int max = Random.Range(1, 4);

            stockQuantityToBuy = max;

        }
    }
    private void Start()
    {
        bubbbleChatCAnvas.enabled = false;
    }
    private void OnEnable()
    {
        bubbbleChatCAnvas.enabled = false;

        aIStuckResolver.stuck += ResetPath;
        if (!isStart)
        {
            speed = agent.speed;
            isStart = true;

        }
        else
        {
            agent.speed = speed;

        }
        if (isExitCashier)
        {
            StartCoroutine(GoToExit());
        }
        else
        {
            isCancelBuy = false;
            StartCoroutine(Run());

        }



    }
    private void OnDestroy()
    {
        aIStuckResolver.stuck -= ResetPath;
    }
    public void ResetPath()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
        agent.SetDestination(currentDestination);

    }
    private IEnumerator Run()
    {
        yield return IsLoaded();
        yield return InitData();
        yield return GoToSellingPlatforms();
        yield return GoToCashier();
        yield return GoToExit();
    }
    private IEnumerator IsLoaded()
    {
        while (!BuildNavmesh.navmeshReady)
        {
            Debug.Log("mulai5");

            yield return null;
        }
    }
    private IEnumerator InitData()
    {
        int getDataLength = Mathf.Clamp(Random.Range(1, maxSellingPlatform + 1), 0, RealtimeDataBuyerSystem.Instance.SellingPlatformsRD.Count);

        int max = getDataLength;

        spAIDatas = new SPAIData[max];
        if (RealtimeDataBuyerSystem.Instance.SellingPlatformsRDID.Count == 0)
        {
            RealtimeDataBuyerSystem.Instance.FillSellingPlatformsRDID();
        }
        ;
        for (int i = 0; i < spAIDatas.Length; i++)
        {
            spAIDatas[i] = new SPAIData(RealtimeDataBuyerSystem.Instance, 0, maxBuySellingPlatformInPercent);
            Debug.Log("initdata6");
            RealtimeDataBuyerSystem.Instance.SellingPlatformsRDID.RemoveAt(0);
            if (RealtimeDataBuyerSystem.Instance.SellingPlatformsRDID.Count == 0)
            {
                RealtimeDataBuyerSystem.Instance.FillSellingPlatformsRDID();
            }

            yield return null;
        }
        if (getDataLength == 0)
        {
            anime.SetTrigger("walk");
            agent.SetDestination(targetWhenShelvesNotAvailable);
            currentDestination = targetWhenShelvesNotAvailable;
            while (true)
            {
                TrashGeneratorInit();
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    anime.SetTrigger("idle");
                    break;
                }
                yield return null;
            }
            yield return RandomMove();

            yield return GoToExit();

        }
    }
    private IEnumerator GoToSellingPlatforms()
    {
        sellingPlatformNextId = 0;
        while (true)
        {
            if (sellingPlatformNextId < spAIDatas.Length)
            {
                yield return CheckSellingPlatformBuyerSlot();

            }
            else
            {
                break;
            }



            yield return null;
        }

    }
    private IEnumerator GoToCashier()
    {
        bool isCashierAble = false;
        for (int i = 0; i < spAIDatas.Length; i++)
        {
            if (spAIDatas[i].stockQuantityToBuy > 0)
            {
                isCashierAble = true;
            }

        }
        if (isCashierAble)
        {
            yield return CheckCashierBuyerSlot();
        }

    }
    private IEnumerator CheckCashierBuyerSlot()
    {
        bool isFoundCashier = false;
        StartCoroutine(ChatTrashAdvice());
        while (true)
        {
            List<CashierRealtimeData> cashierRDs = RealtimeDataBuyerSystem.Instance.cashierRD;
            Debug.Log("cek134");
            int lowestIndex = 0;
            if (cashierRDs.Count == 0)
            {
                yield return RandomMove();
                RestoreStock(spAIDatas, false);
                StartCoroutine(ChatCashierNotAvailable());
                yield return RandomMove();

                break;
            }
            for (int i = 1; i < cashierRDs.Count; i++)
            {
                if (cashierRDs[i].queueNumber < cashierRDs[lowestIndex].queueNumber)
                {
                    lowestIndex = i;
                }
            }
            if (cashierRDs[lowestIndex].queueNumber >= cashierRDs[lowestIndex].buyerSlotClaim.Length)
            {
                yield return RandomMove();
            }
            else
            {
                int queueNumber = cashierRDs[lowestIndex].queueNumber;

                cashierRDs[lowestIndex].buyerSlotClaim[queueNumber] = true;
                currentDestination = cashierRDs[lowestIndex].buyerSlotTranfrom[queueNumber].position;

                agent.SetDestination(cashierRDs[lowestIndex].buyerSlotTranfrom[queueNumber].position);
                anime.SetTrigger("walk");
                cashierRDs[lowestIndex].queueNumber++;
                isFoundCashier = true;
                yield return WhenOnCashierBuyerSlot(lowestIndex, cashierRDs, queueNumber);
                break;
            }
            // for (int i = 0; i < cashierRDs.Count; i++)
            // {
            //     // CashierRealtimeData cashierRD = cashierRDs[i];
            //     if (cashierRDs[i].queueNumber >= cashierRDs[i].buyerSlotClaim.Length)
            //     {
            //         yield return RandomMove();
            //     }
            //     else
            //     {

            //         int queueNumber = cashierRDs[i].queueNumber;

            //         cashierRDs[i].buyerSlotClaim[queueNumber] = true;
            //         currentDestination = cashierRDs[i].buyerSlotTranfrom[queueNumber].position;

            //         agent.SetDestination(cashierRDs[i].buyerSlotTranfrom[queueNumber].position);
            //         anime.SetTrigger("walk");
            //         cashierRDs[i].queueNumber++;
            //         isFoundCashier = true;
            //         yield return WhenOnCashierBuyerSlot(i, cashierRDs, queueNumber);
            //         break;

            //     }
            //     yield return null;
            // }

            if (isFoundCashier)
            {
                break;
            }

            yield return null;

        }
    }
    private IEnumerator ChatCashierNotAvailable()
    {


        bubbleChatTranlation.TranslationName = chatCashierNotAvailable;
        bubbbleChatCAnvas.enabled = true;
        yield return new WaitForSeconds(chatTrashAdvice.Length * .5f);
        bubbbleChatCAnvas.enabled = false;


    }
    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);

    }
    private IEnumerator GoToExit()
    {
        StartCoroutine(WaitToDestroy());

        if (!isExiting)
        {
            BuyerSpawner.Instance.currentBuyerCount--;
            isExiting = true;

        }

        aIStuckResolver.stuck += ResetPath;

        anime.SetTrigger("walk");


        agent.SetDestination(exitTarget);
        currentDestination = exitTarget;
        while (true)
        {
            TrashGeneratorInit();
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Destroy(gameObject);
                break;
            }
            yield return null;
        }

    }
    private IEnumerator WhenOnCashierBuyerSlot(int index, List<CashierRealtimeData> cashierRD, int queueNumber)
    {
        while (true)
        {
            TrashGeneratorInit();
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                bool isStop = false;
                Vector3 targetPos = DOMoveSupportPosition(cashierRD[index].buyerSlotTranfrom[queueNumber].position);
                Vector3 targetRot = DORotateSupportRotation(cashierRD[index].buyerSlotTranfrom[queueNumber].rotationY);
                transform.DOMove(targetPos, moveTweenTime).SetEase(Ease.InOutQuad)
                .OnComplete(
                    () =>
                    {
                        isStop = true;
                        transform.DORotate(targetRot, moveTweenTime).SetEase(Ease.InOutQuad);
                        anime.SetTrigger("idle");

                    }
                );
                while (!isStop)
                {
                    yield return null;
                };
                speed = agent.speed;
                yield return WhenOnQueueCashier(queueNumber, cashierRD, speed, index);
                break;
            }
            yield return null;
        }
    }
    private IEnumerator WhenOnQueueCashier(int queueNumber, List<CashierRealtimeData> cashierRD, float speed, int index)
    {
        aIStuckResolver.stuck -= ResetPath;
        StartCoroutine(ChatWaitingOnCashierQueue());
        while (true)
        {
            if (queueNumber == 0)
            {

                agent.speed = 0;
                cashierRD[index].cashierManager.sPAIDatas = spAIDatas.ToList();
                cashierRD[index].cashierManager.isStillServe = true;
                cashierRD[index].cashierManager.Run();
                while (cashierRD[index].cashierManager.isStillServe)
                {
                    if (isCancelBuy)
                    {
                        RestoreStock(cashierRD[index].cashierManager.sPAIDatas.ToArray());
                        cashierRD[index].cashierManager.Reset();
                        cashierRD[index].cashierManager.MenuEnabler();

                        cashierRD[index].cashierManager.isStillServe = false;
                        break;
                    }
                    yield return null;
                }
                StartCoroutine(ChatAddScore());
                cashierRD[index].buyerSlotClaim[queueNumber] = false;
                cashierRD[index].queueNumber--;
                agent.speed = speed;
                isExitCashier = true;


                break;
            }
            else if (!cashierRD[index].buyerSlotClaim[queueNumber - 1])
            {
                agent.speed = 0;

                cashierRD[index].buyerSlotClaim[queueNumber] = false;
                queueNumber--;
                cashierRD[index].buyerSlotClaim[queueNumber] = true;
                bool isStop = false;
                anime.SetTrigger("walk");
                Vector3 targetPos = DOMoveSupportPosition(cashierRD[index].buyerSlotTranfrom[queueNumber].position);
                Vector3 targetRot = DORotateSupportRotation(cashierRD[index].buyerSlotTranfrom[queueNumber].rotationY);
                transform.DOMove(targetPos, moveTweenTime).SetEase(Ease.InOutQuad)
                .OnComplete(
                    () =>
                    {
                        isStop = true;
                        transform.DORotate(targetRot, moveTweenTime).SetEase(Ease.InOutQuad);
                        anime.SetTrigger("idle");

                    }
                );
                while (!isStop)
                {
                    yield return null;
                };

            }
            else if (isCancelBuy)
            {
                RestoreStock(spAIDatas);
                cashierRD[index].buyerSlotClaim[queueNumber] = false;
                queueNumber--;
                agent.speed = speed;
                isExitCashier = true;


                break;
            }
            yield return null;
        }
    }
    private void RestoreStock(SPAIData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            StockInventoryData.Instance.stockDisplayData[data[i].IDsp - 1] += data[i].stockQuantityToBuy;
        }
        DecreaseScore();

    }
    private void RestoreStock(SPAIData[] data, bool isDecreaseScore)
    {
        for (int i = 0; i < data.Length; i++)
        {
            StockInventoryData.Instance.stockDisplayData[data[i].IDsp - 1] += data[i].stockQuantityToBuy;
        }
        if (isDecreaseScore)
        {
            DecreaseScore();

        }

    }
    private void DecreaseScore()
    {
        if (StoreData.Instance.minScore == StoreData.Instance.data.score)
        {
            return;
        }
        StoreData.Instance.DeacreseScore(.2f);

        EconomyNotif.Instance.Append("stars", Lean.Localization.LeanLocalization.GetTranslationText("Notif_Score"), 20, false);
    }
    private void AddScore()
    {
        if (1f == StoreData.Instance.data.score)
        {
            return;
        }
        StoreData.Instance.AddScore(.2f);

        EconomyNotif.Instance.Append("stars", Lean.Localization.LeanLocalization.GetTranslationText("Notif_Score"), 20, true);
    }
    private IEnumerator ChatWaitingOnCashierQueue()
    {
        yield return new WaitForSeconds(waitingQueueTime / 2);
        if (!isExitCashier)
        {
            bubbleChatTranlation.TranslationName = chatCashierLateMiddle;
            bubbbleChatCAnvas.enabled = true;
        }

        yield return new WaitForSeconds(chatCashierLateMiddle.Length * .5f);
        bubbbleChatCAnvas.enabled = false;
        yield return new WaitForSeconds(waitingQueueTime / 2);

        if (!isExitCashier)
        {
            bubbleChatTranlation.TranslationName = chatCashierLateFinish;
            bubbbleChatCAnvas.enabled = true;
            isCancelBuy = true;
        }


        yield return new WaitForSeconds(chatCashierLateFinish.Length * .5f);
        bubbbleChatCAnvas.enabled = false;



    }
    private IEnumerator ChatTrashAdvice()
    {
        if (isTrashAdvisor && TrashGenerator.Instance.trashCount >= TrashGenerator.Instance.minTrashToAdvice)
        {

            bubbleChatTranlation.TranslationName = chatTrashAdvice;
            bubbbleChatCAnvas.enabled = true;
            isTrashAdvisor = false;
            DecreaseScore();
            yield return new WaitForSeconds(chatTrashAdvice.Length * .5f);
            bubbbleChatCAnvas.enabled = false;
        }
    }
    private IEnumerator ChatAddScore()
    {
        if (isAddScore && !isCancelBuy)
        {

            bubbleChatTranlation.TranslationName = chatCashierFast;
            bubbbleChatCAnvas.enabled = true;
            isAddScore = false;
            AddScore();
            yield return new WaitForSeconds(chatCashierFast.Length * .5f);
            bubbbleChatCAnvas.enabled = false;
        }
    }
    private IEnumerator CheckSellingPlatformBuyerSlot()
    {
        yield return GetSlotSellingPlatform();

    }
    private IEnumerator GetSlotSellingPlatform()
    {
        int id = spAIDatas[sellingPlatformNextId].index;
        int stockQuantityToBuy = spAIDatas[sellingPlatformNextId].stockQuantityToBuy;
        bool gotSellingPlatform = false;
        SellingPlatformRealtimeData data = RealtimeDataBuyerSystem.Instance.SellingPlatformsRD[id];
        for (int i = 0; i < data.buyerSlotClaim.Length; i++)
        {
            data = RealtimeDataBuyerSystem.Instance.SellingPlatformsRD[id];

            if (!data.buyerSlotClaim[i])
            {
                data.buyerSlotClaim[i] = true;

                currentDestination = data.buyerSlotTranfrom[i].position;
                agent.SetDestination(data.buyerSlotTranfrom[i].position);
                anime.SetTrigger("walk");
                yield return WhenOnSellingPlatformBuyerSlot(data, stockQuantityToBuy, i);
                gotSellingPlatform = true;
                break;
            }

            yield return null;

        }
        if (!gotSellingPlatform)
        {
            yield return RandomMove();
        }
    }
    private IEnumerator WhenOnSellingPlatformBuyerSlot(SellingPlatformRealtimeData data, int stockQuantityToBuy, int i)
    {
        while (true)
        {
            TrashGeneratorInit();
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                int stockQuantity = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[data.index].stockQuantity;
                if (stockQuantity <= 0)
                {
                    spAIDatas[sellingPlatformNextId].stockQuantityToBuy = 0;

                    anime.SetTrigger("idle");
                    data.buyerSlotClaim[i] = false;
                    sellingPlatformNextId++;
                    break;

                }
                Vector3 targetPos = DOMoveSupportPosition(data.buyerSlotTranfrom[i].position);
                Vector3 targetRot = DORotateSupportRotation(data.buyerSlotTranfrom[i].rotationY);
                DirectionToTarget(targetPos, targetRot);
                yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
                stockQuantity = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[data.index].stockQuantity;

                Debug.Log("berhenti1");
                data.buyerSlotClaim[i] = false;
                if (spAIDatas[sellingPlatformNextId].stockQuantityToBuy > 0 && stockQuantity > 0)
                {


                    stockQuantityToBuy = Mathf.Clamp(stockQuantityToBuy, 0, stockQuantity);
                    data.stockManager.DecreaseStock(stockQuantityToBuy);
                    StoreData.StoreDataSave.instance.data.stockQuantity[data.IDsp - 1] += stockQuantityToBuy;
                    Debug.Log($"buy slot {stockQuantityToBuy}");
                    spAIDatas[sellingPlatformNextId].stockQuantityToBuy = stockQuantityToBuy;
                }
                else
                {
                    spAIDatas[sellingPlatformNextId].stockQuantityToBuy = 0;

                }
                sellingPlatformNextId++;

                break;
            }
            yield return null;
        }
    }
    private void DirectionToTarget(Vector3 targetPos, Vector3 targetRot)
    {
        transform.DOKill();

        transform.DOMove(targetPos, moveTweenTime).SetEase(Ease.InOutQuad)
        .OnComplete(
            () =>
            {
                transform.DORotate(targetRot, moveTweenTime).SetEase(Ease.InOutQuad);
                anime.SetTrigger("idle");

            }
        );
    }
    private Vector3 DOMoveSupportPosition(Vector3 pos)
    {
        return new Vector3(pos.x, transform.position.y, pos.z);
    }
    private Vector3 DORotateSupportRotation(float rotY)
    {
        return new Vector3(transform.rotation.x, rotY, transform.rotation.z);
    }
    private IEnumerator RandomMove()
    {
        Vector3 pos = transform.position;
        float maxRandomPosOffset = 2f;
        Vector3 randomPos = new Vector3(
        Random.Range(pos.x - maxRandomPosOffset, pos.x + maxRandomPosOffset),
        0f,
        Random.Range(pos.z - maxRandomPosOffset, pos.z + maxRandomPosOffset));

        currentDestination = randomPos;
        agent.SetDestination(randomPos);
        anime.SetTrigger("walk");

        while (true)
        {
            TrashGeneratorInit();
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                anime.SetTrigger("idle");

                yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
                break;
            }
            yield return null;
        }
    }
    private void TrashGeneratorInit()
    {
        TrashGenerator trashGenerator = TrashGenerator.Instance;
        if (!trashGenerator.isWaitingToSpawn)
        {
            trashGenerator.SpawnPos = transform.position;
            trashGenerator.isWaitingToSpawn = true;
        }
    }


}
