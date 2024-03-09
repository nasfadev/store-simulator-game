// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;

// public class AsyncTask : MonoBehaviour
// {
//     // Start is called before the first frame update
//     private int[] nam;
//     private Vector2 mat;
//     private Mesh cicik;

//     public async void asyncCek()
//     {
//         cicik = new Mesh();
//         Vector3 lok = new Vector3();
//         await Task.Run(() =>
//         {
//             lok = Vector3.one;

//         });

//         Debug.Log(lok);

//         Debug.Log("mulai");
//         await Task.Run(() => runFunc());
//         Debug.Log("kelar1");
//         await Task.Run(() => runFunc());
//         Debug.Log("kelar2");

//         Debug.Log(nam[0]);
//     }
//     void runFunc()
//     {
//         mat = new Vector2();
//         cicik.vertices = new[] { Vector3.down };
//         nam = new int[1];
//         for (int i = 0; i < 999999999; i++)
//         {
//             nam[0] = i;
//         }
//         for (int i = 0; i < 999999999; i++)
//         {
//             nam[0] = i;
//         }
//         for (int i = 0; i < 999999999; i++)
//         {
//             nam[0] = i;
//         }
//     }
// }
