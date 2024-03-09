// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Mathematics;
// using Unity.Jobs;
// using Unity.Collections;
// using System;
// using BayatGames.SaveGameFree;
// using ThirtySec;
// using System.Threading.Tasks;
// using UnityEngine.Events;
// using System.Threading;

// // using System.Diagnostics;

// public class ContentsBuilder : MonoBehaviour
// {
//     [Header("Platform")]
//     [SerializeField] private int dimension;
//     [SerializeField] private Transform contentsPlatform;
//     [Header("Cashier")]

//     [SerializeField] private GameObject[] cashier;
//     [SerializeField] private int[] cashierMaxBuyerSlot;
//     [SerializeField] private Transform playerTransform;
//     [SerializeField] private float invokeDistance;
//     [SerializeField] private UnityEvent inArea;
//     [SerializeField] private UnityEvent outArea;
//     [SerializeField] private UnityEvent onDown;
//     [SerializeField] private Vector2[] cashierBoundsSize;


//     [Header("Shelves")]

//     [SerializeField] private GameObject[] shelveses;
//     [SerializeField] private int[] shelvesesMaxBuyerSlot;
//     [SerializeField] private Vector2[] shelvesesBoundsSize;


//     [Header("Exit")]

//     public static Vector3 exitPoint;
//     [SerializeField] private Transform ExitTarget;
//     // public static ShelvesesStatus[] shelvesesStatus;
//     public static NativeArray<int> maxBuyerSlot;
//     public static NativeArray<int> currentBuyerSlot;
//     public static NativeArray<bool2> currentBuyerSlotActive;

//     public static SaveContentsBuilder save;
//     public static ContentLibrary[] contentLibrary;

//     public static bool Loaded;

//     // Update is called once per frame
//     [System.Serializable]
//     public struct Int4
//     {
//         public int a;
//         public int b;
//         public int c;
//         public int d;
//         public float x;
//         public float y;


//         public Int4(int a, int b, int c, int d, float x, float y)
//         {

//             this.a = a;
//             this.b = b;
//             this.c = c;
//             this.d = d;
//             this.x = x;
//             this.y = y;


//         }
//     }
//     // public struct ShelvesesStatus
//     // {
//     //     public NativeArray<int> maxBuyerSlot;
//     //     public NativeArray<int> currentBuyerSlot;
//     // }
//     public struct ContentLibrary
//     {
//         public List<int2> Content;
//     }
//     public class SaveContentsBuilder : ThirtySec.Serializable<SaveContentsBuilder>
//     {
//         public Int4[] data;

//     }
//     private void Start()
//     {
//         exitPoint = ExitTarget.position;
//         maxBuyerSlot = new NativeArray<int>(dimension * dimension, Allocator.Persistent);
//         currentBuyerSlot = new NativeArray<int>(dimension * dimension, Allocator.Persistent);
//         currentBuyerSlotActive = new NativeArray<bool2>(dimension * dimension, Allocator.Persistent);


//         if (SaveContentsBuilder.IsExist())
//         {
//             LoadData();
//             StartCoroutine(SpawnContents());
//             // gameObject.SetActive(false);

//         }
//         else
//         {
//             save = new SaveContentsBuilder();
//             save.data = new Int4[dimension * dimension];
//             Loaded = true;
//             SetContentsLibraries();


//         }
//     }
//     private void Update()
//     {
//         if (PlaceButton.isPlace && PlaceButton.builderId.x == 3)
//         {
//             float3 ids = SnapPosition.snapPos.c0;
//             int minX = (int)(math.ceil(ids.x) - 1);
//             int maxY = (int)(math.ceil(ids.z) - 1);

//             GameObject prefab = Instantiate(GetContents(PlaceButton.builderId.y - 1, PlaceButton.builderId.z), new Vector3(math.ceil(ids.x) - .5f, 0, math.ceil(ids.z) - .5f), Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f), contentsPlatform);


//             int index = ((int)((dimension * maxY) + minX));
//             if (PlaceButton.builderId.z == 1)
//             {
//                 Cashier Cashier = prefab.GetComponent<Cashier>();
//                 Cashier.playerTransform = playerTransform;
//                 Cashier.invokeDistance = invokeDistance;
//                 Cashier.inArea = inArea;
//                 Cashier.outArea = outArea;
//                 Cashier.onDown = onDown;

//             }

