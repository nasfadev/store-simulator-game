using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBillboard : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main == null)
        {
            return;
        }
        Vector3 targetDirection = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDirection * -1);
    }
}
