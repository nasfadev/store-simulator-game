using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorPlacer : MonoBehaviour, IPointerDownHandler
{
    public static bool isPlace;
    public int FloorMaterial;

    public static int floorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        isPlace = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPlace = true;
        floorMaterial = int.Parse(data.pointerCurrentRaycast.gameObject.name);

        Debug.Log($"flooor : {floorMaterial}");

    }
}
