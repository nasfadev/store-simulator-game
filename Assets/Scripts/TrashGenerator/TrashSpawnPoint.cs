using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnPoint : MonoBehaviour
{
    public bool isSpawnAble;
    private void OnTriggerEnter(Collider other)
    {
        isSpawnAble = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isSpawnAble = false;
    }
}
