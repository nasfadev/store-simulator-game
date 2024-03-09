// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using UnityEngine.Events;
// using ThirtySec;
// public class CashierBuilder : MonoBehaviour
// {
//     [SerializeField] private GameObject[] CashierPrefab;

//     [SerializeField] private int[] CashierLevel;
//     [SerializeField] private LayerMask cashierStandLayerMask;

//     [SerializeField] private Vector2[] CashierEvenOddSize;
//     [SerializeField] private UnityEvent OnDisable;
//     [SerializeField] private UnityEvent OnEnterCashier;
//     [SerializeField] private UnityEvent OnExitCashier;
//     private bool Loaded;
//     public static CashierBuilder Instance;
//     public event Action Run;
//     public event Action Stop;
//     [HideInInspector]
//     public bool isCollide;

//     private void Awake()
//     {
//         Instance = this;
//     }
//     private void Start()
//     {
//         if (CashierDataSave.IsExist())
//         {
//             LoadData();
//         }
//         else
//         {
//             CashierDataSave.instance.data = new CashierData();
//             CashierDataSave.instance.Save<CashierDataSave>();
//             if (!Loaded)
//             {
//                 Loaded = true;
//                 StartCoroutine(CheckStateLoadedRequirement());

//             }


//         }

//     }
//     private IEnumerator CheckStateLoadedRequirement()
//     {
//         int numThisState = 5;
//         while (!(StateLoaded.Loaded + 1 == numThisState))
//         {
//             yield return null;
//         }
//         StateLoaded.Loaded++;

//     }
//     public class CashierDataSave : ThirtySec.Serializable<CashierDataSave>
//     {
//         public CashierData data;

//     }
//     public void RunCashierBuilder()
//     {
//         Run?.Invoke();
//         gameObject.SetActive(true);
//         SetBlueprint(CashierDataSave.instance.data.prefabID);
//         StartCoroutine(EnableCashierBuilder());

//     }
//     private IEnumerator EnableCashierBuilder()
//     {
//         while (true)
//         {

//             yield return AddingOperation();

//         }
//     }
//     private IEnumerator AddingOperation()
//     {
//         PlaceButton place = PlaceButton.Instance;

//         if (place.isExecute && place.mode == "CashierBuilder")
//         {

//             Debug.Log("hit it");

//             if (isCollide)
//             {
//                 PlaceButton.Instance.isExecute = false;
//                 PlaceButton.Instance.isRun = false;
//                 Debug.Log("collide");

//             }
//             else
//             {
//                 CashierBlueprint blueprint = CashierBlueprint.Instance;

//                 CashierDataSave.instance.data.position = new CashierData.SerializableVector3(blueprint.currentPosition);
//                 CashierDataSave.instance.data.rotation = getRotate(RotateButton.rotateID);

//                 InstantiateCashier(CashierDataSave.instance.data);

//                 Debug.Log($"location {blueprint.currentPosition}");

//                 PlaceButton.Instance.isExecute = false;
//                 PlaceButton.Instance.isRun = false;
//                 DisableCashierBuilder();
//                 AutoMove.Instance.isMoveTouched = false;

//             }

//         }
//         yield return null;
//     }

//     private void SetBlueprint(int contentID)
//     {


//         CashierBlueprint.Instance.boundsSize = CashierEvenOddSize[contentID];
//         CashierBlueprint.Instance.prefab = CashierPrefab[contentID];


//     }
//     private void LoadData()
//     {
//         InstantiateCashier(CashierDataSave.instance.data);
//         Stop?.Invoke();
//         if (!Loaded)
//         {
//             Loaded = true;
//             StartCoroutine(CheckStateLoadedRequirement());

//         }
//     }
//     private void InstantiateCashier(CashierData data)
//     {
//         AutoMoveAble autoMoveAble = CashierPrefab[data.prefabID].GetComponent<AutoMoveAble>();
//         autoMoveAble.mode = "CashierBuilder";
//         GameObject prefab = Instantiate(
//                     CashierPrefab[data.prefabID],
//                     data.position.ToVector3(),
//                     Quaternion.Euler(0f, data.rotation, 0f),
//                     transform);
//         // RealtimeDataBuyerSystem.Instance.CashierInit(prefab);
//         CashierStand cashierStand = prefab.GetComponentInChildren<CashierStand>(true);
//         cashierStand.OnEnter = OnEnterCashier;
//         cashierStand.OnExit = OnExitCashier;
//         cashierStand.gameObject.SetActive(true);
//         cashierStand.layerMask = cashierStandLayerMask;

//     }
//     private void DisableCashierBuilder()
//     {
//         OnDisable?.Invoke();
//         Stop?.Invoke();
//         StopAllCoroutines();
//     }

//     private float getRotate(int id)
//     {
//         return 90 * (id - 1);
//     }

// }
