using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DoorAndWindowProductData : MonoBehaviour
{
    public Data[] data;
    [System.Serializable]
    public class Data
    {
        public string translationName;
        public Sprite imageSprite;
        public GameObject prefab;
        public GameObject prefabBlueprint;
        public int askariPrice;
        public int moriumPrice;
        public int level;
        public bool isInCard;
        public bool isMultipleAdded;
        public int longInMeter;


    }
    public static DoorAndWindowProductData Instance;
    private void Awake()
    {
        Instance = this;
    }
}
