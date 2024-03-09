using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SellingPlatformBlueprintPrefab : MonoBehaviour
{
    public GameObject animatedPrefab;
    private MaterialPropertyBlock mtb;
    private MaterialPropertyBlock mtbBuyerStand;
    public LayerMask layerMask;
    private List<int> isEnter;
    private List<int> isExit;

    private void Start()
    {

        mtb = new MaterialPropertyBlock();
        mtbBuyerStand = new MaterialPropertyBlock();
        animatedPrefab.GetComponentInChildren<SellingPlatformBuyerStand>().GetComponent<MeshRenderer>().GetPropertyBlock(mtbBuyerStand);

        isEnter = new List<int>();
        isExit = new List<int>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (layerMask != (layerMask | (1 << other.gameObject.layer)))
        {
            return;
        }

        mtb.SetColor("_BaseColor", Color.red);
        animatedPrefab.GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
        animatedPrefab.GetComponentInChildren<SellingPlatformBuyerStand>().GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
        SellingPlatformBuilder.Instance.isCollide = true;
        isEnter.Add(0);

    }
    private void OnTriggerExit(Collider other)
    {
        if (layerMask != (layerMask | (1 << other.gameObject.layer)))
        {
            return;
        }
        isExit.Add(0);

        if (isEnter.Count == isExit.Count)
        {
            isEnter = new List<int>();
            isExit = new List<int>();
            mtb.SetColor("_BaseColor", Color.white);

            animatedPrefab.GetComponent<MeshRenderer>().SetPropertyBlock(mtb);
            animatedPrefab.GetComponentInChildren<SellingPlatformBuyerStand>().GetComponent<MeshRenderer>().SetPropertyBlock(mtbBuyerStand);

            Debug.Log("exit");
            SellingPlatformBuilder.Instance.isCollide = false;

        }


    }
    private void OnDestroy()
    {
        SellingPlatformBuilder.Instance.isCollide = false;


    }

}
