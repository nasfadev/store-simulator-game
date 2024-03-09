using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsGuidePrefab : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // for (int i = 0; i < GetComponents<MeshCollider>().Length; i++)
        // {
        //     GetComponents<MeshCollider>()[i].isTrigger = true;



        // }
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnTriggerStay(Collider other)
    {

    }


}
