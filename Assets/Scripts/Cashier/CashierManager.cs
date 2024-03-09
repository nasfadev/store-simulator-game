using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CashierManager : MonoBehaviour
{
    [Header("Requires")]

    [SerializeField] public CashierStand cashierStand;
    public VariousThings variousThings;
    [SerializeField] private CashierProductPacking cashierProductPacking;
    [SerializeField] private GameObject askariCoins;
    public Transform workerStand;
    [Header("Configs")]
    [SerializeField] private UnityEvent whenCashierWorkerServe;
    [Header("Preview")]
    public int idCashierWorker;
    public bool isWorker;
    public bool isWorkerStand;
    public Workers workers;
    public QuestGenerator questGenerator;
    public List<BuyerAI.SPAIData> sPAIDatas;
    [HideInInspector] public bool isStillServe;
    private void Start()
    {
        BuyerSpawner.Instance.whenDisable += Reset;
        VariousThingsBuilder.Instance.OnBeforeRender += DisableAskariCoins;
        VariousThingsBuilder.Instance.OnAfterRender += EnableAskariCoins;
        EnableAskariCoins();

    }
    private void OnDisable()
    {
        BuyerSpawner.Instance.whenDisable -= Reset;
        VariousThingsBuilder.Instance.OnBeforeRender -= DisableAskariCoins;
        VariousThingsBuilder.Instance.OnAfterRender -= EnableAskariCoins;

    }
    public void DisableAskariCoins()
    {
        askariCoins.SetActive(false);
    }
    public void EnableAskariCoins()
    {
        if (VariousThingsBuilder.VariousThingsDataSave.instance.cashierAskari[variousThings.index] > 0)
        {
            askariCoins.SetActive(true);

        }
    }
    public void Run()
    {
        StartCoroutine(RunIE());
    }
    public void MenuEnabler()
    {
        if (isWorker)
        {
            return;
        }
        CashierGamePlayCanvas.Instance.menuCanvas.Appear();

    }
    public void Reset()
    {
        CGMPackingController controller = CGMPackingController.Instance;

        StopAllCoroutines();
        cashierProductPacking.DisappearToteBag(.4f);
        sPAIDatas = null;

        if (isWorker)
        {
            return;
        }
        Destroy(controller.prefab);
        CashierGamePlayCanvas.Instance.canvasSimpleTweenFade.Disappear();

        controller.Stop();

    }
    private IEnumerator RunIE()
    {
        CGMPackingController controller = CGMPackingController.Instance;
        CGMPacking cashier = CGMPacking.Instance;
        cashier.cashierManager = this;

        while (isStillServe)
        {
            if (!(Player.Instance.mode == Player.Mode.Free || Player.Instance.mode == Player.Mode.Cashier) && !isWorker)
            {
                yield return null;
                continue;
            }
            if (cashierStand.isStand && !(sPAIDatas == null || sPAIDatas.Count == 0) && !isWorker)
            {
                Debug.Log("casx stand");
                CashierGamePlayCanvas.Instance.canvasSimpleTweenFade.Appear();
                controller.SPAIData = sPAIDatas;
                if (sPAIDatas[0].stockQuantityToBuy == 0)
                {
                    sPAIDatas.RemoveAt(0);
                }
                cashierProductPacking.AppearToteBag(.4f);

                controller.prefab = cashierProductPacking.PlayerModeInstancePrefab(sPAIDatas[0].IDsp);
                controller.Run();
                cashier.Reset();
                cashier.spAIDatas = sPAIDatas;
                cashier.isPacking = true;
                cashier.Run();
                int tempStockToBuy = sPAIDatas[0].stockQuantityToBuy;
                int tempCount = sPAIDatas.Count;
                bool isNext = false;

                while (true)
                {
                    if (sPAIDatas.Count > 0 && tempCount != sPAIDatas.Count)
                    {

                        tempCount = sPAIDatas.Count;
                        isNext = true;

                    }
                    else if (sPAIDatas.Count > 0 && tempStockToBuy != sPAIDatas[0].stockQuantityToBuy && sPAIDatas[0].stockQuantityToBuy > 0)
                    {

                        tempStockToBuy = sPAIDatas[0].stockQuantityToBuy;
                        isNext = true;

                    }

                    if (isNext)
                    {
                        Destroy(controller.prefab);
                        controller.prefab = cashierProductPacking.PlayerModeInstancePrefab(sPAIDatas[0].IDsp);
                        isNext = false;

                    }
                    if (!cashier.isPacking)
                    {
                        Destroy(controller.prefab);
                        cashierProductPacking.DisappearToteBag(.4f);

                        isStillServe = false;
                        CashierGamePlayCanvas.Instance.canvasSimpleTweenFade.Disappear();
                        MenuEnabler();
                        controller.Stop();
                        break;
                    }
                    if (!cashierStand.isStand)
                    {
                        Destroy(controller.prefab);

                        CashierGamePlayCanvas.Instance.canvasSimpleTweenFade.Disappear();
                        MenuEnabler();
                        controller.Stop();

                        break;
                    }

                    if (isWorker)
                    {
                        Destroy(controller.prefab);

                        CashierGamePlayCanvas.Instance.canvasSimpleTweenFade.Disappear();
                        MenuEnabler();
                        controller.Stop();

                        break;
                    }

                    yield return null;
                }



            }
            else if (isWorker && isWorkerStand)
            {
                float speed = workers.cashierSefaultSpeed - workers.storeData.data.workerData.cashiers[idCashierWorker].level / 10;
                cashierProductPacking.AppearToteBag(speed / 4);

                while (isWorker)
                {
                    if (sPAIDatas == null || sPAIDatas.Count <= 0)
                    {
                        cashierProductPacking.DisappearToteBag(speed / 4);
                        isStillServe = false;
                        break;
                    }
                    else if (sPAIDatas[0].stockQuantityToBuy > 0)
                    {
                        cashierProductPacking.InstanceThePrefab(sPAIDatas[0].IDsp, speed);
                        float fullSpeed = speed + speed / 2;
                        yield return new WaitForSeconds(fullSpeed / 2);
                        whenCashierWorkerServe?.Invoke();
                        yield return new WaitForSeconds(fullSpeed / 2);
                        int IDsp = sPAIDatas[0].IDsp;
                        SellingPlatformProductData.Data dataSp = SellingPlatformProductData.Instance.data[IDsp - 1];

                        int sellProductPrice = dataSp.sellProductPrice * workers.storeData.addLevelMultiplier[dataSp.level];

                        sPAIDatas[0].stockQuantityToBuy--;
                        VariousThingsBuilder.VariousThingsDataSave.instance.cashierAskari[variousThings.index] += sellProductPrice;
                        EnableAskariCoins();
                        questGenerator.AddQuestCount(IDsp - 1, StoreData.QuestType.SellStock);
                        StoreData.StoreDataSave.instance.data.stockQuantity[IDsp - 1]--;

                    }
                    else if (sPAIDatas[0].stockQuantityToBuy <= 0)
                    {
                        sPAIDatas.RemoveAt(0);
                    }
                    yield return null;

                }

            }

            yield return null;
        }

    }
}
