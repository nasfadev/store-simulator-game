using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using System.Threading.Tasks;
using ThirtySec;
using Unity.VisualScripting;

public class RoofFloorBuilder : MonoBehaviour
{
    [SerializeField] private TemplateData _templateData;
    [SerializeField] private int dimension;
    [SerializeField] private Transform floorPlatform;
    [SerializeField] private MeshFilter FloorMF;
    [SerializeField] private MeshFilter BottomFloorMF;
    [SerializeField] private MeshFilter cornerFloorMF;


    [SerializeField] private MeshRenderer FloorMR;
    [SerializeField] private MeshRenderer BottomFloorMR;


    [SerializeField] private Material[] FloorMaterials;

    private NativeArray<int> floorUsed;
    private NativeArray<int> cornerCount;
    private bool Loaded;
    public static RoofFloorBuilder Instance;
    private NativeArray<int> floorType;
    private NativeArray<int> floorId;
    private NativeArray<int> vertexCount;
    private NativeArray<int> floorCount;
    private NativeArray<int4x2> verticesTop;
    private NativeArray<int4> tempVerticesTop;
    private NativeArray<int4> vertexData;

    private NativeArray<float3x4> vertexTop;
    private NativeArray<int> RenderID;
    private NativeArray<int> RenderType;
    private NativeArray<int> floorUsedCount;
    private NativeArray<int> floorUsedIndex;
    private NativeArray<int> cornerRenderTris;
    private NativeArray<float3> cornerRenderVerts;
    private NativeArray<float3> RenderPosa;
    private Coroutine runBuilderCoroutine;

    private NativeArray<int> FloorID;
    private JobHandle RenderHandle;
    private JobHandle AddHandle;
    private JobHandle DeleteHandle;
    private FloorMaterialData[] subData;
    private bool isAdd;
    private bool isDelete;
    private bool isLoad;
    private bool isRender;
    private bool isRunning;
    private bool IsClear;
    private bool isSaving;

    [System.Serializable]
    public struct Int4x2
    {
        public int a1;
        public int a2;
        public int a3;
        public int a4;

        public int b1;
        public int b2;
        public int b3;
        public int b4;

        public Int4x2(int a1, int a2, int a3, int a4, int b1, int b2, int b3, int b4)
        {
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;
            this.a4 = a4;

            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
            this.b4 = b4;
        }
    }
    [System.Serializable]

    public struct Float3x4
    {
        public float a1;
        public float a2;
        public float a3;

        public float b1;
        public float b2;
        public float b3;

        public float c1;
        public float c2;
        public float c3;

        public float d1;
        public float d2;
        public float d3;
        public Float3x4(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3, float d1, float d2, float d3)
        {
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;

            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;

            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;

            this.d1 = d1;
            this.d2 = d2;
            this.d3 = d3;
        }
    }

    // Start is called before the first frame update


