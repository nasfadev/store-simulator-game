using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NPCTalk : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private LeanLocalizedTextMeshProUGUI talkTranslation;
    [SerializeField] private StoreData storeData;
    [SerializeField] private QuestGenerator questGenerator;
    [SerializeField] private TextDialog textDialog;
    [Header("Configs")]
    [SerializeField] private string yesQuestTalkTranslation;
    [SerializeField] private string noQuestTalkTranslation;
    [SerializeField] private bool isDisableQuest;
    [Header("QuestEvent")]
    [SerializeField] private UnityEvent whenTalkQuest;
    private NPCs.Type tempNpcNameType;
    private TextDialog.DialogData[] tempQuestDialogs;



    public void Set(NPCTalkConfigs npcTalkConfigs)
    {
        tempNpcNameType = npcTalkConfigs.npcNameType;
        nameText.text = npcTalkConfigs.npcName;
        StoreData.QuestData questData = storeData.data.questData;

        if (questData.npcNameType == npcTalkConfigs.npcNameType && !questData.isQuestAccept)
        {
            tempQuestDialogs = npcTalkConfigs.QuestDialogs[Random.Range(0, npcTalkConfigs.QuestDialogs.Length)].dialogs;
            textDialog.TargetCamera = npcTalkConfigs.TextDialogCameraTarget;

            YesQuest();
        }
        else
        {
            NoQuest();
        }
    }
    private void YesQuest()
    {
        if (isDisableQuest)
        {
            talkTranslation.TranslationName = noQuestTalkTranslation;
            return;
        }
        talkTranslation.TranslationName = yesQuestTalkTranslation;
    }
    private void NoQuest()
    {

        talkTranslation.TranslationName = noQuestTalkTranslation;
    }
    public void CheckQuest()
    {
        if (isDisableQuest)
        {
            return;
        }
        StoreData.QuestData questData = storeData.data.questData;

        if (questData.npcNameType == tempNpcNameType && !questData.isQuestAccept)
        {
            textDialog.dialogDatas = tempQuestDialogs;
            textDialog.whenOnDialogFinish = questGenerator.acceptedQuestEvent;
            whenTalkQuest?.Invoke();
        }
    }
}
