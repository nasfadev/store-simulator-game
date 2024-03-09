// using Unity.Jobs;
// using Unity.Collections;
// using Unity.Mathematics;
// using Unity.Burst;
// using UnityEngine;

// [BurstCompile]
// public struct RenderWallJob : IJob
// {
//     public float tall;
//     public float thickness;

//     public int dimension;

//     public NativeArray<int> wallID;

//     public NativeArray<bool4> wallExisting;
//     public NativeArray<float3> renderWallverts;
//     public NativeArray<int> renderWalltris;
//     public NativeArray<float2> renderUVs;




//     public void Execute()
//     {
//         int numVert = 0;
//         int numId = 0;
//         int numTris = 0;


//         for (int i = 0; i < wallID.Length; i++)
//         {


//             if (wallID[i] > 0)
//             {
//                 bool4 wallAngles = wallExisting[i];
//                 float2 ids = new float2(i % dimension, math.floor(i / dimension));
//                 UnityEngine.Debug.Log("getit!!!");



//                 if (wallAngles.x)
//                 {



//                     // xminus

//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, 0, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, 0, 1 + ids.y);
//                     numVert++;

//                     int vert1 = 0 + numId;
//                     int vert2 = 1 + numId;
//                     int vert3 = 2 + numId;
//                     int vert4 = 3 + numId;
//                     numId += 4;


//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;


//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, 0, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, 0, 1 + ids.y);
//                     numVert++;



//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;



//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x - thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x + thickness, tall, 0 + ids.y);
//                     numVert++;


//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                 }
//                 if (wallAngles.z)
//                 {

//                     // xplus

//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, 0, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, 0, 1 + ids.y);
//                     numVert++;

//                     int vert1 = 0 + numId;
//                     int vert2 = 1 + numId;
//                     int vert3 = 2 + numId;
//                     int vert4 = 3 + numId;
//                     numId += 4;


//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;


//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, 0, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, 0, 1 + ids.y);
//                     numVert++;



//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;



//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, tall, 0 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x - thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, tall, 1 + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x + thickness, tall, 0 + ids.y);
//                     numVert++;


//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                 }
//                 if (wallAngles.y)
//                 {

//                     // xplus

//                     // {new Vector3( 0 , 0, 1 - thickness),new Vector3(0 , tall, 1 - thickness), new Vector3(1 , tall, 1 - thickness), new Vector3(1 , 0, 1 - thickness) };
//                     renderWallverts[numVert] = new float3(0 + ids.x, 0, 1 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 1 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 1 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, 0, 1 - thickness + ids.y);
//                     numVert++;

//                     int vert1 = 0 + numId;
//                     int vert2 = 1 + numId;
//                     int vert3 = 2 + numId;
//                     int vert4 = 3 + numId;
//                     numId += 4;


//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     //         Vector3[] zMaxPlusVerts = new Vector3[]
//                     // {new Vector3( 0 , 0, 1 + thickness),new Vector3(0 , tall, 1 + thickness), new Vector3(1 , tall, 1 + thickness), new Vector3(1 , 0, 1 + thickness) };

//                     renderWallverts[numVert] = new float3(0 + ids.x, 0, 1 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 1 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 1 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, 0, 1 + thickness + ids.y);
//                     numVert++;



//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;

//                     // {new Vector3(0, tall, 1 - thickness),new Vector3(0, tall, 1 + thickness), new Vector3(1, tall, 1 + thickness), new Vector3(1, tall, 1 - thickness) };

//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 1 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 1 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 1 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 1 - thickness + ids.y);
//                     numVert++;


//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                 }
//                 if (wallAngles.w)
//                 {

//                     // xplus

//                     // {new Vector3( 0 , 0, 1 - thickness),new Vector3(0 , tall, 1 - thickness), new Vector3(1 , tall, 1 - thickness), new Vector3(1 , 0, 1 - thickness) };
//                     renderWallverts[numVert] = new float3(0 + ids.x, 0, 0 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 0 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 0 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, 0, 0 - thickness + ids.y);
//                     numVert++;

