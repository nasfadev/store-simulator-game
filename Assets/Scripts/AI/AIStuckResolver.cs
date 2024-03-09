using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIStuckResolver : MonoBehaviour
{
    public event Action stuck;
    // private void OnTriggerStay(Collider other)
    // {

    //     // if (layerMask != (layerMask | (1 << other.gameObject.layer)))
    //     // {
    //     //     return;
    //     // }
    //     stuck?.Invoke();
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer != 3)
        {
            return;
        }
        stuck?.Invoke();
        Debug.Log($"stuck name {other.gameObject.name} layer {other.gameObject.layer}");

    }
}
