using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousThingsProductData : MonoBehaviour
{
    public Data[] data;
    [System.Serializable]
    public class Data
    {
        public string translationName;
        public int askariPrice;
        public Sprite imageSprite;
        public GameObject prefab;
        public Type type;
        public Vector2 evenOdd;


    }
    public static VariousThingsProductData Instance;
    private void Awake()
    {
        Instance = this;
    }
    public enum Type
    {

        WorkerThings,
        Accessories,
        Cashier
    }
}
