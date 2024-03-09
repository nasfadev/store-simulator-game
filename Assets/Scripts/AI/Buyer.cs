// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;
// using Unity.Mathematics;
// using UnityEngine.AI;
// using System;
// using DG.Tweening;
// using UnityEngine.UIElements;

// public class Buyer : MonoBehaviour
// {
//     // Start is called before the first frame update

//     private int2[] targetShelveses;
//     private int targetChasier;
//     private Vector3 targetExit;
//     private bool targetProses;
//     private int dimension = 30;
//     private bool isExit;
//     [SerializeField] private NavMeshAgent agent;
//     [SerializeField] private Animator anime;





//     // Update is called once per frame
//     private void Start()
//     {
//         StartCoroutine(RunTarget());

//     }

//     private IEnumerator RunTarget()
//     {

//         ContentsBuilder.ContentLibrary[] contentLib = ContentsBuilder.contentLibrary;

//         int contentLibId = UnityEngine.Random.Range(1, contentLib.Length);
//         int contentLengh = contentLib[contentLibId].Content.Count;


//         int ids = contentLib[contentLibId].Content[UnityEngine.Random.Range(0, contentLengh)].x;
//         int maxShelveses = UnityEngine.Random.Range(1, contentLengh);
//         targetShelveses = new int2[2];
//         agent.avoidancePriority = 50;

//         for (int i = 0; i < 2; i++)
//         {
//             targetShelveses[i] = contentLib[contentLibId].Content[UnityEngine.Random.Range(0, contentLengh)];
//             yield return null;
//         }
//         targetChasier = contentLib[0].Content[0].x;
//         targetExit = ContentsBuilder.exitPoint;




//         int shelvesTargetCount = targetShelveses.Length;
//         bool run = false;
//         int next = 0;
//         int SlotId = 0;

//         while (true)
//         {
//             int id = targetShelveses[next].x;

//             if (!run && ContentsBuilder.currentBuyerSlot[id] != ContentsBuilder.maxBuyerSlot[id])
//             {


//                 int rotateID = ContentsBuilder.save.data[id].b;

//                 bool2 cbsa = ContentsBuilder.currentBuyerSlotActive[id];
//                 if (!cbsa.x)
//                 {
//                     SlotId = 0;
//                     ContentsBuilder.currentBuyerSlotActive[id] = new bool2(true, cbsa.y);
//                     agent.SetDestination(GetPosition(rotateID, id, 0));


//                 }
//                 else
//                 {
//                     ContentsBuilder.currentBuyerSlotActive[id] = new bool2(cbsa.x, true);
//                     agent.SetDestination(GetPosition(rotateID, id, 1));

//                     SlotId = 1;
//                 }



//                 ContentsBuilder.currentBuyerSlot[id]++;

//                 anime.SetTrigger("walk1");

//                 run = true;




//             }
//             if (!agent.pathPending && run && agent.remainingDistance <= agent.stoppingDistance)
//             {
//                 int rotateID = ContentsBuilder.save.data[id].b;

//                 run = false;

//                 float currentSpeed = agent.speed;
//                 float currentAngularSpeed = agent.angularSpeed;
//                 float currentAcceleration = agent.acceleration;

//                 agent.speed = 0;
//                 agent.angularSpeed = 0;


//                 transform.DOMove(GetPosition(rotateID, id, SlotId), .5f).SetEase(Ease.InOutQuad).OnComplete(() =>
//                 {
//                     anime.SetTrigger("idle1");
//                     transform.DORotate(new Vector3(0, GetRotate(rotateID), 0), .5f).SetEase(Ease.InOutQuad);
//                 });



//                 yield return new WaitForSeconds(UnityEngine.Random.Range(5, 10));


//                 agent.speed = currentSpeed;
//                 agent.angularSpeed = currentAngularSpeed;
//                 agent.acceleration = currentAcceleration;


//                 bool2 cbsa = ContentsBuilder.currentBuyerSlotActive[id];
//                 if (SlotId == 0)
//                 {
//                     SlotId = 0;
//                     ContentsBuilder.currentBuyerSlotActive[id] = new bool2(false, cbsa.y);

//                 }
//                 else
//                 {
//                     ContentsBuilder.currentBuyerSlotActive[id] = new bool2(cbsa.x, false);

//                     SlotId = 0;
//                 }

