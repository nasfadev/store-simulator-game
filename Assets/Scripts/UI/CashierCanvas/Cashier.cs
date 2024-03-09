using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    private void Awake()
    {
        // CashierBuilder.Instance.Run += DestroyThis;
    }
    private void Start()
    {
        transform.SetParent(VariousThingsBuilder.Instance.parentCashier);
    }
    private void DestroyThis()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        // CashierBuilder.Instance.Run -= DestroyThis;

    }
}
