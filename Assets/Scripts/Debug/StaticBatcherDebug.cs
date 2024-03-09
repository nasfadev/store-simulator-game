using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBatcherDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pre;
    [SerializeField] private GameObject pre2;


    [SerializeField] private int count;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        List<GameObject> pres = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            Instantiate(pre, Random.insideUnitSphere, Quaternion.identity, transform);
            yield return null;

            // Instantiate(pre2, Random.insideUnitSphere, Quaternion.identity, transform);

            // StaticBatchingUtility.Combine(pres.ToArray(), gameObject);

            // yield return null;

        }
        GetComponent<MeshCombiner>().CombineMeshes(false);
        yield return null;

    }


}
