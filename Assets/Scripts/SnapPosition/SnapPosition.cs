using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;


public class SnapPosition : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera cam;
    [SerializeField] private MeshFilter run;



    [HideInInspector] public static float3x2 snapPos;
    private bool isRun;
    private bool isPlace;


    void Update()
    {
        if (isRun)

        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                Vector3 point = hit.point;
                int minX = (int)(math.ceil(snapPos.c0.x <= point.x ? snapPos.c0.x : point.x) - 1);
                int maxX = (int)(math.ceil(snapPos.c0.x >= point.x ? snapPos.c0.x : point.x));
                int minY = (int)(math.ceil(snapPos.c0.z <= point.z ? snapPos.c0.z : point.z) - 1);
                int maxY = (int)(math.ceil(snapPos.c0.z >= point.z ? snapPos.c0.z : point.z));

                Mesh mesh = new Mesh();

                mesh.vertices = new Vector3[] { new Vector3(minX, 0, minY), new Vector3(minX, 0, maxY), new Vector3(maxX, 0, maxY), new Vector3(maxX, 0, minY) };
                mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                run.mesh = mesh;





            }



        }

        if (isPlace)
        {
            run.mesh.Clear();
            isPlace = false;
        }



    }

    public void GetFirstSnap()
    {
        isRun = true;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {

            snapPos = new float3x2(hit.point, Vector3.zero);



        }

    }
    public void GetLastSnap()
    {

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {

            snapPos = new float3x2(snapPos.c0, hit.point);


        }
        isRun = false;
        isPlace = true;



    }

}