//                 ContentsBuilder.currentBuyerSlot[id]--;
//                 next++;







//             }
//             if (next == shelvesTargetCount)
//             {
//                 run = false;


//                 break;
//             }
//             if (!run && ContentsBuilder.currentBuyerSlot[id] == ContentsBuilder.maxBuyerSlot[id])
//             {
//                 bool runBreak = false;
//                 while (true)
//                 {


//                     if (!runBreak)
//                     {

//                         agent.SetDestination(transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 0, UnityEngine.Random.Range(-3, 3)));
//                         anime.SetTrigger("walk1");

//                         runBreak = true;

//                     }
//                     if (runBreak && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//                     {
//                         anime.SetTrigger("idle1");


//                         yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
//                         runBreak = false;
//                     }
//                     if (!runBreak && ContentsBuilder.currentBuyerSlot[id] != ContentsBuilder.maxBuyerSlot[id])
//                     {
//                         break;
//                     }

//                     yield return null;

//                 }
//             }
//             yield return null;

//             // agent.SetDestination(new Vector3(id % 30, 0f, math.floor(id / 30)));
//         }
//         while (true)
//         {
//             int id = targetChasier;


//             if (!run && ContentsBuilder.currentBuyerSlot[id] != ContentsBuilder.maxBuyerSlot[id])
//             {
//                 SlotId = 0;
//                 int rotateID = ContentsBuilder.save.data[id].b;
//                 // for (int i = 0; i < Cashier.slot.Length; i++)
//                 // {

//                 //     if (!Cashier.slot[i] && !Cashier.slot[i + 1 == Cashier.slot.Length ? i : i + 1])
//                 //     {
//                 //         Cashier.slot[ContentsBuilder.currentBuyerSlot[id]] = true;
//                 //         Debug.Log($"slotid : {i}");

//                 //         SlotId = i;
//                 //         agent.SetDestination(GetPositionCashier(rotateID, id, SlotId));
//                 //         break;
//                 //     }
//                 // }
//                 SlotId = ContentsBuilder.currentBuyerSlot[id];

//                 while (true)
//                 {
//                     if (!Cashier.slot[SlotId])
//                     {
//                         Cashier.slot[SlotId] = true;
//                         Debug.Log($"slotnum : {ContentsBuilder.currentBuyerSlot[id]} slotnum : {GetPositionCashier(rotateID, id, SlotId)} ");

//                         agent.SetDestination(GetPositionCashier(rotateID, id, SlotId));

//                         ContentsBuilder.currentBuyerSlot[id]++;

//                         anime.SetTrigger("walk1");

//                         run = true;
//                         break;
//                     }
//                     yield return null;
//                 }





//             }
//             if (!agent.pathPending && run && agent.remainingDistance <= agent.stoppingDistance)
//             {
//                 int rotateID = ContentsBuilder.save.data[id].b;

//                 run = false;

//                 float currentSpeed = agent.speed;
//                 float currentAngularSpeed = agent.angularSpeed;
//                 float currentAcceleration = agent.acceleration;

//                 agent.speed = 0;
//                 agent.angularSpeed = 0;
//                 Sequence sqc1 = DOTween.Sequence();

//                 bool next1 = false;
//                 sqc1.Append(transform.DOMove(GetPositionCashier(rotateID, id, SlotId), .5f).SetEase(Ease.InOutQuad).OnComplete(() => { next1 = true; }));
//                 sqc1.Append(transform.DORotate(new Vector3(0, GetRotate(rotateID), 0), .5f).SetEase(Ease.InOutQuad));
//                 while (true)
//                 {
//                     if (next1)
//                     {
//                         anime.SetTrigger("idle1");
//                         next1 = false;
//                         break;

//                     }
//                     yield return null;
//                 }



//                 if (SlotId != 0)
//                 {
//                     while (true)
//                     {
//                         if (!Cashier.slot[SlotId - 1])
//                         {


//                             Cashier.slot[SlotId] = false;
//                             SlotId--;
//                             Cashier.slot[SlotId] = true;

//                             anime.SetTrigger("walk1");
//                             Sequence sqc = DOTween.Sequence();


//                             sqc.Append(transform.DOMove(GetPositionCashier(rotateID, id, SlotId), .5f).SetEase(Ease.InOutQuad).OnComplete(() => { next1 = true; }));
//                             sqc.Append(transform.DORotate(new Vector3(0, GetRotate(rotateID), 0), .5f).SetEase(Ease.InOutQuad));

