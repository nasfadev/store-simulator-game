using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thiefs : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData _storeData;
    [SerializeField] private SellingPlatformProductData _sellingPlatformProductData;
    [SerializeField] private RealtimeDataBuyerSystem _realtimeDataBuyerSystem;
    [SerializeField] private StockInventoryData _stockInventoryData;
    [SerializeField] private EconomyNotif _economyNotif;
    [Header("Configs")]
    [SerializeField] private int _maxSpawn;
    [Range(0, 30)]
    [SerializeField] private int _maxStockToSteal;
    [SerializeField] private float _startColdownSpawnTime;

    [SerializeField] private float _minColdownSpawnTime;

    [SerializeField] private float _maxColdownSpawnTime;

    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private GameObject[] _thiefPrefabs;
    [Header("Previews")]
    public int SpawnCount;

    private void OnEnable()
    {
        Run();
    }
    private void Run()
    {
        StartCoroutine(RunIE());
    }
    private IEnumerator RunIE()
    {
        yield return new WaitForSeconds(_startColdownSpawnTime);
        while (!BuildNavmesh.navmeshReady)
        {
            yield return null;
        }
        while (true)
        {

            if (SpawnCount < _maxSpawn)
            {
                Vector3 spawnPosition = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
                GameObject thiefPrefab = _thiefPrefabs[Random.Range(0, _thiefPrefabs.Length)];
                Thief thief = thiefPrefab.GetComponent<Thief>();
                thief.StoreData = _storeData;
                thief.SellingPlatformProductData = _sellingPlatformProductData;
                thief.RealtimeDataBuyerSystem = _realtimeDataBuyerSystem;
                thief.StockInventoryData = _stockInventoryData;
                thief.EconomyNotif = _economyNotif;
                thief.ExitPosition = spawnPosition;
                thief.StockQuantityToThief = Random.Range(1, _maxStockToSteal + 1);
                thief.Thiefs = this;
                Instantiate(thiefPrefab, spawnPosition, Quaternion.identity, transform);
                SpawnCount++;
                yield return new WaitForSeconds(Random.Range(_minColdownSpawnTime, _maxColdownSpawnTime));
            }
            yield return null;
        }
    }

}
