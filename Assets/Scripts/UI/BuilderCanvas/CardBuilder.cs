using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using Lean.Localization;
using TMPro;




public class CardBuilder : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform cardBuilder;
    [SerializeField] private float duration;
    [SerializeField] private float scale;
    public event Action OnUp;
    public event Action AfterChangePlaceButtonPropertyOnUp;
    [SerializeField] private Image cardImage;
    [SerializeField] private LeanLocalizedTextMeshProUGUI titleTranslationNameLean;
    [SerializeField] private TextMeshProUGUI priceTextMesh;

    [SerializeField] private GameObject textLevelPanel;
    [SerializeField] private TextMeshProUGUI textLevel;


    public SlotButton slotButton;
    public int contentId;
    public string mode;

    public Sprite imageSprite;
    public string titleTranslationName;
    public int price;
    public int priceMorium;

    public int level;

    private bool isRun;
    public static bool isChange;
    private bool isPointerDisable;


    private void Start()
    {
        cardImage.sprite = imageSprite;
        titleTranslationNameLean.TranslationName = titleTranslationName;
        priceTextMesh.text = $"<sprite index=2> <alpha=#cc>{price}";
        if (level > StoreData.StoreDataSave.instance.data.level)
        {
            textLevelPanel.SetActive(true);
            textLevel.text += level.ToString();
            cardImage.color = new Color32(168, 168, 168, 255);
            isPointerDisable = true;
        }


    }
    public void OnPointerDown(PointerEventData data)
    {
        if (isPointerDisable)
        {
            return;
        }

        if (!isChange)
        {
            isChange = true;
            cardBuilder.DOScale(Vector3.one * scale, duration);
            isRun = true;
        }

    }
    public void OnPointerMove(PointerEventData data)
    {
        if (isRun)
        {
            cardBuilder.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);
            isRun = false;
            Debug.Log("hit");
            isChange = false;

        }

    }
    public void OnPointerUp(PointerEventData data)
    {
        if (isRun)
        {
            if (!PlacerPanel.isDisappearing && !BuilderPanel.Instance.isDisappearing)
            {
                OnUp?.Invoke();
                ChangeContentId();
                AfterChangePlaceButtonPropertyOnUp?.Invoke();
                slotButton.ChangeImage(imageSprite);
            }
            Debug.Log("up");
            cardBuilder.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);

            isRun = false;
            isChange = false;

        }

    }
    private void ChangeContentId()
    {
        PlaceButton.Instance.ChangeMode(mode);

        PlaceButton.Instance.ChangeContentID(contentId);
    }
}
