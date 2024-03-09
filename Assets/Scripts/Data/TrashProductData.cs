using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;

public class TrashProductData : MonoBehaviour
{
    public DataGrabAble[] dataGrabAbles;
    public DataToolAble[] dataToolAbles;
    public static TrashProductData Instance;
    private void Awake()
    {
        Instance = this;
    }
    [System.Serializable]
    public class DataGrabAble
    {
        public string translationName;
        public Sprite thumbnail;
        public GameObject prefab;
        public Rarety rarety;

    }
    [System.Serializable]
    public class DataToolAble
    {
        public GameObject prefab;
        public Rarety rarety;

    }
    public enum Rarety
    {
        Common,
        Rare,
        Legendary
    }
}
