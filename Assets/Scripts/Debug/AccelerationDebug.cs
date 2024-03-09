// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// public class AccelerationDebug : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
// {
//     // Start is called before the first frame update
//     [SerializeField] private Rigidbody rigid;
//     [SerializeField] private LayerMask ignore;
//     [SerializeField] private float acceleration;

//     private bool isDown;
//     private Vector3 delta;
//     private Vector3 initialPosition;

//     // Update is called once per frame
//     public void OnPointerDown(PointerEventData data)
//     {
//         // rigid.AddForce(Vector3.up * 50);
//         Debug.Log("hittttttt");
//         isDown = true;
//         initialPosition = transform.position;
//     }
//     public void OnDrag(PointerEventData data)
//     {

//         // rigid.AddForce(Vector3.one * 50);
//         Ray ray = Camera.main.ScreenPointToRay(data.position);
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, 1000f, ~ignore))
//         {

//             initialPosition = new Vector3(hit.point.x, 0.51f, hit.point.z);
//             delta = initialPosition - transform.position;
//             transform.position = initialPosition;

//         }

//         Debug.Log("hittttttt");
//     }
//     public void OnPointerUp(PointerEventData data)
//     {
//         isDown = false;

//         rigid.AddForce(delta * acceleration, ForceMode.Acceleration);


//         Debug.Log(delta * acceleration);
//     }

// }
