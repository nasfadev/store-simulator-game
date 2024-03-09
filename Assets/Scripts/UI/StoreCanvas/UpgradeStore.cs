using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeStore : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI nowLevelText;
    [SerializeField] private TextMeshProUGUI willLevelText;
    [SerializeField] private TextMeshProUGUI nowIncomeText;
    [SerializeField] private TextMeshProUGUI willIncomeText;
    [SerializeField] private TextMeshProUGUI askariText;
    [SerializeField] private TextMeshProUGUI moriumText;
    [Header("Configs")]
    [SerializeField] private UnityEvent whenAfterUpgradeLevel;

    public void UpdateTexts()
    {
        StoreData.Data data = StoreData.Instance.data;
        StoreData storeData = StoreData.Instance;

        nowLevelText.text = $"{data.level}";
        willLevelText.text = $"{data.level + 1}";
        nowIncomeText.text = $"<sprite name=\"askari\"> {storeData.GetIncome(data.level)}x";
        willIncomeText.text = $"<sprite name=\"askari\"> {storeData.GetIncome(data.level + 1)}x";
        askariText.text = $"<sprite name=\"askari\"> {storeData.MoneyString(storeData.GetPriceAskari(data.level))}";
        moriumText.text = $"<sprite name=\"morium\"> {storeData.MoneyString(storeData.GetPriceMorium(data.level))}";
    }
    public void UpgradeLevel()
    {
        StoreData.Data data = StoreData.Instance.data;
        StoreData storeData = StoreData.Instance;
        EconomyCurrency economy = EconomyCurrency.Instance;
        EconomyNotif notif = EconomyNotif.Instance;
        int askari = storeData.GetPriceAskari(data.level);
        int morium = storeData.GetPriceMorium(data.level);
        if (!economy.CanAskariDecrease(askari))
        {
            return;
        }
        if (!economy.CanMoriumDecrease(morium))
        {
            return;
        }
        economy.DecreaseAskari(askari);
        economy.DecreaseMorium(morium);
        notif.Append("morium", "Morium", morium, false);
        notif.Append("askari", "Askari", askari, false);
        data.level++;
        data.questData.acceptQuestCount = 0;
        UpdateTexts();
        whenAfterUpgradeLevel?.Invoke();
    }
}
