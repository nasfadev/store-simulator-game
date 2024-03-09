// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Mathematics;
// using System.Linq;

// public class WallGuide : MonoBehaviour
// {
//     // Start is called before the first frame update
//     [SerializeField] private LayerMask Mask;
//     [SerializeField] private MeshFilter meshFilter;
//     [SerializeField] private MeshRenderer meshRenderer;

//     [SerializeField] private Material deleteMaterial;
//     [SerializeField] private float tall;
//     [SerializeField] private float thick;

//     private Vector3 snapPos;
//     [HideInInspector] public Vector3 firstSnapPos;
//     [HideInInspector] public Vector3 lastSnapPos;
//     private Material tempMaterial;
//     private Vector3 tempPos;
//     private int tempIndex;
//     public static WallGuide Instance;
//     private int rotateId;
//     private bool isTouched;
//     private Coroutine runCoroutine;
//     [HideInInspector] public int rotateIdSaved;

//     private void Awake()
//     {
//         Instance = this;
//         rotateId = 1;
//         MakeBlueprint(0, 0);
//         // thick /= 2;

//         Debug.Log("awake cuy");
//     }
//     public void Run()
//     {
//         gameObject.SetActive(true);



//         runCoroutine = StartCoroutine(RunIE());

//     }
//     public void Stop()
//     {

//         if (runCoroutine != null)
//         {

//             if (CardSingleton.Instance.isTouched)
//             {
//                 meshRenderer.sharedMaterial = tempMaterial;
//                 thick -= .01f;
//                 CardSingleton.Instance.isTouched = false;
//             }
//             StopCoroutine(runCoroutine);

//             gameObject.SetActive(false);





//         }
//     }

//     // Update is called once per frame
//     private IEnumerator RunIE()
//     {
//         if (CardSingleton.Instance.isTouched)
//         {
//             tempMaterial = meshRenderer.sharedMaterial;
//             meshRenderer.sharedMaterial = deleteMaterial;
//             thick += .01f;
//         }
//         while (true)
//         {


//             Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
//             RaycastHit hit;
//             if (Physics.Raycast(ray, out hit, 1000f, Mask))
//             {
//                 WhenRayTouched(hit);

//             }

//             yield return null;
//         }
//     }
//     private void WhenRayTouched(RaycastHit hit)
//     {
//         snapPos = hit.point;
//         // transform.position = new Vector3(math.ceil(hit.point.x) - .5f, 0, math.ceil(hit.point.z) - .5f);
//         tempPos = GetPosition(RotateButton.rotateID, hit.point, new Vector2(2f, 1f));
//         Vector3 ids = hit.point;
//         int minX = (int)(math.ceil(ids.x) - 1);
//         int maxY = (int)(math.ceil(ids.z) - 1);
//         int index = ((int)((34 * maxY) + minX));
//         if (transform.position != tempPos || tempIndex != index)
//         {
//             tempIndex = index;

//             Debug.Log($" wallBlueprintPos {tempPos}");
//             Debug.Log($" snappos minx {minX}, snappos maxy {maxY}");
//             Debug.Log($" snappos decx {ids.x % 1f}, snappos decz {ids.z % 1f}");


//             Debug.Log($" decimal x : {tempPos.x % 1f}, decimal z: {tempPos.z % 1f}");
//             int rotateID = RotateButton.rotateID;
//             Debug.Log($"index wall {index}");
//             if (isTouched)
//             {
//                 Blueprint(GetPosition(RotateButton.rotateID, hit.point, new Vector2(2f, 1f)));
//             }
//             if (rotateID == 1 || rotateID == 3)
//             {
//                 if (ids.x % 1f <= .5)
//                 {
//                     Debug.Log($"rotete 1");
//                     rotateId = 1;

//                 }
//                 else
//                 {
//                     Debug.Log($"rotete 3");
//                     rotateId = 3;


//                 }
//             }
//             else
//             {
//                 if (ids.z % 1f <= .5)
//                 {
//                     Debug.Log($"rotete 4");
//                     rotateId = 4;

//                 }
//                 else
//                 {
//                     Debug.Log($"rotete 2");
//                     rotateId = 2;

//                 }
//             }


//         }
//         if (!isTouched)
//         {
//             transform.position = tempPos;

//         }
//     }
//     public void SaveRotateId()
//     {
//         rotateIdSaved = rotateId;
//     }
//     public void GetFirstSnapPos()
//     {
//         firstSnapPos = snapPos;
//         isTouched = true;
//     }
//     public void GetLastSnapPos()
//     {

//         lastSnapPos = snapPos;
//         isTouched = false;
//         MakeBlueprint(0, 0);



//     }

//     public void Rotate()
//     {
//         transform.Rotate(new Vector3(0, 90f, 0));
//     }
//     private Vector3 GetPosition(int rotateID, Vector3 point, Vector2 bounds)
//     {

