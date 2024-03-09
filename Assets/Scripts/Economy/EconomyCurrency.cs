using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.Events;
public class EconomyCurrency : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [Header("Configs")]
    [SerializeField] private UnityEvent whenDoesntHaveMoney;
    public event Action update;

    public static EconomyCurrency Instance;
    private void Start()
    {
        Instance = this;

    }
    public void AddAskari(int amount)
    {
        storeData.data.askari += amount;
        update?.Invoke();
    }
    public void AddMorium(int amount)
    {
        storeData.data.morium += amount;
        update?.Invoke();

    }
    public void DecreaseAskari(int amount)
    {

        storeData.data.askari -= amount;

        update?.Invoke();

    }
    public void DecreaseMorium(int amount)
    {

        storeData.data.morium -= amount;
        update?.Invoke();



    }
    public bool CanAskariDecrease(int amount)
    {
        int count = storeData.data.askari - amount;
        if (count < 0)
        {
            whenDoesntHaveMoney?.Invoke();
            return false;
        }
        return true;
    }
    public bool CanMoriumDecrease(int amount)
    {
        int count = storeData.data.morium - amount;
        if (count < 0)
        {
            return false;
        }
        return true;


    }
}
