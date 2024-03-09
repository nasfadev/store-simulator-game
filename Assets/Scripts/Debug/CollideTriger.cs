using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideTriger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("debug enter");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("debug exit");

    }
}
