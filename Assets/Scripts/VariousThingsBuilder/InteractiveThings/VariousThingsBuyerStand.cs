using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousThingsBuyerStand : MonoBehaviour
{
    private void Start()
    {
        SellingPlatformBuilder.Instance.Run += Active;
        SellingPlatformBuilder.Instance.Stop += Deactive;
        VariousThingsBuilder.Instance.WhenRun += Active;

        VariousThingsBuilder.Instance.OnBeforeRender += Deactive;
        VariousThingsBuilder.Instance.OnAfterRender += Active;
        VariousThingsBuilder.Instance.WhenStop += Deactive;
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
        SellingPlatformBuilder.Instance.Run -= Active;
        VariousThingsBuilder.Instance.WhenRun -= Active;

        VariousThingsBuilder.Instance.OnBeforeRender -= Deactive;
        VariousThingsBuilder.Instance.OnAfterRender -= Active;
        VariousThingsBuilder.Instance.WhenStop -= Deactive;
        AutoMove.Instance.onMoveTouchedDone -= Deactive;

        AutoMove.Instance.onMoveTouched -= Active;
        SellingPlatformBuilder.Instance.Stop -= Deactive;

    }
}
