
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class CashierCanvas : MonoBehaviour
{
    public static List<int2> shelvesContents = new List<int2>();

    public void DecreaseShelvesContents()
    {
        Debug.Log("kurang");
        shelvesContents.RemoveAt(shelvesContents.Count - 1);
    }
}

