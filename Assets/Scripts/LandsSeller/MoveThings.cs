using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThings : MonoBehaviour
{
    public void MoveUp(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
