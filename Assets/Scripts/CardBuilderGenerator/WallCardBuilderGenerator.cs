using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;
using UnityEngine.Events;

public class WallCardBuilderGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform cardBuilderLand;
    [SerializeField] private GameObject CardTemplatePrefab;
    [SerializeField] private string mode;
    [SerializeField] private SlotButton slotButton;
    [SerializeField] private DoorAndWindowProductData productData;

    [SerializeField] private UnityEvent WhenCardOnUp;
    [SerializeField] private UnityEvent AfterChangePlaceButtonPropertyOnUp;

    private List<GameObject> cardBuilderSave;

    private void Start()
    {
        Generate();
    }
    public void Generate()
    {
        StartCoroutine(GenerateIE());
    }
    private IEnumerator GenerateIE()
    {
        if (cardBuilderSave != null)
        {
            for (int i = 0; i < cardBuilderSave.Count; i++)
            {
                Destroy(cardBuilderSave[i]);
                Debug.Log("del");
                yield return null;

            }
        }
        Debug.Log("del");

        cardBuilderSave = new List<GameObject>();

        for (int i = 0; i < productData.data.Length; i++)
        {
            CardBuilder cardBuilder = CardTemplatePrefab.GetComponent<CardBuilder>();
            DoorAndWindowProductData.Data cardBuilderData = productData.data[i];
            cardBuilder.contentId = i + 1;
            cardBuilder.mode = mode;
            cardBuilder.imageSprite = cardBuilderData.imageSprite;
            cardBuilder.titleTranslationName = cardBuilderData.translationName;
            cardBuilder.price = cardBuilderData.askariPrice;
            cardBuilder.priceMorium = cardBuilderData.moriumPrice;
            cardBuilder.level = cardBuilderData.level;
            cardBuilder.slotButton = slotButton;
            GameObject prefab = Instantiate(CardTemplatePrefab, Vector3.zero, Quaternion.identity, cardBuilderLand);
            prefab.GetComponent<CardBuilder>().OnUp += InvokeTheEvents;
            prefab.GetComponent<CardBuilder>().AfterChangePlaceButtonPropertyOnUp += InvokeTheEventsSeconds;

            cardBuilderSave.Add(prefab);



            yield return null;
        }
    }
    private void InvokeTheEvents()
    {
        WhenCardOnUp?.Invoke();
        Debug.Log("muncul");
    }
    private void InvokeTheEventsSeconds()
    {
        AfterChangePlaceButtonPropertyOnUp?.Invoke();
        Debug.Log("muncul");
    }

}
