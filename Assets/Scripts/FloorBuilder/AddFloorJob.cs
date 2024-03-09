using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public struct AddFloorJob : IJob
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


    public void Execute()
    {

        int minX = (int)(math.ceil(ids.c0.x <= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
        int maxX = (int)(math.ceil(ids.c0.x >= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
        int minY = (int)(math.ceil(ids.c0.z <= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
        int maxY = (int)(math.ceil(ids.c0.z >= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
        for (int y = minY; y < maxY + 1; y++)
        {
            for (int x = minX; x < maxX + 1; x++)
            {

                int index = ((int)((dimension * y) + x));
                // float3 idx = math.ceil(ids) - 1;
                if (x <= 0 || y <= 0 || x >= dimension - 1 || y >= dimension - 1)
                {
                    continue;
                }
                if (floorType[index] == floorMaterial)
                {
                    continue;

                }
                if (floorType[index] > 0)
                {
                    floorUsed[floorMaterial - 1]++;
                    floorUsed[floorType[index] - 1]--;
                    floorType[index] = floorMaterial;

                    continue;

                }
                float3 idx = new float3(x, 0f, y);

                // int index = ((int)((dimension * idx.z) + idx.x));
                floorUsed[floorMaterial - 1]++;
                floorType[index] = floorMaterial;



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

                int vertsID1 = 0;
                int vertsID2 = 0;
                int vertsID3 = 0;
                int vertsID4 = 0;
                int vertsRefID1 = 0;
                int vertsRefID2 = 0;
                int vertsRefID3 = 0;
                int vertsRefID4 = 0;

                int indexRange = 0;
                float3 vertsPos1 = float3.zero;
                float3 vertsPos2 = float3.zero;
                float3 vertsPos3 = float3.zero;
                float3 vertsPos4 = float3.zero;
                int vertexCounts = vertexCount[0];

                //     // block1    | block2 +z | block3
                //     // ------------------------
                //     // -x block8 | Target    | block4 +x
                //     // ------------------------
                //     // block7    | block6 -z | block5
                // vertex1
                bool next = true;
                if (block6 && next)
                {
                    int id = IDBlock6;
                    int4x2 blockTopID = verticesTop[id];


                    if (blockTopID.c1.y < 0)
                    {
                        vertsPos1 = float3.zero - 10f;
                        vertsID1 = 2;
                        vertsRefID1 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }


                if (block7 && next)
                {
                    int id = IDBlock7;
                    int4x2 blockTopID = verticesTop[id];

                    if (blockTopID.c1.z < 0)
                    {
                        vertsPos1 = float3.zero - 10f;
                        vertsID1 = 3;
                        vertsRefID1 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }

                }

                if (block8 && next)
                {
                    int id = IDBlock8;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.w < 0)
                    {
                        vertsPos1 = float3.zero - 10f;
                        vertsID1 = 4;
                        vertsRefID1 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }

                }
                if (next)

                {
                    vertsPos1 = new float3(0f + idx.x, 0f, 0f + idx.z);
                    vertsID1 = (0 - indexRange) + vertexCounts;
                    vertsRefID1 = -1;
                    vertexCount[0] += 1;


                }




                next = true;
                // vertex2
                if (block8 && next)
                {
                    int id = IDBlock8;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.z < 0)
                    {
                        vertsPos2 = float3.zero - 10f;
                        vertsID2 = 3;
                        vertsRefID2 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }


                if (block1 && next)
                {
                    int id = IDBlock1;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.w < 0)
                    {
                        vertsPos2 = float3.zero - 10f;
                        vertsID2 = 4;
                        vertsRefID2 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }

                }
                if (block2 && next)
                {
                    int id = IDBlock2;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.x < 0)
                    {
                        vertsPos2 = float3.zero - 10f;
                        vertsID2 = 1;
                        vertsRefID2 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }



                }

                if (next)

                {
                    vertsPos2 = new float3(0f + idx.x, 0f, 1f + idx.z);
                    vertsID2 = (1 - indexRange) + vertexCounts;
                    vertsRefID2 = -1;
                    vertexCount[0] += 1;


                }
                next = true;
                // vertex3
                if (block2 && next)
                {
                    int id = IDBlock2;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.w < 0)
                    {
                        vertsPos3 = float3.zero - 10f;
                        vertsID3 = 4;
                        vertsRefID3 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }

                if (block3 && next)
                {
                    int id = IDBlock3;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.x < 0)
                    {
                        vertsPos3 = float3.zero - 10f;
                        vertsID3 = 1;
                        vertsRefID3 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }

                if (block4 && next)
                {
                    int id = IDBlock4;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.y < 0)
                    {
                        vertsPos3 = float3.zero - 10f;
                        vertsID3 = 2;
                        vertsRefID3 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }
                if (next)

                {
                    vertsPos3 = new float3(1f + idx.x, 0f, 1f + idx.z);
                    vertsID3 = (2 - indexRange) + vertexCounts;
                    vertsRefID3 = -1;
                    vertexCount[0] += 1;




                }
                next = true;
                // vertex4
                if (block4 && next)
                {
                    int id = IDBlock4;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.x < 0)
                    {
                        vertsPos4 = float3.zero - 10f;
                        vertsID4 = 1;
                        vertsRefID4 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }
                }

                if (block5 && next)
                {
                    int id = IDBlock5;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.y < 0)
                    {
                        vertsPos4 = float3.zero - 10f;
                        vertsID4 = 2;
                        vertsRefID4 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }

                }

                if (block6 && next)
                {
                    int id = IDBlock6;
                    int4x2 blockTopID = verticesTop[id];
                    if (blockTopID.c1.z < 0)
                    {
                        vertsPos4 = float3.zero - 10f;
                        vertsID4 = 3;
                        vertsRefID4 = id;
                        indexRange++;

                        next = false;
                    }
                    else
                    {
                        next = true;
                    }


                }
                if (next)
                {
                    vertsPos4 = new float3(1f + idx.x, 0f, 0f + idx.z);
                    vertsID4 = (3 - indexRange) + vertexCounts;
                    vertsRefID4 = -1;
                    vertexCount[0] += 1;



                }


                verticesTop[index] = new int4x2(
                         new int4(vertsID1, vertsID2, vertsID3, vertsID4),
                         new int4(vertsRefID1, vertsRefID2, vertsRefID3, vertsRefID4));


                vertexTop[index] = new float3x4(
                    vertsPos1, vertsPos2, vertsPos3, vertsPos4);

                floorCount[0] += 1;
                floorId[index] = floorCount[0] - 1;

            }
        }

    }
}