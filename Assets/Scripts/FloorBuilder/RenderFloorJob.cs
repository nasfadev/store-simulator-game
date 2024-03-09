using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public struct RenderFloorJob : IJob
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
