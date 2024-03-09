using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreCanvas : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI incomeAmountText;

    [SerializeField] private Image startsScoreImage;
    private void Start()
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        StoreData.Data data = StoreData.Instance.data;
        levelText.text = data.level.ToString();
        startsScoreImage.fillAmount = data.score;
        incomeAmountText.text = $"<sprite name=\"askari\"> {data.income}x";
    }
}
