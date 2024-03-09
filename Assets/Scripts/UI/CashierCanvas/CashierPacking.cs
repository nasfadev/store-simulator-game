using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class CashierPacking : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] prefabs;
    private void Start()
    {
        StartCoroutine(RunPacking());
    }

    private IEnumerator RunPacking()
    {
        while (true)
        {

            if (CashierCanvas.shelvesContents.Count != 0)
            {
                for (int i = 0; i < CashierCanvas.shelvesContents.Count; i++)
                {
                    GameObject prefab = Instantiate(prefabs[CashierCanvas.shelvesContents[i].y], Vector3.zero, quaternion.identity, transform);



                }
            }
            yield return null;
        }
    }
}