    public class FloorRoofBuilderSaveData : ThirtySec.Serializable<FloorRoofBuilderSaveData>
    {
        public int pass;
        public int[] floorType;
        public int[] vertexCount;
        public int[] floorCount;
        public int[] floorUsed;
        public int[] floorId;
        public Int4x2[] verticesTop;
        public Float3x4[] vertexTop;
    }
    public class DataTemplate
    {
        public int[] floorType;
        public int[] vertexCount;
        public int[] floorCount;
        public int[] floorUsed;
        public int[] floorId;
        public Int4x2[] verticesTop;
        public Float3x4[] vertexTop;
    }
    private void Awake()
    {
        Instance = this;
        floorType = new NativeArray<int>(dimension * dimension, Allocator.Persistent);
        vertexCount = new NativeArray<int>(1, Allocator.Persistent); /* just 1 */
        floorCount = new NativeArray<int>(1, Allocator.Persistent); /* just 1 */
        cornerCount = new NativeArray<int>(1, Allocator.Persistent); /* just 1 */

        floorUsed = new NativeArray<int>(FloorMaterials.Length, Allocator.Persistent);
        floorId = new NativeArray<int>(dimension * dimension, Allocator.Persistent);
        verticesTop = new NativeArray<int4x2>(dimension * dimension, Allocator.Persistent);
        vertexTop = new NativeArray<float3x4>(dimension * dimension, Allocator.Persistent);
        if (FloorRoofBuilderSaveData.IsExist() && FloorRoofBuilderSaveData.instance != null && UnityEngine.PlayerPrefs.GetInt("floorRoofPass") == FloorRoofBuilderSaveData.instance.pass)

        {
            LoadData();
        }
        else
        {
            StateLoaded.initLoaded++;
            FloorRoofBuilderSaveData save = FloorRoofBuilderSaveData.instance;
            DataTemplate dataTemplate = JsonUtility.FromJson<DataTemplate>(_templateData.floorRoofBuilder1);
            int seed = (int)System.DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int pass = UnityEngine.Random.Range(1000, 3000);
            UnityEngine.PlayerPrefs.SetInt("floorRoofPass", pass);
            save.pass = pass;
            save.floorType = dataTemplate.floorType;
            save.vertexCount = dataTemplate.vertexCount;
            save.floorCount = dataTemplate.floorCount;
            save.floorUsed = dataTemplate.floorUsed;
            save.floorId = dataTemplate.floorId;
            save.verticesTop = dataTemplate.verticesTop;
            save.vertexTop = dataTemplate.vertexTop;
            LoadData();

        }



        isAdd = false;
        isDelete = false;
        isRender = false;
        isRunning = false;
        IsClear = true;



    }