//                     int vert1 = 0 + numId;
//                     int vert2 = 1 + numId;
//                     int vert3 = 2 + numId;
//                     int vert4 = 3 + numId;
//                     numId += 4;


//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     //         Vector3[] zMaxPlusVerts = new Vector3[]
//                     // {new Vector3( 0 , 0, 1 + thickness),new Vector3(0 , tall, 1 + thickness), new Vector3(1 , tall, 1 + thickness), new Vector3(1 , 0, 1 + thickness) };

//                     renderWallverts[numVert] = new float3(0 + ids.x, 0, 0 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 0 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 0 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, 0, 0 + thickness + ids.y);
//                     numVert++;



//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;

//                     // {new Vector3(0, tall, 1 - thickness),new Vector3(0, tall, 1 + thickness), new Vector3(1, tall, 1 + thickness), new Vector3(1, tall, 1 - thickness) };

//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 0 - thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(0 + ids.x, tall, 0 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 0 + thickness + ids.y);
//                     numVert++;
//                     renderWallverts[numVert] = new float3(1 + ids.x, tall, 0 - thickness + ids.y);
//                     numVert++;


//                     vert1 = 0 + numId;
//                     vert2 = 1 + numId;
//                     vert3 = 2 + numId;
//                     vert4 = 3 + numId;
//                     numId += 4;



//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                     renderWalltris[numTris] = vert2;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert3;
//                     numTris++;
//                     renderWalltris[numTris] = vert4;
//                     numTris++;
//                     renderWalltris[numTris] = vert1;
//                     numTris++;
//                 }


//                 // corner 

//                 int IDBlock1 = i + dimension - 1;
//                 int IDBlock2 = i + dimension;
//                 int IDBlock3 = i + dimension + 1;
//                 int IDBlock4 = i + 1;
//                 int IDBlock5 = i - dimension + 1;
//                 int IDBlock6 = i - dimension;
//                 int IDBlock7 = i - dimension - 1;
//                 int IDBlock8 = i - 1;

//                 if (wallAngles.x && !wallExisting[IDBlock6].x && !wallExisting[IDBlock7].z)
//                 {



//                     int3 corner = AddCorner(ids, new float2(0, 0), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.x && !wallExisting[IDBlock2].x && !wallExisting[IDBlock1].z)
//                 {

//                     int3 corner = AddCorner(ids, new float2(0, 1), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;


//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }

//                 if (wallAngles.y && !wallExisting[IDBlock8].y && !wallExisting[IDBlock1].w)
//                 {


//                     int3 corner = AddCorner(ids, new float2(0, 1), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.y && !wallExisting[IDBlock4].y && !wallExisting[IDBlock3].w)
//                 {

//                     int3 corner = AddCorner(ids, new float2(1, 1), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.z && !wallExisting[IDBlock2].z && !wallExisting[IDBlock3].x)
//                 {

//                     int3 corner = AddCorner(ids, new float2(1, 1), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.z && !wallExisting[IDBlock6].z && !wallExisting[IDBlock5].x)
//                 {

//                     int3 corner = AddCorner(ids, new float2(1, 0), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.w && !wallExisting[IDBlock4].w && !wallExisting[IDBlock5].y)
//                 {


//                     int3 corner = AddCorner(ids, new float2(1, 0), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.w && !wallExisting[IDBlock8].w && !wallExisting[IDBlock7].y)
//                 {


//                     int3 corner = AddCorner(ids, new float2(0, 0), numVert, numTris, numId);
//                     numVert = corner.x;
//                     numTris = corner.y;
//                     numId = corner.z;
//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }


//             }



//         }
//         int vertsToAdd = 0;
//         for (int i = 0; i < renderUVs.Length / 4; i++)
//         {
//             renderUVs[0 + vertsToAdd] = new float2(0, 0);
//             renderUVs[1 + vertsToAdd] = new float2(0, 1);
//             renderUVs[2 + vertsToAdd] = new float2(1, 1);
//             renderUVs[3 + vertsToAdd] = new float2(1, 0);