//         // bounds = bounds - Vector2.one;
//         switch (rotateID)
//         {
//             case 1 or 3:
//                 // ganjil genap
//                 return new Vector3(EvenOddGetPos(bounds.x, point.x), 0, EvenOddGetPos(bounds.y, point.z));

//             default:
//                 return new Vector3(EvenOddGetPos(bounds.y, point.x), 0, EvenOddGetPos(bounds.x, point.z));
//         }
//     }
//     private float EvenOddGetPos(float num, float pos)
//     {
//         if (num % 2 == 0)
//         {
//             return math.ceil(pos - .5f) - 0f;

//         }

//         return math.ceil(pos - 0f) - .5f;


//     }
//     public void Blueprint(Vector3 nowPos)
//     {
//         Vector3 worldPos = transform.InverseTransformPoint(firstSnapPos);
//         Vector3 firstPos = GetPosition(RotateButton.rotateID, firstSnapPos, new Vector2(2f, 1f));
//         nowPos -= firstPos;
//         int minX = (int)(math.ceil(worldPos.x <= nowPos.x ? 0f : nowPos.x));
//         int maxX = (int)(math.ceil(worldPos.x >= nowPos.x ? 0f : nowPos.x));
//         int minY = (int)(math.ceil(worldPos.z <= nowPos.z ? 0f : nowPos.z));
//         int maxY = (int)(math.ceil(worldPos.z >= nowPos.z ? 0f : nowPos.z));
//         if (RotateButton.rotateID == 1)
//         {
//             MakeBlueprint(minY, maxY);

//         }
//         else if (RotateButton.rotateID == 3)
//         {
//             MakeBlueprintReverse(maxY, minY);

//         }
//         else if (RotateButton.rotateID == 2)
//         {
//             MakeBlueprint(minX, maxX);

//         }
//         else
//         {
//             MakeBlueprintReverse(maxX, minX);

//         }


//     }
//     public void MakeBlueprint(int minY, int maxY)
//     {
//         Mesh mesh = new Mesh();
//         Vector3[] xPlusVertices =
//         new[] { new Vector3( 0f + thick, 0f, -.5f + minY),
//         new Vector3( 0f + thick, tall, -.5f + minY),
//         new Vector3( 0f + thick, tall, .5f + maxY),
//         new Vector3( 0f + thick, 0f, .5f + maxY)};
//         int[] xPlusTriangles = new[] { 0, 1, 2, 2, 3, 0 };

//         Vector3[] xMinVertices =
//         new[] { new Vector3(0f - thick, 0f, -.5f + minY),
//         new Vector3( 0f - thick, tall, -.5f + minY),
//         new Vector3( 0f - thick, tall, .5f + maxY),
//         new Vector3( 0f - thick, 0f, .5f + maxY) };
//         int[] xMinTriangles = new[] { 3, 2, 1, 1, 0, 3 };

//         Vector3[] zPlusVertices =
//         new[] { new Vector3( 0f + thick, 0f, .5f + maxY),
//         new Vector3( 0f + thick, tall, .5f + maxY),
//         new Vector3( 0f - thick, tall, .5f + maxY),
//         new Vector3( 0f - thick, 0f, .5f + maxY) };
//         int[] zPlusTriangles = new[] { 0, 1, 2, 2, 3, 0 };

//         Vector3[] zMinVertices =
//         new[] { new Vector3( 0f + thick, 0f, -.5f + minY),
//         new Vector3( 0f + thick, tall, -.5f + minY),
//         new Vector3( 0f - thick, tall, -.5f + minY),
//         new Vector3( 0f - thick, 0f, -.5f + minY) };
//         int[] zMinTriangles = new[] { 3, 2, 1, 1, 0, 3 };

//         Vector3[] topVertices =
//         new[] { new Vector3( 0f + thick, tall, -.5f + minY),
//         new Vector3( 0f - thick, tall, -.5f + minY),
//         new Vector3( 0f - thick, tall, .5f + maxY),
//         new Vector3( 0f + thick, tall, .5f + maxY) };
//         int[] topTriangles = new[] { 0, 1, 2, 2, 3, 0 };
//         Vector3[] vertices = xPlusVertices
//         .Concat(xMinVertices)
//         .Concat(zPlusVertices)
//         .Concat(zMinVertices)
//         .Concat(topVertices).ToArray();
//         int[] triangles = xPlusTriangles
//         .Concat(xMinTriangles)
//         .Concat(zPlusTriangles)
//         .Concat(zMinTriangles)
//         .Concat(topTriangles).ToArray();
//         int trisWait = 0;
//         int trisToAdd = 0;
//         int vertsToAdd = 0;
//         string s = "";
//         Vector2[] uvs = new Vector2[vertices.Length];
//         for (int i = 0; i < vertices.Length / 4; i++)
//         {
//             uvs[0 + vertsToAdd] = new Vector2(0, 0);
//             uvs[1 + vertsToAdd] = new Vector2(0, 1);
//             uvs[2 + vertsToAdd] = new Vector2(1, 1);
//             uvs[3 + vertsToAdd] = new Vector2(1, 0);

