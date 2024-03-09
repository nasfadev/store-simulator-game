// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Collections;
// using Unity.Mathematics;
// using Unity.Jobs;
// using System.Threading.Tasks;
// using BayatGames.SaveGameFree;
// using ThirtySec;

// public class WallBuilder : MonoBehaviour
// {
//     // Start is called before the first frame update
//     [SerializeField] private int dimension;
//     [SerializeField] private float thickness;
//     [SerializeField] private float tall;

//     [SerializeField] private Transform wallPlatform;
//     [SerializeField] private MeshFilter WallMF;
//     [SerializeField] private MeshRenderer WallMR;
//     [SerializeField] private MeshCollider wallMC;
//     private bool Loaded;
//     private Coroutine runWallBuilderCoroutine;



//     // private NativeArray<int> wallType;
//     private NativeArray<int> wallID;
//     private NativeArray<bool4> wallExisting;
//     private NativeArray<int> wallCount;
//     private NativeArray<int> cornerWallCount;
//     private NativeArray<int> renderWallTris;

//     private NativeArray<float3> renderWallVerts;
//     private NativeArray<float2> renderUVs;

//     private string saveFileName = "wll.ardodev";


//     // private NativeArray<float3x4> wallPosition;
//     private JobHandle RenderHandle;
//     private JobHandle AddHandle;
//     private JobHandle DeleteHandle;
//     private bool isRender;
//     private bool isRunning;
//     private bool isAdd;
//     private bool isLoadData;

//     [System.Serializable]
//     public struct Bool4
//     {
//         public bool a;
//         public bool b;
//         public bool c;
//         public bool d;

//         public Bool4(bool a, bool b, bool c, bool d)
//         {

//             this.a = a;
//             this.b = b;
//             this.c = c;
//             this.d = d;
//         }
//     }


//     public class WallBuilderSaveData : ThirtySec.Serializable<WallBuilderSaveData>
//     {

//         public Bool4[] wallExisting;
//         public int[] wallID;
//         public int wallCount;
//         public int cornerWallCount;
//     }

//     void Start()
//     {
//         // thickness /= 2;.07

//         wallID = new NativeArray<int>(dimension * dimension, Allocator.Persistent);
//         wallCount = new NativeArray<int>(1, Allocator.Persistent);
//         cornerWallCount = new NativeArray<int>(1, Allocator.Persistent);
//         wallExisting = new NativeArray<bool4>(dimension * dimension, Allocator.Persistent);
//         if (WallBuilderSaveData.IsExist())
//         {
//             // gameObject.SetActive(false);

//             LoadData();
//         }
//         else
//         {
//             SaveData();

//             // gameObject.SetActive(false);

//         }


//     }
//     public void Run()
//     {
//         runWallBuilderCoroutine = StartCoroutine(RunWallBuilder());
//     }
//     public void Stop()
//     {
//         StartCoroutine(SafeStop());
//     }
//     private IEnumerator SafeStop()
//     {
//         while (true)
//         {
//             if (!isRunning && Loaded)
//             {
//                 if (runWallBuilderCoroutine != null)
//                 {
//                     StopCoroutine(runWallBuilderCoroutine);
//                 }
//                 break;
//             }
//             yield return null;
//         }

//     }
//     // Update is called once per frame
//     private IEnumerator RunWallBuilder()
//     {
//         while (true)
//         {
//             if (!isRunning && PlaceButton.Instance.contentID > 0 && PlaceButton.Instance.isExecute && PlaceButton.Instance.mode == "WallBuilder")
//             {
//                 float3x2 ids = new float3x2(wallPlatform.InverseTransformPoint(WallGuide.Instance.firstSnapPos), wallPlatform.InverseTransformPoint(WallGuide.Instance.lastSnapPos));

