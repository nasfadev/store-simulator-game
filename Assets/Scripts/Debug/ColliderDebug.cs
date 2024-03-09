using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        ContactPoint[] contacts = new ContactPoint[1];
        Debug.Log(other.contactOffset);
    }
    private void OnCollisionStay(Collision other)
    {
        Debug.Log("enterr");

    }


}
