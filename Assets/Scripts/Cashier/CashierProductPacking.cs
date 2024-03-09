using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CashierProductPacking : MonoBehaviour
{

    public static CashierProductPacking Instance;
    [SerializeField] private Transform toteBag;
    [SerializeField] private Transform startPackingPos;
    [SerializeField] private Transform endPackingPos;
    [Header("Configs")]
    public float moveTweenTime;
    public float scaleTweenTime;
    [Header("Previews")]
    public Status status;
    private void Awake()
    {
        Instance = this;
    }
    public enum Status
    {
        Free,
        Start,
        Finish
    }
    public void AppearToteBag(float speed)
    {
        toteBag.localScale = Vector3.zero;
        toteBag.DOKill();
        toteBag.gameObject.SetActive(true);
        toteBag.DOScale(Vector3.one, speed).SetEase(Ease.OutBack);
    }
    public void DisappearToteBag(float speed)
    {
        toteBag.DOScale(Vector3.zero, speed).SetEase(Ease.InBack).OnComplete(() => { toteBag.gameObject.SetActive(false); });
    }
    public void InstanceThePrefab(int IDsp, float speed)
    {
        SellingPlatformProductData spData = SellingPlatformProductData.Instance;
        GameObject productPrefab = spData.data[IDsp - 1].productPrefab;
        GameObject prefab = Instantiate(productPrefab, startPackingPos.position, Quaternion.identity, transform);
        prefab.transform.Rotate(new Vector3(0, 90, 0));
        CashierProductPackingPrefab cashierProductPackingPrefab = prefab.AddComponent<CashierProductPackingPrefab>();
        cashierProductPackingPrefab.EndPos = endPackingPos.localPosition;
        cashierProductPackingPrefab.speed = speed;

    }
    public GameObject PlayerModeInstancePrefab(int IDsp)
    {
        SellingPlatformProductData spData = SellingPlatformProductData.Instance;
        GameObject productPrefab = spData.data[IDsp - 1].productPrefab;
        GameObject prefab = Instantiate(productPrefab, transform, false);
        prefab.GetComponent<BoxCollider>().enabled = true;
        prefab.transform.Rotate(new Vector3(0, 90, 0));

        return prefab;
    }
}