//             vertsToAdd += 4;

//         }
//         for (int i = 0; i < triangles.Length; i++)
//         {
//             triangles[i] += trisToAdd;
//             trisWait++;
//             if (trisWait == 6)
//             {
//                 trisToAdd += 4;
//                 trisWait = 0;
//             }
//             s += triangles[i];
//         }
//         Debug.Log($"verts : {s} lengh{triangles.Length}");
//         mesh.vertices = vertices;
//         mesh.triangles = triangles;
//         mesh.uv = uvs;

//         meshFilter.mesh = mesh;
//         meshFilter.mesh.RecalculateBounds();
//         meshFilter.mesh.RecalculateNormals();
//     }
//     public void MakeBlueprintReverse(int minY, int maxY)
//     {
//         Mesh mesh = new Mesh();
//         Vector3[] xPlusVertices =
//         new[] { new Vector3( 0f + thick, 0f, -.5f - minY),
//         new Vector3( 0f + thick, tall, -.5f - minY),
//         new Vector3( 0f + thick, tall, .5f - maxY),
//         new Vector3( 0f + thick, 0f, .5f - maxY)};
//         int[] xPlusTriangles = new[] { 0, 1, 2, 2, 3, 0 };

//         Vector3[] xMinVertices =
//         new[] { new Vector3(0f - thick, 0f, -.5f - minY),
//         new Vector3( 0f - thick, tall, -.5f - minY),
//         new Vector3( 0f - thick, tall, .5f - maxY),
//         new Vector3( 0f - thick, 0f, .5f - maxY) };
//         int[] xMinTriangles = new[] { 3, 2, 1, 1, 0, 3 };

//         Vector3[] zPlusVertices =
//         new[] { new Vector3( 0f + thick, 0f, .5f - maxY),
//         new Vector3( 0f + thick, tall, .5f - maxY),
//         new Vector3( 0f - thick, tall, .5f - maxY),
//         new Vector3( 0f - thick, 0f, .5f - maxY) };
//         int[] zPlusTriangles = new[] { 0, 1, 2, 2, 3, 0 };

//         Vector3[] zMinVertices =
//         new[] { new Vector3( 0f + thick, 0f, -.5f - minY),
//         new Vector3( 0f + thick, tall, -.5f - minY),
//         new Vector3( 0f - thick, tall, -.5f - minY),
//         new Vector3( 0f - thick, 0f, -.5f - minY) };
//         int[] zMinTriangles = new[] { 3, 2, 1, 1, 0, 3 };

//         Vector3[] topVertices =
//         new[] { new Vector3( 0f + thick, tall, -.5f - minY),
//         new Vector3( 0f - thick, tall, -.5f - minY),
//         new Vector3( 0f - thick, tall, .5f - maxY),
//         new Vector3( 0f + thick, tall, .5f - maxY) };
//         int[] topTriangles = new[] { 0, 1, 2, 2, 3, 0 };
//         Vector3[] vertices = xPlusVertices
//         .Concat(xMinVertices)
//         .Concat(zPlusVertices)
//         .Concat(zMinVertices)
//         .Concat(topVertices).ToArray();
//         int[] triangles = xPlusTriangles
//         .Concat(xMinTriangles)
//         .Concat(zPlusTriangles)
//         .Concat(zMinTriangles)
//         .Concat(topTriangles).ToArray();
//         int trisWait = 0;
//         int trisToAdd = 0;
//         int vertsToAdd = 0;
//         string s = "";
//         Vector2[] uvs = new Vector2[vertices.Length];
//         for (int i = 0; i < vertices.Length / 4; i++)
//         {
//             uvs[0 + vertsToAdd] = new Vector2(0, 0);
//             uvs[1 + vertsToAdd] = new Vector2(0, 1);
//             uvs[2 + vertsToAdd] = new Vector2(1, 1);
//             uvs[3 + vertsToAdd] = new Vector2(1, 0);

//             vertsToAdd += 4;

//         }
//         for (int i = 0; i < triangles.Length; i++)
//         {
//             triangles[i] += trisToAdd;
//             trisWait++;
//             if (trisWait == 6)
//             {
//                 trisToAdd += 4;
//                 trisWait = 0;
//             }
//             s += triangles[i];
//         }

//         Debug.Log($"verts : {s} lengh{triangles.Length}");
//         mesh.vertices = vertices;
//         mesh.triangles = triangles;
//         mesh.uv = uvs;
//         meshFilter.mesh = mesh;
//         meshFilter.mesh.RecalculateBounds();
//         meshFilter.mesh.RecalculateNormals();
//     }
// }
