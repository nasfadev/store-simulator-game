using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLoaded : MonoBehaviour
{
    // final state is 7
    // 1 = FloorBuilder
    // 2 = RoofFloorBuilder
    // 3 = DoorAndWindowBuilder
    // 4 = SellingPlatformBuilder
    // 5 = StoreData
    // 6 = VariousThingsBuilder

    public static int Loaded;
    public static bool isLoaded;
    public static int initLoaded;
    public static int maxLoaded = 6;
    private int tempLoaded;

    private void Awake()
    {
        StartCoroutine(CheckLoaded());
    }
    private IEnumerator CheckLoaded()
    {
        while (true)
        {
            if (Loaded != tempLoaded)
            {
                tempLoaded = Loaded;
                Debug.Log($"loaded state{tempLoaded}");
            }
            Debug.Log($"init loaded state{initLoaded}");

            if (Loaded == maxLoaded)
            {
                if (initLoaded > 0 && initLoaded < maxLoaded)
                {
                    FloorBuilder.FloorBuilderSaveData.instance.pass = 0;
                    RoofFloorBuilder.FloorRoofBuilderSaveData.instance.pass = 0;
                    DoorAndWindowBuilder.DoorAndWindowSaveData.instance.pass = 0;
                    SellingPlatformBuilder.SellingPlatformDataSave.instance.pass = 0;
                    StoreData.StoreDataSave.instance.pass = 0;
                    VariousThingsBuilder.VariousThingsDataSave.instance.pass = 0;
                }
                isLoaded = true;
                break;
            }

            yield return null;
        }
    }
}
