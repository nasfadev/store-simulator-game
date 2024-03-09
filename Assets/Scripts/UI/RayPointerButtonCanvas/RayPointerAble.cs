using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPointerAble : MonoBehaviour
{
    public Type mode;
    public enum Type
    {
        WareHouseShelves,
        SellingPlatform,
        GrabAbleTrash,
        ToolAbleTrash,
        Ignore,
        CashierAskari,
        NPC,
        LandSellSign,
        AreaLocker,
        Worker,
        Thief

    }
}