//             vertsToAdd += 4;

//         }
//     }
//     private int3 AddCorner(float2 ids, float2 offset, int numVert, int numTris, int numId)
//     {
//         // { new Vector3(0 + thickness, 0, 0 - thickness),
//         // new Vector3(0 + thickness, tall, 0 - thickness), 
//         // new Vector3(0 + thickness, tall, 0 + thickness), 
//         // new Vector3(0 + thickness, 0, 0 + thickness) };
//         float tebalCorner = thickness - 0.0001f;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, 0, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, 0, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;

//         int vert1 = 0 + numId;
//         int vert2 = 1 + numId;
//         int vert3 = 2 + numId;
//         int vert4 = 3 + numId;
//         numId += 4;


//         renderWalltris[numTris] = vert1;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert4;
//         numTris++;
//         renderWalltris[numTris] = vert1;
//         numTris++;

//         // { new Vector3(0 - thickness, 0, 0 - thickness),
//         // new Vector3(0 - thickness, tall, 0 - thickness), 
//         // new Vector3(0 - thickness, tall, 0 + thickness), 
//         // new Vector3(0 - thickness, 0, 0 + thickness) };
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, 0, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, 0, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;

//         vert1 = 0 + numId;
//         vert2 = 1 + numId;
//         vert3 = 2 + numId;
//         vert4 = 3 + numId;
//         numId += 4;

//         // vert4, vert3, vert2, vert2, vert1, vert4

//         renderWalltris[numTris] = vert4;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert1;
//         numTris++;
//         renderWalltris[numTris] = vert4;
//         numTris++;

//         // new Vector3(0 - thickness, 0, 0 - thickness),
//         // new Vector3(0 - thickness, tall, 0 - thickness), 
//         // new Vector3(0 + thickness, tall, 0 - thickness),
//         //  new Vector3(0 + thickness, 0, 0 - thickness) };

//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, 0, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, 0, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;

//         vert1 = 0 + numId;
//         vert2 = 1 + numId;
//         vert3 = 2 + numId;
//         vert4 = 3 + numId;
//         numId += 4;

//         renderWalltris[numTris] = vert1;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert4;
//         numTris++;
//         renderWalltris[numTris] = vert1;
//         numTris++;

//         // { new Vector3(0 - thickness, 0, 0 + thickness),
//         // new Vector3(0 - thickness, tall, 0 + thickness),
//         //  new Vector3(0 + thickness, tall, 0 + thickness),
//         //   new Vector3(0 + thickness, 0, 0 + thickness) };


//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, 0, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, 0, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;

//         vert1 = 0 + numId;
//         vert2 = 1 + numId;
//         vert3 = 2 + numId;
//         vert4 = 3 + numId;
//         numId += 4;

//         renderWalltris[numTris] = vert4;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert1;
//         numTris++;
//         renderWalltris[numTris] = vert4;
//         numTris++;

//         // new Vector3(0 - thickness, tall, 0 - thickness),
//         // new Vector3(0 - thickness, tall, 0 + thickness), 
//         // new Vector3(0 + thickness, tall, 0 + thickness),
//         //  new Vector3(0 + thickness, tall, 0 - thickness) };

//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 - tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 + tebalCorner + ids.y + offset.y);
//         numVert++;
//         renderWallverts[numVert] = new float3(0 + tebalCorner + ids.x + offset.x, tall, 0 - tebalCorner + ids.y + offset.y);
//         numVert++;

//         vert1 = 0 + numId;
//         vert2 = 1 + numId;
//         vert3 = 2 + numId;
//         vert4 = 3 + numId;
//         numId += 4;

//         renderWalltris[numTris] = vert1;
//         numTris++;
//         renderWalltris[numTris] = vert2;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert3;
//         numTris++;
//         renderWalltris[numTris] = vert4;
//         numTris++;
//         renderWalltris[numTris] = vert1;
//         numTris++;
//         return new int3(numVert, numTris, numId);
//     }
// }