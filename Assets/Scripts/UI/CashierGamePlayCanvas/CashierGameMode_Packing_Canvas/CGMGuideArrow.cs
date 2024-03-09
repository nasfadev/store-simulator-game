
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CGMGuideArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] arrowRect;
    public TextMeshProUGUI stockAmountText;
    public GameObject blank;

    private int arrowRoundId;
    public static CGMGuideArrow Instance;
    private void Awake()
    {
        Instance = this;
        BuyerSpawner.Instance.whenDisable += HideArrow;

    }
    private void OnDestroy()
    {
        BuyerSpawner.Instance.whenDisable -= HideArrow;

    }
    public void UpdateStockAmountText(int stockAmount)
    {
        stockAmountText.text = $"{stockAmount}x";

    }
    public int SetArrow()
    {
        arrowRect[arrowRoundId].gameObject.SetActive(false);
        arrowRoundId = Random.Range(0, arrowRect.Length);
        arrowRect[arrowRoundId].gameObject.SetActive(true);
        return arrowRoundId;
    }
    public void HideArrow()
    {
        arrowRect[arrowRoundId].gameObject.SetActive(false);

    }
}
