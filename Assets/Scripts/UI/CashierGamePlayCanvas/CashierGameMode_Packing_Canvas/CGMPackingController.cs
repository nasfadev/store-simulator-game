using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CGMPackingController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private UnityEvent OnTrue;
    public static CGMPackingController Instance;
    public List<BuyerAI.SPAIData> SPAIData;

    [HideInInspector] public GameObject prefab;
    [HideInInspector] public bool isTrue;

    private float prefabHeight;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        BuyerSpawner.Instance.whenDisable += Stop;
    }
    private void OnDisable()
    {
        BuyerSpawner.Instance.whenDisable -= Stop;

    }
    public void Run()
    {
        StartCoroutine(RunIE());
    }
    private IEnumerator RunIE()
    {
        while (true)
        {
            if (isTrue)
            {
                yield return null;
                continue;
            }
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                Cashier_PackingMode cashier_PackingMode = hit.transform.GetComponent<Cashier_PackingMode>();
                if (cashier_PackingMode == null)
                {
                    yield return null;
                    continue;
                }
                Cashier_PackingMode.Mode mode = cashier_PackingMode.mode;

                if (mode == Cashier_PackingMode.Mode.Content)
                {
                    if (prefab != null)
                    {
                        prefabHeight = prefab.GetComponent<MeshFilter>().mesh.bounds.size.y;

                    }


                    yield return WhenContentMode();
                }
            }
            if (SPAIData.Count == 0)
            {
                Debug.Log("kewlarcuy");
                break;
            }
            yield return null;
        }

    }
    public void Stop()
    {


        StopAllCoroutines();

    }
    private IEnumerator WhenContentMode()
    {
        while (true)
        {

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                Cashier_PackingMode.Mode mode = hit.transform.GetComponent<Cashier_PackingMode>().mode;
                if (prefab != null)
                {
                    if (prefab.layer != 11)
                    {
                        prefab.layer = 11;

                    }
                }

                if (mode == Cashier_PackingMode.Mode.Desk && prefab != null)
                {
                    prefab.transform.position = new Vector3(hit.point.x, hit.point.y - (prefabHeight / 2), hit.point.z);

                }
                if (mode == Cashier_PackingMode.Mode.Bag)
                {
                    isTrue = true;
                    OnTrue?.Invoke();

                    break;
                }
            }
            yield return null;
        }


    }
}
