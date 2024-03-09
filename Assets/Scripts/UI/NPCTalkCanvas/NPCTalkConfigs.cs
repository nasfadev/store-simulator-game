using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkConfigs : MonoBehaviour
{
    [Header("Configs")]
    public string npcName;
    public NPCs.Type npcNameType;
    public Transform TextDialogCameraTarget;
    public MultiDialogData[] QuestDialogs;
    [System.Serializable]
    public class MultiDialogData
    {
        public TextDialog.DialogData[] dialogs;
    }
}
