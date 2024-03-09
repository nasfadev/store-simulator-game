// using Unity.Jobs;
// using Unity.Collections;
// using Unity.Mathematics;
// using Unity.Burst;

// [BurstCompile]
// public struct DeleteWallJob : IJob
// {
//     public float3x2 ids;
//     public int dimension;
//     public int rotateID;

//     public NativeArray<int> wallID;

//     public NativeArray<bool4> wallExisting;
//     public NativeArray<int> wallCount;
//     public NativeArray<int> cornerWallCount;


//     public void Execute()
//     {
//         int firstX = (int)(math.ceil(ids.c0.x) - 1);
//         int firstY = (int)(math.ceil(ids.c0.z) - 1);


//         int minX = (int)(math.ceil(ids.c0.x <= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
//         int maxX = (int)(math.ceil(ids.c0.x >= ids.c1.x ? ids.c0.x : ids.c1.x) - 1);
//         int minY = (int)(math.ceil(ids.c0.z <= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
//         int maxY = (int)(math.ceil(ids.c0.z >= ids.c1.z ? ids.c0.z : ids.c1.z) - 1);
//         // int index = ((int)((dimension + minX) * maxY));
//         // int index = ((int)((dimension * maxY) + minX));
//         if (rotateID == 1 || rotateID == 3)
//         {
//             minX = firstX;
//             maxX = minX;



//         }
//         else
//         {
//             minY = firstY;
//             maxY = minY;


//         }
//         for (int y = minY; y < maxY + 1; y++)
//         {
//             for (int x = minX; x < maxX + 1; x++)
//             {
//                 int index = ((int)((dimension * y) + x));
//                 if (rotateID == 1 && wallExisting[index].x)
//                 {
//                     bool4 wallAngles = wallExisting[index];

//                     wallExisting[index] = new bool4(false, wallAngles.y, wallAngles.z, wallAngles.w);
//                     wallCount[0]--;

//                 }
//                 else if (rotateID == 2 && wallExisting[index].y)
//                 {
//                     bool4 wallAngles = wallExisting[index];

//                     wallExisting[index] = new bool4(wallAngles.x, false, wallAngles.z, wallAngles.w);
//                     wallCount[0]--;

//                 }
//                 else if (rotateID == 3 && wallExisting[index].z)
//                 {
//                     bool4 wallAngles = wallExisting[index];

//                     wallExisting[index] = new bool4(wallAngles.x, wallAngles.y, false, wallAngles.w);
//                     wallCount[0]--;

//                 }
//                 else if (rotateID == 4 && wallExisting[index].w)
//                 {
//                     bool4 wallAngles = wallExisting[index];

//                     wallExisting[index] = new bool4(wallAngles.x, wallAngles.y, wallAngles.z, false);
//                     wallCount[0]--;

//                 }

//                 int IDBlock1 = index + dimension - 1;
//                 int IDBlock2 = index + dimension;
//                 int IDBlock3 = index + dimension + 1;
//                 int IDBlock4 = index + 1;
//                 int IDBlock5 = index - dimension + 1;
//                 int IDBlock6 = index - dimension;
//                 int IDBlock7 = index - dimension - 1;
//                 int IDBlock8 = index - 1;
//                 //     // block1    | block2 +z | block3
//                 //     // ------------------------
//                 //     // -x block8 | Target    | block4 +x
//                 //     // ------------------------
//                 //     // block7    | block6 -z | block5
//                 if (rotateID == 1 && wallExisting[IDBlock8].z)
//                 {
//                     bool4 wallAngles = wallExisting[IDBlock8];
//                     wallExisting[IDBlock8] = new bool4(wallAngles.x, wallAngles.y, false, wallAngles.w);
//                     wallCount[0]--;
//                     IDremover(IDBlock8);


//                 }
//                 else if (rotateID == 2 && wallExisting[IDBlock2].w)
//                 {
//                     bool4 wallAngles = wallExisting[IDBlock2];

//                     wallExisting[IDBlock2] = new bool4(wallAngles.x, wallAngles.y, wallAngles.z, false);
//                     wallCount[0]--;
//                     IDremover(IDBlock2);

//                 }
//                 else if (rotateID == 3 && wallExisting[IDBlock4].x)
//                 {
//                     bool4 wallAngles = wallExisting[IDBlock4];

//                     wallExisting[IDBlock4] = new bool4(false, wallAngles.y, wallAngles.z, wallAngles.w);
//                     wallCount[0]--;
//                     IDremover(IDBlock4);


//                 }
//                 else if (rotateID == 4 && wallExisting[IDBlock6].y)
//                 {
//                     bool4 wallAngles = wallExisting[IDBlock6];

//                     wallExisting[IDBlock6] = new bool4(wallAngles.x, false, wallAngles.z, wallAngles.w);
//                     wallCount[0]--;
//                     IDremover(IDBlock6);

//                 }
//                 IDremover(index);

//             }
//         }


//         Checker();


//     }
//     private void IDremover(int id)
//     {
//         bool4 cek = wallExisting[id];
//         if (!cek.x && !cek.y && !cek.z && !cek.w)
//         {
//             wallID[id] = 0;

//         }
//     }
//     private void Checker()
//     {
//         cornerWallCount[0] = 0;

//         for (int i = 0; i < wallID.Length; i++)
//         {
//             if (wallID[i] > 0)
//             {
//                 int IDBlock1 = i + dimension - 1;
//                 int IDBlock2 = i + dimension;
//                 int IDBlock3 = i + dimension + 1;
//                 int IDBlock4 = i + 1;
//                 int IDBlock5 = i - dimension + 1;
//                 int IDBlock6 = i - dimension;
//                 int IDBlock7 = i - dimension - 1;
//                 int IDBlock8 = i - 1;
//                 bool4 wallAngles = wallExisting[i];
//                 if (wallAngles.x && !wallExisting[IDBlock6].x && !wallExisting[IDBlock7].z)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.x && !wallExisting[IDBlock2].x && !wallExisting[IDBlock1].z)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }

//                 if (wallAngles.y && !wallExisting[IDBlock8].y && !wallExisting[IDBlock1].w)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.y && !wallExisting[IDBlock4].y && !wallExisting[IDBlock3].w)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.z && !wallExisting[IDBlock2].z && !wallExisting[IDBlock3].x)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.z && !wallExisting[IDBlock6].z && !wallExisting[IDBlock5].x)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.w && !wallExisting[IDBlock4].w && !wallExisting[IDBlock5].y)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//                 if (wallAngles.w && !wallExisting[IDBlock8].w && !wallExisting[IDBlock7].y)
//                 {

//                     cornerWallCount[0]++;

//                     //     // block1    | block2 +z | block3
//                     //     // ------------------------
//                     //     // -x block8 | Target    | block4 +x
//                     //     // ------------------------
//                     //     // block7    | block6 -z | block5


//                 }
//             }
//         }
//     }


// }