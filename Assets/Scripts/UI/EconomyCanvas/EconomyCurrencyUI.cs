using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI askariText;
    [SerializeField] private TextMeshProUGUI moriumText;
    private void Start()
    {
        UpdateTexts();
        EconomyCurrency.Instance.update += UpdateTexts;
    }
    private void UpdateTexts()
    {
        int askari = StoreData.Instance.data.askari;
        int morium = StoreData.Instance.data.morium;
        string askariFormatted = StoreData.Instance.MoneyString(askari);
        string moriumFormatted = StoreData.Instance.MoneyString(morium);

        askariText.text = $"<sprite index=2> {askariFormatted}  <sprite index=0>";
        moriumText.text = $"<sprite index=1> {moriumFormatted}  <sprite index=0>";
    }

}
