using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lean.Localization;
public class Thief : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Animator _anime;
    [Header("Configs")]
    [SerializeField] private float runSpeed;

    [Header("Previews")]
    public Vector3 ExitPosition;
    public StoreData StoreData;
    public SellingPlatformProductData SellingPlatformProductData;
    public RealtimeDataBuyerSystem RealtimeDataBuyerSystem;
    public StockInventoryData StockInventoryData;
    public EconomyNotif EconomyNotif;
    public Thiefs Thiefs;
    //HIden
    public int StockQuantityToThief;
    private Status _status = Status.GotoTarget;
    private bool _isRobbed;
    private int _tempSpId;
    private enum Status
    {
        GotoTarget,
        GotoExit,
        Caught
    }

    private void OnEnable()
    {
        Run();
    }
    private void Run()
    {
        StartCoroutine(RunIE());
    }
    private IEnumerator RunIE()
    {
        while (!BuildNavmesh.navmeshReady)
        {
            yield return null;
        }
        List<SellingPlatformRealtimeData> spDatas = RealtimeDataBuyerSystem.SellingPlatformsRD;


        while (true)
        {
            if (_status == Status.GotoTarget)
            {
                SellingPlatformRealtimeData spData = spDatas[Random.Range(0, spDatas.Count)];
                _anime.SetTrigger("walk");
                _agent.SetDestination(spData.buyerSlotTranfrom[0].position);
                yield return GotoTarget(spData);
            }
            else if (_status == Status.GotoExit)
            {
                _anime.SetTrigger("walk");
                _agent.SetDestination(ExitPosition);
                yield return GotoExit();
            }
            else if (_status == Status.Caught)
            {
                _anime.SetTrigger("run");
                _agent.SetDestination(ExitPosition);
                _agent.speed = runSpeed;
                yield return GotoExitWhenCaught();
            }
            yield return null;
        }
    }
    private IEnumerator GotoTarget(SellingPlatformRealtimeData spData)
    {
        while (true)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _anime.SetTrigger("idle");
                float time = 0f;
                bool isCaught = false;
                while (time < 3f)
                {
                    time += Time.deltaTime;
                    if (_status == Status.Caught)
                    {
                        isCaught = true;
                        break;
                    }
                    yield return null;
                }
                if (isCaught)
                {
                    break;
                }
                int stockQuantity = SellingPlatformBuilder.SellingPlatformDataSave.instance.spData.datas[spData.index].stockQuantity;

                StockQuantityToThief = Mathf.Clamp(StockQuantityToThief, 0, stockQuantity);
                spData.stockManager.DecreaseStock(StockQuantityToThief);
                _tempSpId = spData.IDsp - 1;
                _status = Status.GotoExit;
                _isRobbed = true;
                EconomyNotif.Append("RedAlert", LeanLocalization.GetTranslationText("Thief_Alert"));
                break;
            }
            if (_status == Status.Caught)
            {
                break;
            }

            yield return null;
        }

    }
    private IEnumerator GotoExit()
    {
        while (true)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _anime.SetTrigger("idle");
                Destory();
                break;
            }
            if (_status == Status.Caught)
            {
                break;
            }
            yield return null;
        }
    }
    private IEnumerator GotoExitWhenCaught()
    {
        while (true)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _anime.SetTrigger("idle");
                Destory();
                break;
            }

            yield return null;
        }
    }
    private void Destory()
    {
        Thiefs.SpawnCount--;
        Destroy(gameObject);

    }


    public void Catch()
    {
        _status = Status.Caught;
        RestoreStock();
    }
    private void RestoreStock()
    {
        if (!_isRobbed)
        {
            return;
        }
        SellingPlatformProductData.Data spProductData = SellingPlatformProductData.data[_tempSpId];
        StoreData.data.stockQuantity[_tempSpId] += StockQuantityToThief;
        StockInventoryData.stockDisplayData[_tempSpId] += StockQuantityToThief;
        EconomyNotif.Append(spProductData.imageSprite,
        LeanLocalization.GetTranslationText(spProductData.titleTranslationName), StockQuantityToThief, true);

    }

}
