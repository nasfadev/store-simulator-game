using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorRemover : MonoBehaviour, IPointerDownHandler
{
    public static bool isDelete;


    public static int floorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        isDelete = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        isDelete = true;
        // Debug.Log("hit");

    }
}