//                 AddWallJob addWallJob = new AddWallJob
//                 {
//                     ids = ids,
//                     dimension = dimension,
//                     rotateID = WallGuide.Instance.rotateIdSaved,
//                     wallID = wallID,
//                     wallExisting = wallExisting,
//                     wallCount = wallCount,
//                     cornerWallCount = cornerWallCount
//                 };
//                 AddHandle = addWallJob.Schedule();
//                 PlaceButton.Instance.isExecute = false;
//                 isRunning = true;
//             }
//             if (!isRunning && PlaceButton.Instance.contentID == 0 && PlaceButton.Instance.isExecute && PlaceButton.Instance.mode == "WallBuilder")
//             {
//                 Debug.Log($" wall contentID{PlaceButton.Instance.contentID}");
//                 float3x2 ids = new float3x2(wallPlatform.InverseTransformPoint(WallGuide.Instance.firstSnapPos), wallPlatform.InverseTransformPoint(WallGuide.Instance.lastSnapPos));

//                 DeleteWallJob deleteWallJob = new DeleteWallJob
//                 {
//                     ids = ids,
//                     dimension = dimension,
//                     rotateID = WallGuide.Instance.rotateIdSaved,
//                     wallID = wallID,
//                     wallExisting = wallExisting,
//                     wallCount = wallCount,
//                     cornerWallCount = cornerWallCount
//                 };
//                 DeleteHandle = deleteWallJob.Schedule();
//                 PlaceButton.Instance.isExecute = false;
//                 isRunning = true;
//             }
//             if ((AddHandle.IsCompleted || DeleteHandle.IsCompleted) && !isRender && (isRunning || isLoadData))
//             {
//                 AddHandle.Complete();
//                 DeleteHandle.Complete();
//                 renderWallTris = new NativeArray<int>((wallCount[0] * 18) + (cornerWallCount[0] * 30), Allocator.Persistent);
//                 renderWallVerts = new NativeArray<float3>((wallCount[0] * 12) + (cornerWallCount[0] * 20), Allocator.Persistent);
//                 renderUVs = new NativeArray<float2>((wallCount[0] * 12) + (cornerWallCount[0] * 20), Allocator.Persistent);
//                 isRender = true;
//                 RenderWallJob renderWallJob = new RenderWallJob
//                 {
//                     dimension = dimension,
//                     tall = tall,
//                     thickness = thickness,
//                     wallID = wallID,
//                     wallExisting = wallExisting,
//                     renderWalltris = renderWallTris,
//                     renderWallverts = renderWallVerts,
//                     renderUVs = renderUVs

//                 };

//                 RenderHandle = renderWallJob.Schedule();
//             }
//             if (RenderHandle.IsCompleted && isRender && (isRunning || isLoadData))
//             {
//                 RenderHandle.Complete();
//                 Render();
//                 isRender = false;
//                 isRunning = false;
//                 isLoadData = false;
//                 PlaceButton.Instance.isRun = false;
//                 Debug.Log("mulai");
//             }
//             yield return null;
//         }

//     }
//     private IEnumerator RenderLoad()
//     {
//         while (true)
//         {
//             if (isLoadData)
//             {
//                 renderWallTris = new NativeArray<int>((wallCount[0] * 18) + (cornerWallCount[0] * 30), Allocator.Persistent);
//                 renderWallVerts = new NativeArray<float3>((wallCount[0] * 12) + (cornerWallCount[0] * 20), Allocator.Persistent);
//                 renderUVs = new NativeArray<float2>((wallCount[0] * 12) + (cornerWallCount[0] * 20), Allocator.Persistent);

//                 isLoadData = false;
//                 isRender = true;
//                 RenderWallJob renderWallJob = new RenderWallJob
//                 {
//                     dimension = dimension,
//                     tall = tall,
//                     thickness = thickness,
//                     wallID = wallID,
//                     wallExisting = wallExisting,
//                     renderWalltris = renderWallTris,
//                     renderWallverts = renderWallVerts,
//                     renderUVs = renderUVs

//                 };

//                 RenderHandle = renderWallJob.Schedule();
//             }
//             if (RenderHandle.IsCompleted && isRender)
//             {
//                 RenderHandle.Complete();
//                 Render();
//                 isRender = false;
//                 isRunning = false;
//                 isLoadData = false;
//                 Debug.Log("mulai");
//                 Stop();
//                 break;
//             }
//             yield return null;
//         }

