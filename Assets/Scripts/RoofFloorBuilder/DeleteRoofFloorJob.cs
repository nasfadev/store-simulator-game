using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

using System.Collections;
using System.Collections.Generic;
[BurstCompile]
public struct DeleteRoofFloorJob : IJob
{
    public float3x2 ids;
    public int dimension;
    public int floorMaterial;
    public NativeArray<int> floorType;
    public NativeArray<int> vertexCount;
    public NativeArray<int4x2> verticesTop;
    public NativeArray<float3x4> vertexTop;
    public NativeArray<int> floorCount;
    public NativeArray<int> floorUsed;

    public NativeArray<int> floorId;
    public NativeArray<int4> vertexData;
    public NativeArray<int> corner;


    public void Execute()
    {
        int minX = (int)(math.ceil(ids.c0.x <= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
        int maxX = (int)(math.ceil(ids.c0.x >= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
        int minY = (int)(math.ceil(ids.c0.z <= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
        int maxY = (int)(math.ceil(ids.c0.z >= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
        for (int yc = minY; yc < maxY + 1; yc++)
        {
            for (int xc = minX; xc < maxX + 1; xc++)
            {

                int index = ((int)((dimension * yc) + xc));
                // float3 idx = math.ceil(ids) - 1;
                if (xc <= 0 || yc <= 0 || xc >= dimension - 1 || yc >= dimension - 1)
                {
                    continue;
                }
                if (floorType[index] == 0)
                {
                    continue;

                }

                // floorType[index] = floorMaterial;

                int IDBlock1 = index + dimension - 1;
                int IDBlock2 = index + dimension;
                int IDBlock3 = index + dimension + 1;
                int IDBlock4 = index + 1;
                int IDBlock5 = index - dimension + 1;
                int IDBlock6 = index - dimension;
                int IDBlock7 = index - dimension - 1;
                int IDBlock8 = index - 1;
                bool block1 = floorType[IDBlock1] == 0 ? false : true;
                bool block2 = floorType[IDBlock2] == 0 ? false : true;
                bool block3 = floorType[IDBlock3] == 0 ? false : true;
                bool block4 = floorType[IDBlock4] == 0 ? false : true;
                bool block5 = floorType[IDBlock5] == 0 ? false : true;
                bool block6 = floorType[IDBlock6] == 0 ? false : true;
                bool block7 = floorType[IDBlock7] == 0 ? false : true;
                bool block8 = floorType[IDBlock8] == 0 ? false : true;

                // int vertsID1 = 0;
                // int vertsID2 = 0;
                // int vertsID3 = 0;
                // int vertsID4 = 0;
                // int vertsRefID1 = 0;
                // int vertsRefID2 = 0;
                // int vertsRefID3 = 0;
                // int vertsRefID4 = 0;

                // int indexRange = 0;
                // float3 vertsPos1 = float3.zero;
                // float3 vertsPos2 = float3.zero;
                // float3 vertsPos3 = float3.zero;
                // float3 vertsPos4 = float3.zero;
                // int vertexCounts = vertexCount[0];

                // bool next = true;


                if (vertexTop[index].c0.x > -1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    UnityEngine.Debug.Log($"vertex1 aman");

                    // vertex1
                    if (!block6 && !block7 && !block8)
                    {
                        UnityEngine.Debug.Log($"vertex1 delete");

                        vertexCount[0]--;
                    }
                    else
                    {
                        vertexData[0] = new int4(floorId[IDBlock6], IDBlock6, 2, block6 ? 1 : 0);
                        vertexData[1] = new int4(floorId[IDBlock7], IDBlock7, 3, block7 ? 1 : 0);
                        vertexData[2] = new int4(floorId[IDBlock8], IDBlock8, 4, block8 ? 1 : 0);
                        float3x4 x = vertexTop[index];
                        vertexData.Sort(new FloorIDSort());
                        Delete(index, x.c0, vertexData);
                    }

                }
                if (vertexTop[index].c1.x > -1)
                {
                    UnityEngine.Debug.Log($"vertex2 aman");

                    // vertex1
                    if (!block8 && !block1 && !block2)
                    {
                        UnityEngine.Debug.Log($"vertex2 delete");

                        vertexCount[0]--;
                    }
                    else
                    {
                        vertexData[0] = new int4(floorId[IDBlock8], IDBlock8, 3, block8 ? 1 : 0);
                        vertexData[1] = new int4(floorId[IDBlock1], IDBlock1, 4, block1 ? 1 : 0);
                        vertexData[2] = new int4(floorId[IDBlock2], IDBlock2, 1, block2 ? 1 : 0);
                        float3x4 x = vertexTop[index];
                        vertexData.Sort(new FloorIDSort());
                        Delete(index, x.c1, vertexData);
                    }
                    // next = true;
                    // vertex2



                }
                if (vertexTop[index].c2.x > -1)
                {
                    UnityEngine.Debug.Log($"vertex3 aman");

                    if (!block2 && !block3 && !block4)
                    {
                        UnityEngine.Debug.Log($"vertex3 delete");

                        vertexCount[0]--;
                    }
                    else
                    {
                        vertexData[0] = new int4(floorId[IDBlock2], IDBlock2, 4, block2 ? 1 : 0);
                        vertexData[1] = new int4(floorId[IDBlock3], IDBlock3, 1, block3 ? 1 : 0);
                        vertexData[2] = new int4(floorId[IDBlock4], IDBlock4, 2, block4 ? 1 : 0);
                        float3x4 x = vertexTop[index];
                        vertexData.Sort(new FloorIDSort());
                        Delete(index, x.c2, vertexData);
                    }
                    // vertex3

                }
                if (vertexTop[index].c3.x > -1)
                {
                    UnityEngine.Debug.Log($"vertex4 aman");

                    if (!block4 && !block5 && !block6)
                    {
                        UnityEngine.Debug.Log($"vertex4 delete");

                        vertexCount[0]--;
                    }
                    else
                    {
                        vertexData[0] = new int4(floorId[IDBlock4], IDBlock4, 1, block4 ? 1 : 0);
                        vertexData[1] = new int4(floorId[IDBlock5], IDBlock5, 2, block5 ? 1 : 0);
                        vertexData[2] = new int4(floorId[IDBlock6], IDBlock6, 3, block6 ? 1 : 0);
                        float3x4 x = vertexTop[index];
                        vertexData.Sort(new FloorIDSort());
                        Delete(index, x.c3, vertexData);
                    }

                    // vertex4


                }










                // verticesTop[index] = new int4x2(
                //          new int4(vertsID1, vertsID2, vertsID3, vertsID4),
                //          new int4(vertsRefID1, vertsRefID2, vertsRefID3, vertsRefID4));


                // vertexTop[index] = new float3x4(
                //     vertsPos1, vertsPos2, vertsPos3, vertsPos4);

                // floorCount[0] += 1;
                // floorId[index] = floorCount[0] - 1;


                for (int i = 0; i < floorId.Length; i++)
                {
                    if (floorId[i] > floorId[index])
                    {
                        floorId[i]--;
                    }
                }
                floorUsed[floorType[index] - 1] -= 1;
                floorId[index] = -1;
                floorType[index] = 0;
                floorCount[0]--;
            }
        }
        corner[0] = 0;
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
                    corner[0]++;

                }
                if (floorType[IDBlock2] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    corner[0]++;

                }
                if (floorType[IDBlock4] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    corner[0]++;

                }
                if (floorType[IDBlock6] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    corner[0]++;

                }
            }
        }

    }

    private void Delete(int index, float3 vertexPosTarget, NativeArray<int4> data)
    {
        bool isAdd = true;
        int moveId = 0;
        int movePos = 0;

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].w == 1)
            {
                if (data[i].z == 1)
                {
                    int id = data[i].y;
                    if (isAdd)
                    {
                        float3x4 d = vertexTop[id];
                        float3x4 x = vertexTop[index];
                        vertexTop[id] = new float3x4(vertexPosTarget, d.c1, d.c2, d.c3);

                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(-1, f.c0.y, f.c0.z, f.c0.w),
                            new int4(-1, f.c1.y, f.c1.z, f.c1.w));
                        moveId = id;
                        movePos = 1;
                        isAdd = false;
                        UnityEngine.Debug.Log($"vertex4 block4 added");

                    }
                    else
                    {
                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(movePos, f.c0.y, f.c0.z, f.c0.w),
                            new int4(moveId, f.c1.y, f.c1.z, f.c1.w));
                        UnityEngine.Debug.Log($"vertex4 block4 {moveId}");

                    }
                }
                else if (data[i].z == 2)
                {
                    int id = data[i].y;

                    if (isAdd)
                    {
                        float3x4 d = vertexTop[id];
                        float3x4 x = vertexTop[index];
                        vertexTop[id] = new float3x4(d.c0, vertexPosTarget, d.c2, d.c3);

                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, -1, f.c0.z, f.c0.w),
                            new int4(f.c1.x, -1, f.c1.z, f.c1.w));
                        moveId = id;
                        movePos = 2;
                        isAdd = false;
                        UnityEngine.Debug.Log($"vertex4 block5 added");

                    }
                    else
                    {
                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, movePos, f.c0.z, f.c0.w),
                            new int4(f.c1.x, moveId, f.c1.z, f.c1.w));
                        UnityEngine.Debug.Log($"vertex4 block5 {moveId}");

                    }
                }
                else if (data[i].z == 3)
                {
                    int id = data[i].y;
                    if (isAdd)
                    {
                        float3x4 d = vertexTop[id];
                        float3x4 x = vertexTop[index];
                        vertexTop[id] = new float3x4(d.c0, d.c1, vertexPosTarget, d.c3);

                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, f.c0.y, -1, f.c0.w),
                            new int4(f.c1.x, f.c1.y, -1, f.c1.w));
                        moveId = id;
                        movePos = 3;
                        isAdd = false;
                        UnityEngine.Debug.Log($"vertex4 block6 added");

                    }
                    else
                    {
                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, f.c0.y, movePos, f.c0.w),
                            new int4(f.c1.x, f.c1.y, moveId, f.c1.w));
                        UnityEngine.Debug.Log($"vertex4 block6 {moveId}");

                    }
                }
                else if (data[i].z == 4)
                {
                    int id = data[i].y;

                    if (isAdd)
                    {
                        float3x4 d = vertexTop[id];
                        float3x4 x = vertexTop[index];
                        vertexTop[id] = new float3x4(d.c0, d.c1, d.c2, vertexPosTarget);

                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, f.c0.y, f.c0.z, -1),
                            new int4(f.c1.x, f.c1.y, f.c1.z, -1));
                        moveId = id;
                        movePos = 4;
                        isAdd = false;
                        UnityEngine.Debug.Log($"vertex3 block2 added");

                    }
                    else
                    {
                        int4x2 f = verticesTop[id];

                        verticesTop[id] = new int4x2(
                            new int4(f.c0.x, f.c0.y, f.c0.z, movePos),
                            new int4(f.c1.x, f.c1.y, f.c1.z, moveId));
                        UnityEngine.Debug.Log($"vertex3 block2 {moveId}");

                    }
                }

            }

        }

    }
    struct FloorIDSort : IComparer<int4>
    {
        public int Compare(int4 a, int4 b)
        {
            return a.x.CompareTo(b.x);
        }
    }
}
