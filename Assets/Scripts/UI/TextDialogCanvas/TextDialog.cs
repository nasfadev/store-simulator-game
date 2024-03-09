using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class TextDialog : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField]
    private CanvasSimpleTweenFade _canvasSimpleTweenFade;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private StoreData storeData;
    [SerializeField] private GameObject _textDialogCamera;
    [SerializeField] private GameObject _playerCamera;
    [Header("Preview")]
    public UnityEvent whenOnDialogFinish;
    public Transform TargetCamera;
    private Tween tween;
    public DialogData[] dialogDatas;
    private int nowIndex;
    private int maxIndex;
    [System.Serializable]
    public class DialogData
    {
        public bool isPlayer;
        public string name;
        public GameObject dialog;
    }
    public void StartDialog()
    {
        _textDialogCamera.SetActive(true);
        _playerCamera.SetActive(false);
        _textDialogCamera.transform.position = TargetCamera.position;
        _textDialogCamera.transform.rotation = TargetCamera.rotation;

        nowIndex = 0;
        maxIndex = dialogDatas.Length;
        UpdateDialog();
    }
    public void NextDialog()
    {
        nowIndex++;
        if (nowIndex >= maxIndex)
        {
            _textDialogCamera.SetActive(false);
            _playerCamera.SetActive(true);
            _canvasSimpleTweenFade.Disappear();
            whenOnDialogFinish?.Invoke();
            return;
        }
        UpdateDialog();

    }
    public void UpdateDialog()
    {
        DialogData dialogData = dialogDatas[nowIndex];
        nameText.text = dialogData.isPlayer ? storeData.data.name : dialogData.name;
        dialogText.text = LeanLocalization.GetTranslationText(dialogData.dialog.name);
    }

}
