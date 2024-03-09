using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _CanvasGroup;
    [SerializeField] private Image thumbnailImage;
    [SerializeField] private LeanLocalizedTextMeshProUGUI fieldText;
    [SerializeField] private CanvasSimpleTweenFade canvasSimpleTweenFade;
    [Header("Configs")]
    [SerializeField] private TutorialData[] tutorialDatas;
    //hiden
    private int _indexNow;
    [System.Serializable]
    public class TutorialData
    {
        public Sprite sprite;
        public GameObject translation;
    }
    private void Start()
    {
        if (!storeData.data.IsTutorial)
        {
            Run();
            _canvas.enabled = true;
            _CanvasGroup.alpha = 1f;
        }
    }
    public void Previous()
    {
        _indexNow = Mathf.Clamp(_indexNow - 1, 0, tutorialDatas.Length - 1);
        UpdateUI();

    }
    public void Next()
    {
        if (_indexNow + 1 >= tutorialDatas.Length)
        {
            storeData.data.IsTutorial = true;

            canvasSimpleTweenFade.Disappear();
            return;
        }
        _indexNow = Mathf.Clamp(_indexNow + 1, 0, tutorialDatas.Length - 1);
        UpdateUI();

    }
    public void Run()
    {
        _indexNow = 0;
        UpdateUI();

    }
    private void UpdateUI()
    {
        thumbnailImage.sprite = tutorialDatas[_indexNow].sprite;
        fieldText.TranslationName = tutorialDatas[_indexNow].translation.name;
    }
}
