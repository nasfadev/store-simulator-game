
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;

// public class CashierBlueprintPrefab : MonoBehaviour
// {
//     public GameObject animatedPrefab;
//     private MaterialPropertyBlock mtb;
//     private MaterialPropertyBlock mtbBuyerStand;

//     private List<int> isEnter;
//     private List<int> isExit;

//     private void Start()
//     {
//         mtb = new MaterialPropertyBlock();
//         mtbBuyerStand = new MaterialPropertyBlock();
//         animatedPrefab.GetComponentInChildren<CashierBuyerStand>().GetComponent<MeshRenderer>().GetPropertyBlock(mtbBuyerStand);


//         isEnter = new List<int>();
//         isExit = new List<int>();

//     }


//     private void OnTriggerEnter(Collider other)
//     {
//         mtb.SetColor("_BaseColor", Color.red);
//         animatedPrefab.GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
//         animatedPrefab.GetComponentInChildren<CashierBuyerStand>().GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
//         CashierBuilder.Instance.isCollide = true;
//         isEnter.Add(0);
//     }
//     private void OnTriggerExit(Collider other)
//     {
//         isExit.Add(0);

//         if (isEnter.Count == isExit.Count)
//         {
//             isEnter = new List<int>();
//             isExit = new List<int>();
//             mtb.SetColor("_BaseColor", Color.white);

//             animatedPrefab.GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
//             animatedPrefab.GetComponentInChildren<CashierBuyerStand>().GetComponent<MeshRenderer>().SetPropertyBlock(mtbBuyerStand);

//             Debug.Log("exit");
//             CashierBuilder.Instance.isCollide = false;

//         }


//     }
//     private void OnDestroy()
//     {
//         CashierBuilder.Instance.isCollide = false;

//     }

// }
