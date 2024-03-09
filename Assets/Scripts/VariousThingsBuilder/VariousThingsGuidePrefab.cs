using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousThingsGuidePrefab : MonoBehaviour
{
    public bool isCantPlace;
    public int enter;
    public int exit;

    private MaterialPropertyBlock materialProperty;
    private MaterialPropertyBlock tempMaterialProperty;
    private MaterialPropertyBlock tempBuyerStandMaterialProperty;

    private MeshRenderer meshRenderer;
    private MeshRenderer buyerStandMeshRenderer;
    private VariousThingsBuyerStand buyerStand;
    private void Start()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<BoxCollider>().isTrigger = true;
        materialProperty = new MaterialPropertyBlock();
        tempMaterialProperty = new MaterialPropertyBlock();
        tempBuyerStandMaterialProperty = new MaterialPropertyBlock();
        // transform.localScale = new Vector3(1.0005f, 1f, 1f);
        materialProperty.SetColor("_BaseColor", Color.red);

        meshRenderer = GetComponent<MeshRenderer>();
        buyerStand = GetComponentInChildren<VariousThingsBuyerStand>();

        if (buyerStand != null)
        {
            buyerStandMeshRenderer = GetComponentInChildren<VariousThingsBuyerStand>().GetComponent<MeshRenderer>();
        }
        if (buyerStandMeshRenderer != null)
        {
            buyerStandMeshRenderer.GetPropertyBlock(tempBuyerStandMaterialProperty);
        }
        meshRenderer.GetPropertyBlock(tempMaterialProperty);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("various info name " + other.gameObject.name + " layer " + other.gameObject.layer);
        enter++;
        if (!isCantPlace)
        {
            if (buyerStandMeshRenderer != null)
            {
                buyerStandMeshRenderer.SetPropertyBlock(materialProperty);
            }
            meshRenderer.SetPropertyBlock(materialProperty);
            isCantPlace = true;
            Debug.Log("doorwall cant Place");


        }
    }
    private void OnTriggerExit(Collider other)
    {
        exit++;
        if (enter == exit)
        {
            enter = 0;
            exit = 0;
            if (buyerStandMeshRenderer != null)
            {
                buyerStandMeshRenderer.SetPropertyBlock(tempBuyerStandMaterialProperty);
            }
            meshRenderer.SetPropertyBlock(tempMaterialProperty);
            isCantPlace = false;
            Debug.Log("doorwall Place");

        }
    }
}
