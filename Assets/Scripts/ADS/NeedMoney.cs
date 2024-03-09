using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeedMoney : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [SerializeField] private SellingPlatformProductData sp;
    [SerializeField] private TextMeshProUGUI fieldText;
    [Header("Configa")]
    [SerializeField] private int divideRewardByBaseReward;
    public void UpdateUI()
    {
        SellingPlatformProductData.Data data = sp.data[Mathf.Clamp(storeData.data.shelvesBoughtCount, 0, sp.data.Length - 1)];
        int askariReward = (data.sellingPlatformPrice * storeData.addLevelMultiplier[data.level]) / divideRewardByBaseReward;
        fieldText.text = Lean.Localization.LeanLocalization.GetTranslationText("NeedMoney_Earn") + " <sprite name=\"askari\"> " + storeData.MoneyString(askariReward);
    }
    public void GetRewards()
    {
        SellingPlatformProductData.Data data = sp.data[Mathf.Clamp(storeData.data.shelvesBoughtCount, 0, sp.data.Length - 1)];
        int askariReward = (data.sellingPlatformPrice * storeData.addLevelMultiplier[data.level]) / divideRewardByBaseReward;
        EconomyCurrency.Instance.AddAskari(askariReward);
        EconomyNotif.Instance.Append("askari", "Askari", askariReward, true);
    }

}
