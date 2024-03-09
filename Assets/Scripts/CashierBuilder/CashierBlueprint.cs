// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.PlayerLoop;
// using Unity.Mathematics;
// using UnityEngine.Rendering.Universal;
// using DG.Tweening;

// public class CashierBlueprint : MonoBehaviour

// {
//     [SerializeField] private LayerMask placingMask;


//     [HideInInspector] public GameObject prefab;
//     [HideInInspector] public Vector2 boundsSize;
//     [HideInInspector] public Vector3 currentPosition;



//     private GameObject blueprint;
//     private GameObject blueprintCollider;


//     public static CashierBlueprint Instance;
//     [HideInInspector]
//     public GameObject cashierGO;
//     private void Awake()
//     {
//         Instance = this;
//     }
//     private void Start()
//     {
//         CashierBuilder.Instance.Stop += DestroyBlueprint;

//     }
//     public void SpawnBlueprint()
//     {
//         StartCoroutine(RunBlueprint());
//     }
//     private IEnumerator RunBlueprint()
//     {
//         yield return null;
//         if (PlaceButton.Instance.mode == "CashierBuilder")
//         {
//             InstantiateBlueprint();

//         }
//         while (true)
//         {

//             yield return PlacingOperation();


//         }
//     }
//     private void InstantiateBlueprint()
//     {
//         blueprint = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f), transform);
//         blueprintCollider = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f), transform);
//         Destroy(blueprintCollider.GetComponent<MeshRenderer>());
//         Destroy(blueprintCollider.GetComponent<Rigidbody>());
//         Destroy(blueprint.GetComponent<Rigidbody>());
//         Destroy(blueprintCollider.GetComponentInChildren<CashierBuyerStand>().gameObject);



//         foreach (BoxCollider item in blueprintCollider.GetComponents<BoxCollider>())
//         {
//             item.isTrigger = true;
//         }
//         blueprintCollider.AddComponent<CashierBlueprintPrefab>().animatedPrefab = blueprint;
//     }
//     private IEnumerator PlacingOperation()
//     {

//         Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, 1000f, placingMask))
//         {
//             currentPosition = GetPosition(RotateButton.rotateID, hit.point, boundsSize);

//             blueprintCollider.transform.position = currentPosition;
//             blueprint.transform.DOMove(currentPosition + (Vector3.up * .001f), .15f).SetEase(Ease.OutSine);

//         }
//         yield return null;
//     }


//     private Vector3 GetPosition(int rotateID, Vector3 point, Vector2 bounds)
//     {

//         // bounds = bounds - Vector2.one;
//         switch (rotateID)
//         {
//             case 1 or 3:
//                 // ganjil genap
//                 return new Vector3(EvenOddGetPos(bounds.x, point.x), 0, EvenOddGetPos(bounds.y, point.z));

//             default:
//                 return new Vector3(EvenOddGetPos(bounds.y, point.x), 0, EvenOddGetPos(bounds.x, point.z));
//         }
//     }
//     private float EvenOddGetPos(float num, float pos)
//     {
//         if (num % 2 == 0)
//         {
//             return math.ceil(pos - .5f) - 0f;

//         }

//         return math.ceil(pos - 0f) - .5f;


//     }
//     private float getRotate(int id)
//     {
//         return 90 * (id - 1);
//     }
//     public void Rotate()
//     {
//         Debug.Log($"rotate {PlaceButton.Instance.mode}");
//         if (PlaceButton.Instance.mode == "CashierBuilder")
//         {
//             blueprint.transform.DORotate(new Vector3(0, getRotate(RotateButton.rotateID), 0), .5f).SetEase(Ease.OutSine);
//             blueprintCollider.transform.Rotate(new Vector3(0, 90f, 0));
//         }
//     }
//     private void DestroyBlueprint()
//     {
//         StopAllCoroutines();
//         Destroy(blueprint);
//         Destroy(blueprintCollider);
//     }

// }