//     }
//     void Render()
//     {
//         Mesh mesh = new Mesh();
//         Vector3[] vert = new Vector3[renderWallVerts.Length];
//         Vector2[] uvs = new Vector2[renderWallVerts.Length];

//         int[] tris = new int[renderWallTris.Length];
//         for (int i = 0; i < vert.Length; i++)
//         {
//             vert[i] = renderWallVerts[i];
//             uvs[i] = renderUVs[i];
//         }
//         for (int i = 0; i < tris.Length; i++)
//         {
//             tris[i] = renderWallTris[i];
//         }
//         mesh.vertices = vert;
//         mesh.triangles = tris;
//         mesh.uv = uvs;
//         mesh.RecalculateBounds();
//         mesh.RecalculateNormals();
//         // string s = "";
//         // int numtris = 0;
//         // for (int i = 0; i < tris.Length; i++)
//         // {

//         //     s += $"{tris[i]}, ";
//         //     numtris++;

//         //     if (numtris == 6)
//         //     {
//         //         s += " | ";
//         //         numtris = 0;
//         //     }

//         // }
//         // // VertexDebug.vertex = vert;
//         // UnityEngine.Debug.Log($" tris : {s}");
//         // UnityEngine.Debug.Log($" corner : {cornerWallCount[0]}");
//         // UnityEngine.Debug.Log($" corner : {wallCount[0]}");

//         WallMF.mesh = mesh;
//         wallMC.sharedMesh = mesh;
//         if (!Loaded)
//         {
//             Loaded = true;
//             StartCoroutine(CheckStateLoadedRequirement());

//         }

//     }
//     private IEnumerator CheckStateLoadedRequirement()
//     {
//         int numThisState = 3;
//         while (!(StateLoaded.Loaded + 1 == numThisState))
//         {
//             yield return null;
//         }
//         StateLoaded.Loaded++;

//     }
//     public async void SaveData()
//     {
//         WallBuilderSaveData save = new WallBuilderSaveData();

//         await Task.Run(() =>
//         {
//             save.wallID = new int[wallID.Length];
//             save.wallExisting = new Bool4[wallExisting.Length];
//             for (int i = 0; i < wallID.Length; i++)
//             {
//                 save.wallID[i] = wallID[i];
//                 bool4 a = wallExisting[i];
//                 save.wallExisting[i] = new Bool4(a.x, a.y, a.z, a.w);

//             }
//             save.cornerWallCount = cornerWallCount[0];
//             save.wallCount = wallCount[0];
//         });
//         UnityEngine.Debug.Log($" savewall");
//         WallBuilderSaveData.instance.wallID = save.wallID;
//         WallBuilderSaveData.instance.wallExisting = save.wallExisting;
//         WallBuilderSaveData.instance.cornerWallCount = save.cornerWallCount;
//         WallBuilderSaveData.instance.wallCount = save.wallCount;
//         if (!Loaded)
//         {
//             WallBuilderSaveData.instance.Save<WallBuilderSaveData>();
//             Loaded = true;
//             StartCoroutine(CheckStateLoadedRequirement());


//         }


//         // SaveGame.Save<WallBuilderSaveData>(saveFileName, save);

//     }
//     private async void LoadData()
//     {
//         WallBuilderSaveData save = WallBuilderSaveData.instance;

//         await Task.Run(() =>
//         {
//             for (int i = 0; i < save.wallID.Length; i++)
//             {
//                 wallID[i] = save.wallID[i];
//                 Bool4 dt = save.wallExisting[i];
//                 wallExisting[i] = new bool4(dt.a, dt.b, dt.c, dt.d);


//             }
//             cornerWallCount[0] = save.cornerWallCount;
//             wallCount[0] = save.wallCount;
//         });
//         isLoadData = true;
//         StartCoroutine(RenderLoad());



//     }
// }
