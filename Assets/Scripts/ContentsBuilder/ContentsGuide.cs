// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Mathematics;


// public class ContentsGuide : MonoBehaviour
// {
//     public static GameObject prefab;
//     public static Vector2 boundsSize;
//     private GameObject guide;
//     private bool isInit;
//     [SerializeField] private Camera cam;
//     [SerializeField] private LayerMask ignore;
//     // Start is called before the first frame update

//     // Update is called once per frame
//     void Update()
//     {
//         if (isInit)
//         {
//             Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
//             RaycastHit hit;
//             if (Physics.Raycast(ray, out hit, 1000f, ~ignore))
//             {

//                 guide.transform.position = GetPosition(RotateButton.rotateID, hit.point, boundsSize);



//             }
//         }

//     }
//     public void Init()
//     {
//         guide = Instantiate(prefab, Vector3.zero, quaternion.identity, transform);
//         guide.isStatic = false;
//         guide.AddComponent<ContentsGuidePrefab>();
//         Destroy(guide.GetComponent<Rigidbody>());
//         Destroy(guide?.GetComponent<Cashier>());
//         guide.transform.rotation = Quaternion.Euler(0f, getRotate(RotateButton.rotateID), 0f);

//         isInit = true;
//     }
//     public void Remove()
//     {
//         isInit = false;

//         Destroy(guide);
//     }
//     public void Rotate()
//     {
//         if (PlaceButton.builderId.x == 3)
//         {
//             guide.transform.Rotate(new Vector3(0, 90f, 0));

//         }
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
// }
