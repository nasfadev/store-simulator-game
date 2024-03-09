using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAndBuilderGuidePrefab : MonoBehaviour
{
    public bool isCantPlace;
    private int status;
    private MaterialPropertyBlock materialProperty;
    private MaterialPropertyBlock tempMaterialProperty;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<BoxCollider>().isTrigger = true;
        materialProperty = new MaterialPropertyBlock();
        tempMaterialProperty = new MaterialPropertyBlock();
        transform.localScale = new Vector3(1.0005f, 1f, 1f);
        materialProperty.SetColor("_BaseColor", Color.red);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.GetPropertyBlock(tempMaterialProperty);
    }
    private void OnTriggerEnter(Collider other)
    {
        status++;
        if (!isCantPlace)
        {
            meshRenderer.SetPropertyBlock(materialProperty);
            isCantPlace = true;
            Debug.Log("doorwall cant Place");


        }
    }
    private void OnTriggerExit(Collider other)
    {

        status--;
        if (status == 0)
        {
            meshRenderer.SetPropertyBlock(tempMaterialProperty);
            isCantPlace = false;
            Debug.Log("doorwall Place");

        }
    }
}