//                             while (true)
//                             {
//                                 if (next1)
//                                 {
//                                     anime.SetTrigger("idle1");
//                                     next1 = false;
//                                     break;
//                                 }
//                                 yield return null;
//                             }
//                             if (SlotId == 0)
//                             {

//                                 break;
//                             }

//                         }


//                         yield return null;
//                     }


//                 }

//                 CashierCanvas.shelvesContents.AddRange(targetShelveses);
//                 while (true)
//                 {
//                     if (CashierCanvas.shelvesContents.Count == 0)
//                     {
//                         break;
//                     }
//                     yield return null;
//                 }

//                 agent.avoidancePriority = 50;


//                 agent.speed = currentSpeed;
//                 agent.angularSpeed = currentAngularSpeed;
//                 agent.acceleration = currentAcceleration;


//                 Cashier.slot[SlotId] = false;


//                 ContentsBuilder.currentBuyerSlot[id]--;
//                 isExit = true;
//                 break;

//             }
//             if (!run && ContentsBuilder.currentBuyerSlot[id] == ContentsBuilder.maxBuyerSlot[id])
//             {
//                 bool runBreak = false;
//                 while (true)
//                 {


//                     if (!runBreak)
//                     {


//                         agent.SetDestination(transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 0, UnityEngine.Random.Range(-3, 3)));
//                         anime.SetTrigger("walk1");

//                         runBreak = true;

//                     }
//                     if (runBreak && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//                     {
//                         anime.SetTrigger("idle1");


//                         yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
//                         runBreak = false;
//                     }
//                     if (!runBreak && ContentsBuilder.currentBuyerSlot[id] != ContentsBuilder.maxBuyerSlot[id])
//                     {
//                         break;
//                     }

//                     yield return null;

//                 }
//             }
//             yield return null;

//             // agent.SetDestination(new Vector3(id % 30, 0f, math.floor(id / 30)));
//         }
//         while (true)
//         {
//             if (!run)
//             {

//                 anime.SetTrigger("walk1");
//                 Debug.Log("kelaur");
//                 agent.SetDestination(targetExit);

//                 run = true;



//             }
//             if (!agent.pathPending && run && agent.remainingDistance <= agent.stoppingDistance)
//             {

//                 run = false;
//                 BuyerSpawner.currentBuyerCount--;
//                 Destroy(gameObject);

//                 break;

//             }
//             yield return null;

//         }

//     }

//     private Vector3 GetPosition(int rotateID, int id, float offset)
//     {
//         float pivot = 1.1f;
//         float anchor = .5f;

//         switch (rotateID)
//         {
//             case 1:

//                 return new Vector3((id % dimension) + (offset + anchor), 0f, math.floor(id / dimension) + pivot);

//             case 2:
//                 return new Vector3((id % dimension) + pivot, 0f, math.floor(id / dimension) + (-offset + -anchor + 1));


//             case 3:
//                 return new Vector3((id % dimension) + (-offset + -anchor + 1), 0f, math.floor(id / dimension));



//             default:
//                 return new Vector3((id % dimension), 0f, math.floor(id / dimension) + (offset + anchor));
//         }
//     }
//     private Vector3 GetPositionCashier(int rotateID, int id, float slot)
//     {
//         // slot++;
//         float pivot = 1.1f;
//         float distance = .8f * slot;
//         float anchor = 1f;
//         float offset = 0f;

//         switch (rotateID)
//         {
//             case 1:

//                 return new Vector3((id % dimension) + (offset + anchor), 0f, math.floor(id / dimension) + pivot + distance);

//             case 2:
//                 return new Vector3((id % dimension) + pivot + distance, 0f, math.floor(id / dimension) + (-offset + -anchor + 1));


//             case 3:
//                 return new Vector3((id % dimension) + (-offset + -anchor + 1), 0f, math.floor(id / dimension) - distance);



//             default:
//                 return new Vector3((id % dimension) - distance, 0f, math.floor(id / dimension) + (offset + anchor));
//         }
//     }
//     private float GetRotate(int id)
//     {
//         switch (id)
//         {
//             case 1:
//                 return 180f;

//             case 2:
//                 return 270f;
//             case 3:
//                 return 0f;

//             default:
//                 return 90f;
//         }
//     }

// }