//             Vector2 bounds = GetBoundsSize(PlaceButton.builderId.y - 1, PlaceButton.builderId.z);
//             save.data[index] = new Int4(PlaceButton.builderId.y, RotateButton.rotateID, 40, PlaceButton.builderId.z, bounds.x, bounds.y);

//             PlaceButton.isPlace = false;
//             PlaceButton.isRun = false;
//             SetContentsLibraries();

//         }
//     }
//     private GameObject GetContents(int id, int typeId)
//     {
//         GameObject Prefab;
//         switch (typeId)
//         {
//             case 1:
//                 Prefab = cashier[id];

//                 break;
//             default:
//                 Prefab = shelveses[id];
//                 break;
//         }
//         return Prefab;
//     }
//     private Vector2 GetBoundsSize(int id, int typeId)
//     {
//         Vector2 Prefab;
//         switch (typeId)
//         {
//             case 1:
//                 Prefab = cashierBoundsSize[id];

//                 break;
//             default:
//                 Prefab = shelvesesBoundsSize[id];
//                 break;
//         }
//         return Prefab;
//     }
//     private float getRotate(int id)
//     {
//         float degrees = 1f;
//         switch (id)
//         {
//             case 1:
//                 degrees = 0f;
//                 break;
//             case 2:
//                 degrees = 90f;
//                 break;
//             case 3:
//                 degrees = 180f;
//                 break;
//             default:
//                 degrees = 270f;
//                 break;

//         }
//         return degrees;
//     }
//     public void ChangeGuide()
//     {
//         ContentsGuide.prefab = GetContents(PlaceButton.builderId.y - 1, PlaceButton.builderId.z);
//         ContentsGuide.boundsSize = GetBoundsSize(PlaceButton.builderId.y - 1, PlaceButton.builderId.z);

//     }
//     public void SaveData()
//     {
//         SaveContentsBuilder.instance.data = save.data;
//         // SaveContentsBuilder.instance.Save<SaveContentsBuilder>();
//         // SaveGame.Save<Save>(saveFileName, save);

//     }
//     public void LoadData()
//     {
//         // save = SaveGame.Load<Save>(saveFileName, new Save());
//         save = new SaveContentsBuilder();
//         save.data = SaveContentsBuilder.instance.data;

//     }
//     private IEnumerator SpawnContents()
//     {

//         for (int i = 0; i < save.data.Length; i++)
//         {
//             Int4 index = save.data[i];

//             if (index.a > 0)
//             {
//                 float2 ids = new float2(i % dimension, math.floor(i / dimension));

//                 GameObject prefab = Instantiate(GetContents(index.a - 1, index.d), new Vector3(ids.x + .5f, 0, ids.y + .5f), Quaternion.Euler(0f, getRotate(index.b), 0f), contentsPlatform);
//                 if (index.d == 1)
//                 {
//                     Cashier Cashier = prefab.GetComponent<Cashier>();
//                     Cashier.playerTransform = playerTransform;
//                     Cashier.invokeDistance = invokeDistance;
//                     Cashier.inArea = inArea;
//                     Cashier.outArea = outArea;
//                     Cashier.onDown = onDown;
//                     Cashier.MaxbuyerSlot = GetMaxBuyerSlot(save.data[i].d, save.data[i].a);
//                 }
//                 yield return null;
//             }

//         }

//         SetContentsLibraries();

//     }
//     private async void SetContentsLibraries()
//     {

//         await Task.Run(() =>
//         {
//             contentLibrary = new ContentLibrary[shelveses.Length + cashier.Length];

//             for (int i = 0; i < contentLibrary.Length; i++)
//             {
//                 contentLibrary[i].Content = new List<int2>();
//             }
//             for (int i = 0; i < save.data.Length; i++)
//             {
//                 if (save.data[i].a > 0)
//                 {
//                     contentLibrary[save.data[i].d - 1].Content.Add(new int2(i, save.data[i].a));
//                     maxBuyerSlot[i] = GetMaxBuyerSlot(save.data[i].d, save.data[i].a);
//                 }
//             }




//             Loaded = true;


//         });
//     }
//     private int GetMaxBuyerSlot(int type, int index)
//     {
//         if (type == 1)
//         {
//             return cashierMaxBuyerSlot[index - 1];

//         }
//         else
//         {
//             return shelvesesMaxBuyerSlot[index - 1];

//         }
//     }
// }
