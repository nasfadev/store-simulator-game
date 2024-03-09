using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SellingPlatformProductData : MonoBehaviour
{
    public static SellingPlatformProductData Instance;
    [Header("NPC For Quest - Order By Enum Type")]
    public NPCData[] npcDatas;
    [Header("Shelves Data")]
    public Data[] data;
    [System.Serializable]
    public class NPCData
    {
        public Transform questTarget;
        public NPCs.Type npcNameType;
    }
    [System.Serializable]

    public class Data
    {
        public Sprite imageSprite;
        public Sprite prefabImageSprite;
        public GameObject prefab;
        public GameObject productPrefab;
        public Type type;
        public Vector2 EvenOddSize;
        public string titleTranslationName;
        public int maxStockQuantity;
        public int sellingPlatformPrice;
        public int sellingPlatformPriceMorium;

        public int buyProductPrice;
        public int buyProductPriceMorium;

        public int sellProductPrice;
        public int sellProductPriceMorium;

        public int level;

    }
    public enum Type
    {
        Snack,
        Drink,
        FruitVegetable,
        Electronic
    }
    private void Awake()
    {
        Instance = this;
    }
}
