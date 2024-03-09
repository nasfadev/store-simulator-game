using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuestCanvas : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private TextMeshProUGUI askariRewardText;

    [SerializeField] private StoreData storeData;
    [SerializeField] private QuestGenerator questGenerator;
    [SerializeField] private TextMeshProUGUI statusText;
    // [SerializeField] private TextMeshProUGUI completeMoriumRewardText;
    [SerializeField] private TextMeshProUGUI completeAskariRewardText;

    [Header("Events")]
    [SerializeField] private UnityEvent whenQuestAcept;

    public void UpdateUI()
    {
        StoreData.QuestData questData = storeData.data.questData;
        explainText.text =
        Lean.Localization.LeanLocalization.GetTranslationText(questGenerator.questTranslation) +
        " " +
        questData.maxCount +
        " " +
        Lean.Localization.LeanLocalization.GetTranslationText(questGenerator.objectiveTranslation);
        // completeMoriumRewardText.text = "<sprite name=\"morium\">" + storeData.MoneyString(questData.moriumReward);
        askariRewardText.text = "<sprite name=\"askari\">" + storeData.MoneyString(questData.askariReward);
        completeAskariRewardText.text = "<sprite name=\"askari\">" + storeData.MoneyString(questData.askariReward);

    }
    public void AcceptQuest()
    {
        StoreData.QuestData questData = storeData.data.questData;
        questData.isQuestAccept = true;
        whenQuestAcept?.Invoke();
        UpdateStatusUI();
    }
    public void UpdateStatusUI()
    {
        StoreData.QuestData questData = storeData.data.questData;
        statusText.text =
         Lean.Localization.LeanLocalization.GetTranslationText(questGenerator.questTranslation) +
        " " +
        Lean.Localization.LeanLocalization.GetTranslationText(questGenerator.objectiveTranslation) +
        " " +
        questData.nowCount +
        "/" +
        questData.maxCount;
    }
}
