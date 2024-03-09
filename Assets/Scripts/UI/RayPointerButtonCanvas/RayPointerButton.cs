using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using Lean.Localization;
using UnityEngine.Events;
public class RayPointerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Required Things")]
    [SerializeField] private LayerMask placingMask;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image fill;
    [SerializeField] private LeanLocalizedTextMeshProUGUI translation;
    [SerializeField] private LandsSeller landsSeller;
    [SerializeField] private AreaLocker areaLocker;
    [SerializeField] private Workers _workers;
    [SerializeField] private EconomyNotif _economyNotif;
    [Header("Configs")]
    [SerializeField] private float fillDifficulty;
    [SerializeField] private float decreaseFillDifficulty;

    [SerializeField] private float fadeTweenTime;
    [SerializeField] private float rayDistance;
    [Header("Translation Name")]

    [SerializeField] private string WareHouseShelvesTranslationName;

    [SerializeField] private string sellingPlatformTranslationName;
    [SerializeField] private string grabAbleTrashTranslationName;
    [SerializeField] private string toolAbleTrashTranslationName;
    [SerializeField] private string cashierAskariTranslationName;
    [SerializeField] private string NPCTranslationName;
    [SerializeField] private string landSellSignTranslationName;
    [SerializeField] private string areaLockerTranslationName;
    [SerializeField] private string workerTranslationName;
    [SerializeField] private string thiefTranslationName;


    [Header("General Event")]

    [SerializeField] private UnityEvent OnDown;
    [Header("WareHouseShelves Event")]

    [SerializeField] private UnityEvent WareHouseShelvesHolded;

    [SerializeField] private UnityEvent WareHouseShelvesException;
    [Header("SellingPlatform Event")]

    [SerializeField] private UnityEvent sellingPlatformEventException;
    [SerializeField] private UnityEvent sellingPlatformEventHolded;
    [Header("Grab Able Trash Event")]

    [SerializeField] private UnityEvent grabAbleTrashEventException;
    [SerializeField] private UnityEvent grabAbleTrashEvent;
    [Header("Tool Able Trash Event")]

    [SerializeField] private UnityEvent toolAbleTrashEventException;
    [SerializeField] private UnityEvent toolAbleTrashEvent;
    [Header("Cashier Askari Event")]

    [SerializeField] private UnityEvent cashierAskariEventException;
    [SerializeField] private UnityEvent cashierAskariEvent;
    [Header("Land Sell SIgn Event")]

    [SerializeField] private UnityEvent landSellSignEventException;
    [SerializeField] private UnityEvent landSellSignEvent;
    [Header("Area Locker Event")]

    [SerializeField] private UnityEvent areaLockerEventException;
    [SerializeField] private UnityEvent areaLockerEvent;
    [Header("Area Locker Event")]

    [SerializeField] private UnityEvent workerEventException;
    [SerializeField] private UnityEvent workerEvent;

    private Player.Mode tempPlayerMode;

    private bool isHold;
    private bool isHolded;
    private bool isAppear;


    private void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(RunIE());
    }
    public void Run()
    {
        StopAllCoroutines();

        StartCoroutine(RunIE());

    }
    public void Stop()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0f;
        canvas.enabled = false;
        isAppear = false;


    }
    private IEnumerator RunIE()
    {
        while (true)
        {
            if (Player.Instance.mode == Player.Mode.Builder)
            {
                yield return null;
                continue;
            }

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                tempPlayerMode = Player.Instance.mode;
                RayPointerAble rayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble == null)
                {
                    yield return null;
                    continue;
                }
                yield return Checktype(hit, rayPointerAble);

            }

            yield return null;
        }
    }
    private IEnumerator Checktype(RaycastHit hit, RayPointerAble rayPointerAble)
    {

        if (rayPointerAble.mode == RayPointerAble.Type.WareHouseShelves && Player.Instance.mode == Player.Mode.Free)
        {
            isAppear = true;
            yield return WhenWareHouseShelvesType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.SellingPlatform && Player.Instance.mode == Player.Mode.PickCardBoard)
        {
            isAppear = true;

            yield return WhenSellingPlatformType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.GrabAbleTrash)
        {
            isAppear = true;

            yield return WhenGrabAbleTrashType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.ToolAbleTrash)
        {
            isAppear = true;

            yield return WhenToolAbleTrashType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.CashierAskari)
        {
            isAppear = true;

            yield return WhenCashierAskariType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.NPC && Player.Instance.mode == Player.Mode.Free)
        {
            isAppear = true;

            yield return WhenNPCType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.LandSellSign)
        {
            isAppear = true;

            yield return WhenLandSellSignType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.AreaLocker)
        {
            isAppear = true;

            yield return WhenAreaLockerType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.Worker)
        {
            isAppear = true;

            yield return WhenWorkerType(hit, rayPointerAble);

        }
        else if (rayPointerAble.mode == RayPointerAble.Type.Thief)
        {
            isAppear = true;

            yield return WhenThiefType(hit, rayPointerAble);

        }
        isAppear = false;


    }


    private IEnumerator WhenWareHouseShelvesType(RaycastHit hit, RayPointerAble rayPointerAble)
    {

        translation.TranslationName = WareHouseShelvesTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenWareHouseShelvesTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }

    }
    private void WhenWareHouseShelvesTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            isHolded = true;
            WareHouseShelvesHolded?.Invoke();
            Player.Instance.mode = Player.Mode.PickCardBoard;

        }
    }


    private IEnumerator WhenSellingPlatformType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = sellingPlatformTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenSellingPlatformTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }

            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenSellingPlatformTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            isHolded = true;
            SellingPlatform sellingPlatform = hit.transform.GetComponent<SellingPlatform>();
            int currentStockCount = StockInventoryData.Instance.stockDisplayData[sellingPlatform.IDsp - 1];
            ;
            if (currentStockCount <= 0)
            {
                StockShop.Instance.Run(sellingPlatform.IDsp);
                sellingPlatformEventException?.Invoke();
            }
            else
            {
                sellingPlatform.FillStock();
                sellingPlatformEventHolded?.Invoke();

            }


        }
    }

    private IEnumerator WhenGrabAbleTrashType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = grabAbleTrashTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenGrabAbleTrashTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenGrabAbleTrashTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            isHolded = true;
            grabAbleTrashEvent?.Invoke();
            Trash trash = hit.transform.GetComponent<Trash>();
            trash.Delete();
        }
    }
    private IEnumerator WhenToolAbleTrashType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = toolAbleTrashTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenToolAbleTrashTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenToolAbleTrashTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            toolAbleTrashEvent?.Invoke();
            isHolded = true;
            Trash trash = hit.transform.GetComponent<Trash>();
            trash.Delete();
        }
    }
    private IEnumerator WhenCashierAskariType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = cashierAskariTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenCashierAskariTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenCashierAskariTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            cashierAskariEvent?.Invoke();
            isHolded = true;
            AskariCoins askariCoins = hit.transform.GetComponent<AskariCoins>();
            int askari = VariousThingsBuilder.VariousThingsDataSave.instance.cashierAskari[askariCoins.cashierManager.variousThings.index];
            EconomyCurrency.Instance.AddAskari(askari);
            VariousThingsBuilder.VariousThingsDataSave.instance.cashierAskari[askariCoins.cashierManager.variousThings.index] = 0;
            askariCoins.cashierManager.DisableAskariCoins();

            EconomyNotif.Instance.Append("askari", "Askari", askari, true);
        }

    }
    private IEnumerator WhenNPCType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = NPCTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenNPCTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenNPCTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {

            isHolded = true;
            hit.transform.GetComponent<NPCs>().getMenu?.Invoke();
            StopAllCoroutines();
            canvasGroup.DOFade(0f, fadeTweenTime)
.OnComplete(() => { canvas.enabled = false; });
        }

    }
    private IEnumerator WhenLandSellSignType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = landSellSignTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenLandSellSignTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenLandSellSignTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {

            isHolded = true;
            landsSeller.bookLandId = hit.transform.GetComponent<Lands>().id;
            landSellSignEvent?.Invoke();
        }

    }
    private IEnumerator WhenAreaLockerType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = areaLockerTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenAreaLockerTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenAreaLockerTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {

            isHolded = true;
            areaLocker.BookAreaLockerId = hit.transform.GetComponent<Locker>().id;
            areaLockerEvent?.Invoke();
        }

    }
    private IEnumerator WhenWorkerType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = workerTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenWorkerTypeHolding(hit);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenWorkerTypeHolding(RaycastHit hit)
    {
        fill.fillAmount += 1f;
        if (fill.fillAmount == 1f)
        {
            isHolded = true;

            CashierWorker cashierWorker = hit.transform.GetComponent<CashierWorker>();
            if (cashierWorker != null)
            {
                int levelNow = _workers.storeData.data.workerData.cashiers[cashierWorker.idCashierWorker].level;
                int maxLevel = _workers.maxLevel;
                if (levelNow >= maxLevel - 1)
                {
                    _economyNotif.Append("YellowAlert", LeanLocalization.GetTranslationText("Worker_MaxLevelSimpleUI"));
                    return;
                }
                _workers.typeNowSimpleUi = Workers.Type.Cashier;
                _workers.bookIdWorker = cashierWorker.idCashierWorker;
                _workers.isSimpleUI = true;
                _workers.UpdateUpgradeUI();
            }
            workerEvent?.Invoke();
        }

    }
    private IEnumerator WhenThiefType(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        translation.TranslationName = thiefTranslationName;
        fill.fillAmount = 0f;
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
        while (true)
        {
            if (isHold && !isHolded)
            {
                WhenThiefTypeHolding(hit, rayPointerAble);
            }
            else
            {
                fill.fillAmount -= decreaseFillDifficulty * Time.deltaTime;

            }
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, rayDistance, placingMask))
            {
                RayPointerAble tempRayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                if (rayPointerAble != tempRayPointerAble || rayPointerAble.mode == RayPointerAble.Type.Ignore)
                {
                    canvasGroup.DOFade(0f, fadeTweenTime)
                    .OnComplete(() => { canvas.enabled = false; });
                    break;
                }


            }
            else
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }
            yield return null;

            if (tempPlayerMode != Player.Instance.mode)
            {
                canvasGroup.DOFade(0f, fadeTweenTime)
                .OnComplete(() => { canvas.enabled = false; });
                break;
            }


            yield return null;
        }
    }
    private void WhenThiefTypeHolding(RaycastHit hit, RayPointerAble rayPointerAble)
    {
        fill.fillAmount += .1f;
        if (fill.fillAmount == 1f)
        {
            isHolded = true;

            Thief thief = hit.transform.GetComponent<Thief>();
            thief.Catch();
            rayPointerAble.mode = RayPointerAble.Type.Ignore;
        }

    }
    public void OnPointerDown(PointerEventData data)
    {
#if UNITY_EDITOR
        return;
#elif UNITY_STANDALONE_WIN
            return;

#elif UNITY_ANDROID
        OnDown?.Invoke();
        isHold = true;
#endif

    }
    public void OnPointerUp(PointerEventData data)
    {
#if UNITY_EDITOR
        return;

#elif UNITY_STANDALONE_WIN
            return;

#elif UNITY_ANDROID
      isHolded = false;
        isHold = false;
#endif

    }
    private void Update()
    {
#if UNITY_EDITOR
        OnPointerDownForWindows();

#elif UNITY_STANDALONE_WIN
                    OnPointerDownForWindows();


#elif UNITY_ANDROID
      return;
#endif

    }
    private void OnPointerDownForWindows()
    {

        if (Input.GetMouseButtonDown(0) && isAppear)
        {
            OnDown?.Invoke();
            isHold = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHolded = false;
            isHold = false;
        }
    }
}
