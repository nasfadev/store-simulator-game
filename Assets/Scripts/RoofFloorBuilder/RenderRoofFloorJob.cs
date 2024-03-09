using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public struct RenderRoofFloorJob : IJob
{
    public NativeArray<int> floorType;
    public NativeArray<int> vertexCount;
    public NativeArray<int> floorId;
    public NativeArray<int> FloorID;
    public NativeArray<int4x2> verticesTop;
    public NativeArray<int> RenderID;
    public NativeArray<int> RenderType;
    public NativeArray<int> floorUsedCount;
    public NativeArray<int> floorUsed;
    public NativeArray<int> floorUsedIndex;
    public NativeArray<int4> tempVerticesTop;
    public NativeArray<float3> RenderPos;
    public NativeArray<float3x4> vertexTop;
    public NativeArray<int> cornerRenderTris;
    public NativeArray<float3> cornerRenderVerts;
    public int dimension;

    public void Execute()
    {

        int numID = 0;
        for (int i = 0; i < floorType.Length; i++)
        {
            if (floorType[i] > 0 && floorId[i] >= 0)
            {
                FloorID[floorId[i]] = i;
                UnityEngine.Debug.Log($"floor type {floorType[i]}");
                UnityEngine.Debug.Log($"floor id {floorId[i]}");


            }

        }
        int numTemp = 0;
        int numVerts = 0;
        int numPos = 0;
        foreach (int i in FloorID)
        {
            // UnityEngine.Debug.Log($"floor");

            int id1 = verticesTop[i].c0.x;
            int id2 = verticesTop[i].c0.y;
            int id3 = verticesTop[i].c0.z;
            int id4 = verticesTop[i].c0.w;

            int idR1 = verticesTop[i].c1.x;
            int idR2 = verticesTop[i].c1.y;
            int idR3 = verticesTop[i].c1.z;
            int idR4 = verticesTop[i].c1.w;

            int idT1 = -1;
            int idT2 = -1;
            int idT3 = -1;
            int idT4 = -1;


            if (idR1 < 0)
            {
                int4 d = tempVerticesTop[i];
                tempVerticesTop[i] = new int4(numVerts, d.y, d.z, d.w);
                idT1 = numVerts;
                numVerts++;
            }
            else
            {
                idT1 = GetVertId(tempVerticesTop[idR1], id1);
            }
            // ---------------------------------------------
            if (idR2 < 0)
            {
                int4 d = tempVerticesTop[i];
                tempVerticesTop[i] = new int4(d.x, numVerts, d.z, d.w);
                idT2 = numVerts;
                numVerts++;

            }
            else
            {
                idT2 = GetVertId(tempVerticesTop[idR2], id2);
            }
            // ---------------------------------------------
            if (idR3 < 0)
            {
                int4 d = tempVerticesTop[i];
                tempVerticesTop[i] = new int4(d.x, d.y, numVerts, d.w);
                idT3 = numVerts;
                numVerts++;
            }
            else
            {
                idT3 = GetVertId(tempVerticesTop[idR3], id3);
            }
            // ---------------------------------------------
            if (idR4 < 0)
            {
                int4 d = tempVerticesTop[i];
                tempVerticesTop[i] = new int4(d.x, d.y, d.z, numVerts);
                idT4 = numVerts;
                numVerts++;
            }
            else
            {
                idT4 = GetVertId(tempVerticesTop[idR4], id4);
            }
            numTemp++;

            RenderID[numID] = idT1;
            RenderType[numID] = floorType[i];
            numID++;
            RenderID[numID] = idT2;
            RenderType[numID] = floorType[i];
            numID++;
            RenderID[numID] = idT3;
            RenderType[numID] = floorType[i];
            numID++;
            RenderID[numID] = idT3;
            RenderType[numID] = floorType[i];
            numID++;
            RenderID[numID] = idT4;
            RenderType[numID] = floorType[i];
            numID++;
            RenderID[numID] = idT1;
            RenderType[numID] = floorType[i];
            numID++;

            // RenderID[numID] = idR1 < 0 ? id1 : GetVertId(verticesTop[idR1].c0, id1);
            // RenderType[numID] = floorType[i];
            // numID++;
            // RenderID[numID] = idR2 < 0 ? id2 : GetVertId(verticesTop[idR2].c0, id2);
            // RenderType[numID] = floorType[i];
            // numID++;
            // RenderID[numID] = idR3 < 0 ? id3 : GetVertId(verticesTop[idR3].c0, id3);
            // RenderType[numID] = floorType[i];
            // numID++;
            // RenderID[numID] = idR3 < 0 ? id3 : GetVertId(verticesTop[idR3].c0, id3);
            // RenderType[numID] = floorType[i];
            // numID++;
            // RenderID[numID] = idR4 < 0 ? id4 : GetVertId(verticesTop[idR4].c0, id4);
            // RenderType[numID] = floorType[i];
            // numID++;
            // RenderID[numID] = idR1 < 0 ? id1 : GetVertId(verticesTop[idR1].c0, id1);
            // RenderType[numID] = floorType[i];
            // numID++;

            // NativeArray<float3> pos = new NativeArray<float3>(4, Allocator.Temp);
            UnityEngine.Debug.Log($"render id dalam job render : {idT1}, {idT2}, {idT3}, {idT4}");
            float3 pos1 = vertexTop[i].c0;
            float3 pos2 = vertexTop[i].c1;
            float3 pos3 = vertexTop[i].c2;
            float3 pos4 = vertexTop[i].c3;
            if (pos1.x >= 0f)
            {
                RenderPos[numPos] = pos1;
                numPos++;
            }
            if (pos2.x >= 0f)
            {
                RenderPos[numPos] = pos2;
                numPos++;
            }
            if (pos3.x >= 0f)
            {
                RenderPos[numPos] = pos3;
                numPos++;
            }
            if (pos4.x >= 0f)
            {
                RenderPos[numPos] = pos4;
                numPos++;
            }
            UnityEngine.Debug.Log($"renderpos : {pos1}, {pos2}, {pos3}, {pos4}");


            // for (int j = 0; j < pos.Length; j++)
            // {
            //     if (pos[j].x >= 0f)
            //     {

            //         RenderPos[numPos] = pos[j];
            //         numPos++;

            //     }
            // }

            // pos.Dispose();


        }

        int indexNum = 0;
        for (int i = 0; i < floorUsed.Length; i++)
        {
            if (floorUsed[i] > 0)
            {

                floorUsedIndex[i] = indexNum;
                floorUsedCount[0]++;
                // UnityEngine.Debug.Log($"floor used {indexNum}");
                // UnityEngine.Debug.Log($"floor used index {floorUsedIndex[1]}");

                indexNum++;


            }
        }
        int corvert = 0;
        int cortris = 0;
        int numId = 0;
        float idsCek = 0;
        float tall = .11f;
        for (int i = 0; i < floorType.Length; i++)
        {
            idsCek = idsCek == 30 ? 0 : idsCek;
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


                float2 ids = new float2(i % dimension - 1, math.floor(i / dimension) - 1);

                if (floorType[IDBlock8] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    // Vector3(0, 0, 1),new Vector3(0, tall, 1), new Vector3(0, tall, 0), new Vector3(0, 0, 0)
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, 0, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, tall, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, tall, 0 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, 0, 0 + ids.y);
                    corvert++;

                    int vert1 = 0 + numId;
                    int vert2 = 1 + numId;
                    int vert3 = 2 + numId;
                    int vert4 = 3 + numId;
                    numId += 4;

                    cornerRenderTris[cortris] = vert1;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert4;
                    cortris++;
                    cornerRenderTris[cortris] = vert1;
                    cortris++;



                }
                if (floorType[IDBlock2] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    // new Vector3(0, 0, 1),new Vector3(0, tall, 1), new Vector3(1, tall, 1), new Vector3(1, 0, 1) };
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, 0, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, tall, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, tall, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, 0, 1 + ids.y);
                    corvert++;

                    int vert1 = 0 + numId;
                    int vert2 = 1 + numId;
                    int vert3 = 2 + numId;
                    int vert4 = 3 + numId;
                    numId += 4;
                    // int[] zPlusTris = new int[] { vert4, vert3, vert2, vert2, vert1, vert4 };

                    cornerRenderTris[cortris] = vert4;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert1;
                    cortris++;
                    cornerRenderTris[cortris] = vert4;
                    cortris++;

                }
                if (floorType[IDBlock4] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    // { new Vector3(1, 0, 1),new Vector3(1, tall, 1), new Vector3(1, tall, 0), new Vector3(1, 0, 0) };

                    cornerRenderVerts[corvert] = new float3(1 + ids.x, 0, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, tall, 1 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, tall, 0 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, 0, 0 + ids.y);
                    corvert++;

                    int vert1 = 0 + numId;
                    int vert2 = 1 + numId;
                    int vert3 = 2 + numId;
                    int vert4 = 3 + numId;
                    numId += 4;
                    // int[] zPlusTris = new int[] { vert4, vert3, vert2, vert2, vert1, vert4 };

                    cornerRenderTris[cortris] = vert4;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert1;
                    cortris++;
                    cornerRenderTris[cortris] = vert4;
                    cortris++;

                }
                if (floorType[IDBlock6] < 1)
                {
                    //     // block1    | block2 +z | block3
                    //     // ------------------------
                    //     // -x block8 | Target    | block4 +x
                    //     // ------------------------
                    //     // block7    | block6 -z | block5
                    // { new Vector3(0, 0, 0),new Vector3(0, tall, 0), new Vector3(1, tall, 0), new Vector3(1, 0, 0) };

                    cornerRenderVerts[corvert] = new float3(0 + ids.x, 0, 0 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(0 + ids.x, tall, 0 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, tall, 0 + ids.y);
                    corvert++;
                    cornerRenderVerts[corvert] = new float3(1 + ids.x, 0, 0 + ids.y);
                    corvert++;

                    int vert1 = 0 + numId;
                    int vert2 = 1 + numId;
                    int vert3 = 2 + numId;
                    int vert4 = 3 + numId;
                    numId += 4;

                    cornerRenderTris[cortris] = vert1;
                    cortris++;
                    cornerRenderTris[cortris] = vert2;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert3;
                    cortris++;
                    cornerRenderTris[cortris] = vert4;
                    cortris++;
                    cornerRenderTris[cortris] = vert1;
                    cortris++;
                }
            }
            idsCek++;
        }

    }
    private int GetVertId(int4 verts, int num)
    {

        int result = -1;
        if (num == 1)
        {
            result = verts.x;
        }
        else if (num == 2)
        {
            result = verts.y;
        }
        else if (num == 3)
        {
            result = verts.z;
        }
        else
        {
            result = verts.w;
        }
        return result;
    }


}
