using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CashierStand : MonoBehaviour
{
    // public UnityEvent OnEnter;
    // public UnityEvent OnExit;
    public LayerMask layerMask;
    public List<BuyerAI.SPAIData> sPAIDatas;
    public bool isStand;
    private void OnTriggerEnter(Collider other)
    {
        if (!(Player.Instance.mode == Player.Mode.Free || Player.Instance.mode == Player.Mode.Cashier))
        {
            return;
        }
        if (layerMask != (layerMask | (1 << other.gameObject.layer)))
        {
            return;
        }

        Debug.Log("enter cashie stand");
        // OnEnter?.Invoke();
        Player.Instance.mode = Player.Mode.Cashier;
        isStand = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!(Player.Instance.mode == Player.Mode.Free || Player.Instance.mode == Player.Mode.Cashier))
        {
            return;
        }
        if (layerMask != (layerMask | (1 << other.gameObject.layer)))
        {
            return;
        }
        // OnExit?.Invoke();
        Player.Instance.mode = Player.Mode.Free;

        isStand = false;
    }
}
