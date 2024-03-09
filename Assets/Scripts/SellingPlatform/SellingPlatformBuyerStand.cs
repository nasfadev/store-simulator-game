using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingPlatformBuyerStand : MonoBehaviour
{
    private void Awake()
    {
        VariousThingsBuilder.Instance.WhenRun += Active;
        VariousThingsBuilder.Instance.WhenStop += Deactive;

        SellingPlatformBuilder.Instance.Run += Active;
        SellingPlatformBuilder.Instance.OnRendering += Deactive;
        SellingPlatformBuilder.Instance.OnRendered += Active;
        SellingPlatformBuilder.Instance.Stop += Deactive;
        AutoMove.Instance.onMoveTouchedDone += Deactive;
        AutoMove.Instance.onMoveTouched += Active;

    }
    private void Deactive()
    {
        gameObject.SetActive(false);
    }
    private void Active()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        VariousThingsBuilder.Instance.WhenRun -= Active;
        VariousThingsBuilder.Instance.WhenStop -= Deactive;

        SellingPlatformBuilder.Instance.OnRendering -= Deactive;
        SellingPlatformBuilder.Instance.OnRendered -= Active;
        AutoMove.Instance.onMoveTouched -= Active;
        AutoMove.Instance.onMoveTouchedDone -= Deactive;
        SellingPlatformBuilder.Instance.Run -= Active;

        SellingPlatformBuilder.Instance.Stop -= Deactive;

    }
}