    public async void SaveData()
    {
        FloorRoofBuilderSaveData save = new FloorRoofBuilderSaveData();

        int[] a = new int[dimension * dimension];
        await Task.Run(() =>
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = floorType[i];
            }
            // save.floorType = a;

        });
        FloorRoofBuilderSaveData.instance.floorType = a;
        a = new int[1];
        a[0] = vertexCount[0];
        FloorRoofBuilderSaveData.instance.vertexCount = a;

        a = new int[1];
        a[0] = floorCount[0];
        FloorRoofBuilderSaveData.instance.floorCount = a;

        a = new int[FloorMaterials.Length];
        await Task.Run(() =>
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = floorUsed[i];
            }
            // save.floorUsed = a;
        });
        FloorRoofBuilderSaveData.instance.floorUsed = a;

        a = new int[dimension * dimension];
        await Task.Run(() =>
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = floorId[i];
            }
            // save.floorId = a;

        });
        FloorRoofBuilderSaveData.instance.floorId = a;

        Int4x2[] b = new Int4x2[dimension * dimension];
        await Task.Run(() =>
        {
            for (int i = 0; i < b.Length; i++)
            {
                b[i].a1 = verticesTop[i].c0.x;
                b[i].a2 = verticesTop[i].c0.y;
                b[i].a3 = verticesTop[i].c0.z;
                b[i].a4 = verticesTop[i].c0.w;

                b[i].b1 = verticesTop[i].c1.x;
                b[i].b2 = verticesTop[i].c1.y;
                b[i].b3 = verticesTop[i].c1.z;
                b[i].b4 = verticesTop[i].c1.w;


            }
            // save.verticesTop = b;

        });
        FloorRoofBuilderSaveData.instance.verticesTop = b;

        Float3x4[] c = new Float3x4[dimension * dimension];
        await Task.Run(() =>
        {
            for (int i = 0; i < c.Length; i++)
            {
                c[i].a1 = vertexTop[i].c0.x;
                c[i].a2 = vertexTop[i].c0.y;
                c[i].a3 = vertexTop[i].c0.z;

                c[i].b1 = vertexTop[i].c1.x;
                c[i].b2 = vertexTop[i].c1.y;
                c[i].b3 = vertexTop[i].c1.z;

                c[i].c1 = vertexTop[i].c2.x;
                c[i].c2 = vertexTop[i].c2.y;
                c[i].c3 = vertexTop[i].c2.z;


                c[i].d1 = vertexTop[i].c3.x;
                c[i].d2 = vertexTop[i].c3.y;
                c[i].d3 = vertexTop[i].c3.z;

            }
            // save.vertexTop = c;

        });
        FloorRoofBuilderSaveData.instance.vertexTop = c;
        if (!Loaded)
        {
            FloorRoofBuilderSaveData.instance.Save<FloorRoofBuilderSaveData>();
            Loaded = true;
            StartCoroutine(CheckStateLoadedRequirement());

        }
        isSaving = false;
        // SaveGame.Save<FloorRoofBuilderSaveData>(saveFileName, save);


    }
    private IEnumerator CheckStateLoadedRequirement()
    {
        int numThisState = 2;
        while (!(StateLoaded.Loaded + 1 == numThisState))
        {
            yield return null;
            Debug.Log("check loaded now");
        }
        Debug.Log("check loaded kill");

        StateLoaded.Loaded++;

    }

    public async void LoadData()
    {

        FloorRoofBuilderSaveData save = FloorRoofBuilderSaveData.instance;

        await Task.Run(() =>
        {
            for (int i = 0; i < save.floorType.Length; i++)
            {
                floorType[i] = save.floorType[i];
            }

        });

        vertexCount[0] = save.vertexCount[0];


        floorCount[0] = save.floorCount[0];

        await Task.Run(() =>
        {
            for (int i = 0; i < FloorMaterials.Length; i++)
            {
                if (i < save.floorUsed.Length)
                {
                    floorUsed[i] = save.floorUsed[i];
                }
            }

        });

        await Task.Run(() =>
        {
            for (int i = 0; i < save.floorId.Length; i++)
            {
                floorId[i] = save.floorId[i];
            }

        });

        await Task.Run(() =>
        {
            for (int i = 0; i < save.verticesTop.Length; i++)
            {
                verticesTop[i] = new int4x2(
                    new int4(save.verticesTop[i].a1,
                    save.verticesTop[i].a2,
                    save.verticesTop[i].a3,
                    save.verticesTop[i].a4),
                    new int4(save.verticesTop[i].b1,
                    save.verticesTop[i].b2,
                    save.verticesTop[i].b3,
                    save.verticesTop[i].b4)
                    );
            }


        });

        await Task.Run(() =>
        {
            for (int i = 0; i < save.vertexTop.Length; i++)
            {
                vertexTop[i] = new float3x4(
                    new float3(save.vertexTop[i].a1, save.vertexTop[i].a2, save.vertexTop[i].a3),
                    new float3(save.vertexTop[i].b1, save.vertexTop[i].b2, save.vertexTop[i].b3),
                    new float3(save.vertexTop[i].c1, save.vertexTop[i].c2, save.vertexTop[i].c3),
                    new float3(save.vertexTop[i].d1, save.vertexTop[i].d2, save.vertexTop[i].d3));
            }

            for (int i = 0; i < floorType.Length; i++)
            {
                if (floorType[i] > 0)
                {
                    int IDBlock1 = i + dimension - 1;
                    int IDBlock2 = i + dimension;
                    int IDBlock3 = i + dimension + 1;
                    int IDBlock4 = i + 1;
                    int IDBlock5 = i - dimension + 1;
                    int IDBlock6 = i - dimension;
                    int IDBlock7 = i - dimension - 1;
                    int IDBlock8 = i - 1;

                    if (floorType[IDBlock8] < 1)
                    {
                        //     // block1    | block2 +z | block3
                        //     // ------------------------
                        //     // -x block8 | Target    | block4 +x
                        //     // ------------------------
                        //     // block7    | block6 -z | block5
                        cornerCount[0]++;

                    }
                    if (floorType[IDBlock2] < 1)
                    {
                        //     // block1    | block2 +z | block3
                        //     // ------------------------
                        //     // -x block8 | Target    | block4 +x
                        //     // ------------------------
                        //     // block7    | block6 -z | block5
                        cornerCount[0]++;

                    }
                    if (floorType[IDBlock4] < 1)
                    {
                        //     // block1    | block2 +z | block3
                        //     // ------------------------
                        //     // -x block8 | Target    | block4 +x
                        //     // ------------------------
                        //     // block7    | block6 -z | block5
                        cornerCount[0]++;

                    }
                    if (floorType[IDBlock6] < 1)
                    {
                        //     // block1    | block2 +z | block3
                        //     // ------------------------
                        //     // -x block8 | Target    | block4 +x
                        //     // ------------------------
                        //     // block7    | block6 -z | block5
                        cornerCount[0]++;

                    }
                }
            }

        });
        isLoad = true;
        StartCoroutine(RenderLoad());


    }
    public void Run()
    {
        runBuilderCoroutine = StartCoroutine(RunBuilder());
        Debug.Log("run roof floor");


    }
    public void Stop()
    {
        StartCoroutine(SafeStop());
    }
    private IEnumerator SafeStop()
    {
        while (true)
        {
            if (!isRunning && Loaded)
            {
                Debug.Log("stop floor roof");
                if (runBuilderCoroutine != null)
                {
                    StopCoroutine(runBuilderCoroutine);

                }

                break;
            }
            Debug.Log("stop floor roof");
            yield return null;
        }

    }
    private IEnumerator RenderLoad()
    {
        while (true)
        {

            if (isLoad)
            {

                AddHandle.Complete();
                DeleteHandle.Complete();
                isAdd = false;
                isDelete = false;
                isLoad = false;

                RenderID = new NativeArray<int>(6 * floorCount[0], Allocator.Persistent);
                RenderType = new NativeArray<int>(6 * floorCount[0], Allocator.Persistent);
                RenderPosa = new NativeArray<float3>(vertexCount[0], Allocator.Persistent);
                cornerRenderTris = new NativeArray<int>(6 * cornerCount[0], Allocator.Persistent);
                cornerRenderVerts = new NativeArray<float3>(4 * cornerCount[0], Allocator.Persistent);
                tempVerticesTop = new NativeArray<int4>(dimension * dimension, Allocator.Persistent);
                FloorID = new NativeArray<int>(floorCount[0], Allocator.Persistent);
                floorUsedIndex = new NativeArray<int>(FloorMaterials.Length, Allocator.Persistent);
                floorUsedCount = new NativeArray<int>(1, Allocator.Persistent); /* just 1 */

                RenderRoofFloorJob renderFloorJob = new RenderRoofFloorJob
                {
                    floorType = floorType,
                    vertexCount = vertexCount,
                    floorId = floorId,
                    FloorID = FloorID,
                    verticesTop = verticesTop,
                    RenderID = RenderID,
                    RenderType = RenderType,
                    RenderPos = RenderPosa,
                    vertexTop = vertexTop,
                    floorUsedCount = floorUsedCount,
                    floorUsed = floorUsed,
                    floorUsedIndex = floorUsedIndex,
                    tempVerticesTop = tempVerticesTop,
                    dimension = dimension,
                    cornerRenderTris = cornerRenderTris,
                    cornerRenderVerts = cornerRenderVerts
                };
                RenderHandle = renderFloorJob.Schedule();

                Debug.Log("add kelar");
                isRender = true;
            }

            if (isRender && RenderHandle.IsCompleted)
            {
                isRender = false;


                RenderHandle.Complete();
                Build();


                Debug.Log("render kelar");
                Stop();
                break;
            }
            Debug.Log("dont stop roof");

            yield return null;
        }
    }

    // Update is called once per frame
    private IEnumerator RunBuilder()
    {
        while (true)
        {
            if (IsClear && !isRunning && PlaceButton.Instance.contentID > 0 && PlaceButton.Instance.isExecute && PlaceButton.Instance.mode == "FloorRoofBuilder")
            {
                float3x2 ids = new float3x2(floorPlatform.InverseTransformPoint(RoofFloorGuide.Instance.firstSnapPos), floorPlatform.InverseTransformPoint(RoofFloorGuide.Instance.lastSnapPos));
                AddRoofFloorJob addFloorJob = new AddRoofFloorJob
                {
                    ids = ids,
                    dimension = dimension,
                    floorType = floorType,
                    vertexCount = vertexCount,
                    verticesTop = verticesTop,
                    vertexTop = vertexTop,
                    floorCount = floorCount,
                    floorId = floorId,
                    floorUsed = floorUsed,
                    floorMaterial = PlaceButton.Instance.contentID,
                    corner = cornerCount
                };
                AddHandle = addFloorJob.Schedule();
                PlaceButton.Instance.isExecute = false;
                isAdd = true;
                isRunning = true;
                IsClear = false;
                Debug.Log($"x {ids.c0.x} z {ids.c0.z}");
            }
            if (IsClear && !isRunning && PlaceButton.Instance.contentID == 0 && PlaceButton.Instance.isExecute && PlaceButton.Instance.mode == "FloorRoofBuilder")
            {
                float3x2 ids = new float3x2(floorPlatform.InverseTransformPoint(RoofFloorGuide.Instance.firstSnapPos), floorPlatform.InverseTransformPoint(RoofFloorGuide.Instance.lastSnapPos));


                vertexData = new NativeArray<int4>(3, Allocator.Persistent);
                DeleteRoofFloorJob deleteFloorJob = new DeleteRoofFloorJob
                {
                    ids = ids,
                    dimension = dimension,
                    floorType = floorType,
                    verticesTop = verticesTop,
                    vertexTop = vertexTop,
                    vertexCount = vertexCount,
                    floorCount = floorCount,
                    floorId = floorId,
                    vertexData = vertexData,
                    floorUsed = floorUsed,
                    floorMaterial = PlaceButton.Instance.contentID,
                    corner = cornerCount
                };
                DeleteHandle = deleteFloorJob.Schedule();
                PlaceButton.Instance.isExecute = false;
                isDelete = true;
                isRunning = true;
                IsClear = false;

            }

            if (isAdd && AddHandle.IsCompleted || isDelete && DeleteHandle.IsCompleted || isLoad)
            {

                AddHandle.Complete();
                if (isAdd)
                {
                    isAdd = false;
                    SaveData();
                    isSaving = true;
                    while (true)
                    {
                        if (!isSaving)
                        {
                            int floorCount = RoofFloorGuide.Instance.floorCount;
                            int askariPrice = RoofFloorGuide.Instance.floorData.price * floorCount;
                            int moriumPrice = RoofFloorGuide.Instance.floorData.priceMorium * floorCount;
                            EconomyNotif.Instance.Append("askari", "Askari", askariPrice, false);
                            EconomyNotif.Instance.Append("morium", "Morium", moriumPrice, false);
                            EconomyCurrency.Instance.DecreaseAskari(askariPrice);
                            EconomyCurrency.Instance.DecreaseMorium(moriumPrice);
                            break;
                        }
                        yield return null;
                    }

                }
                DeleteHandle.Complete();
                isDelete = false;
                isLoad = false;

                RenderID = new NativeArray<int>(6 * floorCount[0], Allocator.Persistent);
                RenderType = new NativeArray<int>(6 * floorCount[0], Allocator.Persistent);
                RenderPosa = new NativeArray<float3>(vertexCount[0], Allocator.Persistent);
                cornerRenderTris = new NativeArray<int>(6 * cornerCount[0], Allocator.Persistent);
                cornerRenderVerts = new NativeArray<float3>(4 * cornerCount[0], Allocator.Persistent);
                tempVerticesTop = new NativeArray<int4>(dimension * dimension, Allocator.Persistent);
                FloorID = new NativeArray<int>(floorCount[0], Allocator.Persistent);
                floorUsedIndex = new NativeArray<int>(FloorMaterials.Length, Allocator.Persistent);
                floorUsedCount = new NativeArray<int>(1, Allocator.Persistent); /* just 1 */

                RenderRoofFloorJob renderFloorJob = new RenderRoofFloorJob
                {
                    floorType = floorType,
                    vertexCount = vertexCount,
                    floorId = floorId,
                    FloorID = FloorID,
                    verticesTop = verticesTop,
                    RenderID = RenderID,
                    RenderType = RenderType,
                    RenderPos = RenderPosa,
                    vertexTop = vertexTop,
                    floorUsedCount = floorUsedCount,
                    floorUsed = floorUsed,
                    floorUsedIndex = floorUsedIndex,
                    tempVerticesTop = tempVerticesTop,
                    dimension = dimension,
                    cornerRenderTris = cornerRenderTris,
                    cornerRenderVerts = cornerRenderVerts
                };
                RenderHandle = renderFloorJob.Schedule();

                Debug.Log("add kelar");
                isRender = true;
            }

            if (isRender && RenderHandle.IsCompleted)
            {
                isRender = false;


                RenderHandle.Complete();
                Build();


                Debug.Log("render kelar");
            }

            yield return null;
        }



    }
    private async void Build()
    {
        var mesh = new Mesh();
        Vector3[] verts = new Vector3[RenderPosa.Length];
        Vector2[] uvs = new Vector2[RenderPosa.Length];
        subData = new FloorMaterialData[floorUsedCount[0]];
        int[] tris = new int[RenderID.Length];
        int[] corTris = new int[cornerRenderTris.Length];
        Vector3[] corVerts = new Vector3[cornerRenderVerts.Length];

        await Task.Run(() =>
        {

            for (int i = 0; i < subData.Length; i++)
            {
                subData[i] = new FloorMaterialData();
                subData[i].triangles = new List<int>();
                subData[i].subMeshId = -1;
            }
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = RenderPosa[i];
                uvs[i] = new Vector2(RenderPosa[i].x, RenderPosa[i].z);

            }
            for (int i = 0; i < tris.Length; i++)
            {
                tris[i] = RenderID[i];
                UnityEngine.Debug.Log($"floorused index {RenderType[i] - 1}");
                // UnityEngine.Debug.Log($"subcount {floorUsedCount[0]}");
                // UnityEngine.Debug.Log($"renderType - 1 {RenderType[i] - 1}");

                subData[floorUsedIndex[RenderType[i] - 1]].triangles.Add(RenderID[i]);
                subData[floorUsedIndex[RenderType[i] - 1]].subMeshId = floorUsedIndex[RenderType[i] - 1];
            }
            for (int i = 0; i < corTris.Length; i++)
            {
                corTris[i] = cornerRenderTris[i];


            }
            for (int i = 0; i < corVerts.Length; i++)
            {
                corVerts[i] = cornerRenderVerts[i];


            }
        });

        // string s = "";
        // int numtris = 0;
        // for (int i = 0; i < tris.Length; i++)
        // {

        //     s += $"{tris[i]}, ";
        //     numtris++;

        //     if (numtris == 6)
        //     {
        //         s += " | ";
        //         numtris = 0;
        //     }

        // }
        // UnityEngine.Debug.Log($" tris : {s}");
        // string j = "";


        // for (int i = 0; i < floorUsed.Length; i++)
        // {
        //     j += $"{floorUsed[i]}, ";

        // }
        Material[] CurrentMaterials = new Material[floorUsedCount[0]];
        int curMatNum = 0;
        for (int i = 0; i < FloorMaterials.Length; i++)
        {
            if (floorUsed[i] > 0)
            {
                CurrentMaterials[curMatNum] = FloorMaterials[i];
                curMatNum++;
            }
        }

        mesh.vertices = verts;
        // VertexDebug.vertex = verts;
        // mesh.triangles = tris;
        mesh.subMeshCount = floorUsedCount[0];
        for (int i = 0; i < subData.Length; i++)
        {
            mesh.SetTriangles(subData[i].triangles.ToArray(), subData[i].subMeshId);
        }
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        FloorMF.mesh.Clear();
        BottomFloorMF.mesh.Clear();

        FloorMF.mesh = mesh;
        FloorMR.sharedMaterials = CurrentMaterials;
        BottomFloorMF.mesh.vertices = verts;
        BottomFloorMF.mesh.triangles = tris;
        BottomFloorMF.mesh.RecalculateNormals();
        BottomFloorMF.mesh.RecalculateBounds();
        Mesh meshcor = new Mesh();
        meshcor.vertices = corVerts;
        meshcor.triangles = corTris;
        meshcor.RecalculateBounds();
        meshcor.RecalculateNormals();
        cornerFloorMF.mesh = meshcor;

        isRunning = false;
        IsClear = true;
        if (PlaceButton.Instance != null)
        {
            PlaceButton.Instance.isRun = false;

        }
        if (!Loaded)
        {
            Loaded = true;
            StartCoroutine(CheckStateLoadedRequirement());


        }
        AutoDelete.Instance.isDeleteTouched = false;

    }
}
