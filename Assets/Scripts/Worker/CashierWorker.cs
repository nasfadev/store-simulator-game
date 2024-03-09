using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.VisualScripting;
public class CashierWorker : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private Animator anime;
    [SerializeField] private NavMeshAgent agent;
    [Header("Configs")]
    [SerializeField] float moveTweenTime;

    [Header("Previews")]
    public RealtimeDataBuyerSystem realtimeDataBuyerSystem;
    public Workers workers;
    public QuestGenerator questGenerator;
    public int idCashierWorker;
    private float tempSpeed;
    private float _tempAcceleration;
    private CashierManager tempCashierManager;
    private bool isStart;
    private void Start()
    {
        tempSpeed = agent.speed;
        _tempAcceleration = agent.acceleration;

    }
    private void OnEnable()
    {
        if (isStart)
        {
            agent.enabled = true;
            agent.speed = tempSpeed;
            agent.acceleration = _tempAcceleration;
            Run();

            return;
        }
        Run();

        isStart = true;

    }
    private void OnDisable()
    {
        if (tempCashierManager != null)
        {
            tempCashierManager.isWorker = false;
            tempCashierManager.isWorkerStand = false;

        }
    }
    private IEnumerator RunIE()
    {
        while (!BuildNavmesh.navmeshReady)
        {
            Debug.Log("cashier navmesh");

            yield return null;
        }
        bool isCashierFound = false;
        while (true)
        {
            Debug.Log("cashier recheck");

            for (int i = 0; i < realtimeDataBuyerSystem.cashierRD.Count; i++)
            {
                Debug.Log("cashier check kasir");

                CashierManager cashierManager = realtimeDataBuyerSystem.cashierRD[i].cashierManager;
                if (!cashierManager.isWorker)
                {
                    isCashierFound = true;
                    cashierManager.isWorker = true;
                    cashierManager.workers = workers;
                    cashierManager.idCashierWorker = idCashierWorker;
                    cashierManager.questGenerator = questGenerator;
                    tempCashierManager = cashierManager;
                    anime.SetTrigger("walk");
                    agent.SetDestination(cashierManager.workerStand.position);
                    Debug.Log("cashier check Find Kasir");

                    while (true)
                    {
                        Debug.Log("cashier check goto kasir");

                        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                        {
                            Debug.Log("cashier check in kasir");
                            cashierManager.isWorkerStand = true;
                            Vector3 targetPos = DOMoveSupportPosition(cashierManager.workerStand.position);
                            Vector3 targetRot = DORotateSupportRotation(cashierManager.workerStand.eulerAngles.y);
                            DirectionToTarget(targetPos, targetRot);
                            break;
                        }
                        yield return null;
                    }
                    break;
                }

                yield return null;
            }
            if (isCashierFound)
            {
                Debug.Log("cashier check fill kasir");

                break;
            }
            yield return null;

        }
        Debug.Log("cashier check finish");


    }
    private void Run()
    {
        StartCoroutine(RunIE());

    }
    private void DirectionToTarget(Vector3 targetPos, Vector3 targetRot)
    {
        agent.speed = 0;
        agent.acceleration = 0;
        agent.enabled = false;
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
}
