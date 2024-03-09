
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [Header("Required Things")]
    [SerializeField] private StoreData _storeData;
    [SerializeField] private TrashProductData trashProductData;
    [SerializeField] private MeshCombiner meshCombiner;

    public TrashSpawnPoint spawnPoint;
    [SerializeField] private Transform spawnPointPos;
    [Header("Rarety")]

    [Range(0, 1000)]
    [SerializeField] private int commonChance;
    [Range(0, 1000)]
    [SerializeField] private int rareChance;
    [Range(0, 1000)]
    [SerializeField] private int legendaryChance;
    [Header("Configs")]
    [SerializeField] private float maxTimeToDrop;
    [SerializeField] private float waitAfterSpawn;

    [SerializeField] private int maxSpawnTrash;
    public int minTrashToAdvice;

    [HideInInspector] public Vector3 SpawnPos;
    [Header("Preview")]

    public int trashCount;
    private List<int> commonGrabAble;
    private List<int> rareGrabAble;
    private List<int> legendaryGrabAble;
    private List<int> commonToolAble;
    private List<int> rareToolAble;
    private List<int> legendaryToolAble;
    private bool isRemoving;
    [HideInInspector] public bool isWaitingToSpawn;
    public static TrashGenerator Instance;
    private void Awake()
    {
        commonGrabAble = new List<int>();
        rareGrabAble = new List<int>();
        legendaryGrabAble = new List<int>();
        commonToolAble = new List<int>();
        rareToolAble = new List<int>();
        legendaryToolAble = new List<int>();
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(Run());
        for (int i = 0; i < trashProductData.dataGrabAbles.Length; i++)
        {
            if (trashProductData.dataGrabAbles[i].rarety == TrashProductData.Rarety.Common)
            {
                commonGrabAble.Add(i);
            }
            else if (trashProductData.dataGrabAbles[i].rarety == TrashProductData.Rarety.Rare)
            {
                rareGrabAble.Add(i);
            }
            else
            {
                legendaryGrabAble.Add(i);
            }
        }
        for (int i = 0; i < trashProductData.dataToolAbles.Length; i++)
        {
            if (trashProductData.dataToolAbles[i].rarety == TrashProductData.Rarety.Common)
            {
                commonToolAble.Add(i);
            }
            else if (trashProductData.dataToolAbles[i].rarety == TrashProductData.Rarety.Rare)
            {
                rareToolAble.Add(i);
            }
            else
            {
                legendaryToolAble.Add(i);
            }
        }
    }
    public void Remove(Trash trash)
    {
        StartCoroutine(RemoveIE(trash));
    }
    private IEnumerator RemoveIE(Trash trash)
    {
        if (trash.type == Trash.Type.GrabAble)
        {
            Destroy(trash.gameObject);
            _storeData.data.trash[trash.id].quantity++;
            trashCount--;
            yield return null;
            meshCombiner.CombineMeshes(false);

        }
        else
        {
            Destroy(trash.gameObject);
            trashCount--;
            yield return null;
            meshCombiner.CombineMeshes(false);
        }
    }
    private IEnumerator Run()
    {
        while (!BuildNavmesh.navmeshReady)
        {
            yield return null;
        }
        float randomSeconds = Random.Range(0, maxTimeToDrop);
        yield return new WaitForSeconds(randomSeconds);
        while (true)
        {
            if (trashCount >= maxSpawnTrash)
            {
                yield return null;
                continue;
            }
            if (spawnPoint.isSpawnAble && isWaitingToSpawn)
            {


                int randomChance = Random.Range(0, 1001);

                if (randomChance <= commonChance)
                {
                    Debug.Log("draw common");

                    SpawnTrash(commonGrabAble, commonToolAble);


                }
                yield return null;
                meshCombiner.CombineMeshes(false);
                yield return new WaitForSeconds(waitAfterSpawn);

            }
            randomSeconds = Random.Range(0, maxTimeToDrop);
            isWaitingToSpawn = false;
            spawnPointPos.position = SpawnPos;

            yield return new WaitForSeconds(randomSeconds);
        }


    }
    private void SpawnTrash(List<int> grabAbleData, List<int> toolAbleData)
    {
        int grabAbleOrToolAble = Random.Range(0, 2);
        int randomIndex = Random.Range(0, grabAbleOrToolAble == 0 ? grabAbleData.Count : toolAbleData.Count);
        int randomItem = grabAbleOrToolAble == 0 ? grabAbleData[randomIndex] : toolAbleData[randomIndex];
        InstantiatePrefab(grabAbleOrToolAble, randomItem);
        trashCount++;

    }
    private void InstantiatePrefab(int grabAbleOrToolAble, int randomItem)
    {
        int randomRotateY = Random.Range(0, 360);
        GameObject prefab = grabAbleOrToolAble == 0 ? trashProductData.dataGrabAbles[randomItem].prefab : trashProductData.dataToolAbles[randomItem].prefab;
        Trash trash = prefab.GetComponent<Trash>();
        trash.id = randomItem;
        trash.type = grabAbleOrToolAble == 0 ? Trash.Type.GrabAble : Trash.Type.ToolAble;
        Instantiate(
        prefab,
        spawnPointPos.position + (Vector3.up * 0.002f),
        Quaternion.Euler(0f, randomRotateY, 0),
        transform);
    }

}
