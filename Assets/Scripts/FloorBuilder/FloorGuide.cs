using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
public class FloorGuide : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material deleteMaterial;
    [SerializeField] private TextMeshProUGUI costPriceText;
    [SerializeField] private LayerMask mask;
    [SerializeField] private CardBuilderGenerator data;

    public Vector3 firstSnapPos;
    public Vector3 lastSnapPos;
    private Coroutine runCoroutine;
    private bool isGotFirstSnap;
    public static FloorGuide Instance;
    public int floorCount;
    public CardBuilderGenerator.CardBuilderData floorData;
    private MaterialPropertyBlock mtbAdded;
    private MaterialPropertyBlock tempMtb;


    private void Awake()
    {
        Instance = this;
        mtbAdded = new MaterialPropertyBlock();
        tempMtb = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(tempMtb);
        Color addColor;
        ColorUtility.TryParseHtmlString("#D8E2FB", out addColor);
        mtbAdded.SetColor("_BaseColor", addColor);

    }
    private IEnumerator RunIE()
    {

        if (PlaceButton.Instance.contentID == 0)
        {
            meshRenderer.sharedMaterial = deleteMaterial;
            meshRenderer.SetPropertyBlock(tempMtb);

            Debug.Log("isTouched Broh");
        }
        else
        {
            floorData = data.cardBuilderDatas[PlaceButton.Instance.contentID - 1];

            meshRenderer.sharedMaterial = FloorBuilder.Instance.FloorMaterials[PlaceButton.Instance.contentID - 1];
            meshRenderer.SetPropertyBlock(mtbAdded);
        }
        while (true)
        {

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                Vector3 snapPos = hit.point;
                if (isGotFirstSnap)
                {
                    MakeBlueprint(firstSnapPos, snapPos);
                }
                else
                {
                    MakeBlueprint(snapPos, snapPos);

                }

            }
            yield return null;


        }
    }
    public void Run()
    {
        runCoroutine = StartCoroutine(RunIE());
    }
    public void MakeBlueprint(Vector3 firstPos, Vector3 snapPos)
    {
        int minX = (int)(math.ceil(firstPos.x <= snapPos.x ? firstPos.x : snapPos.x) - 1);
        int maxX = (int)(math.ceil(firstPos.x >= snapPos.x ? firstPos.x : snapPos.x));
        int minY = (int)(math.ceil(firstPos.z <= snapPos.z ? firstPos.z : snapPos.z) - 1);
        int maxY = (int)(math.ceil(firstPos.z >= snapPos.z ? firstPos.z : snapPos.z));
        if (PlaceButton.Instance.contentID > 0)
        {
            int floorCount = (maxX - minX) * (maxY - minY);
            costPriceText.text = $"<sprite name=\"askari\"> {floorData.price * floorCount}";
        }
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] { new Vector3(minX, 0, minY), new Vector3(minX, 0, maxY), new Vector3(maxX, 0, maxY), new Vector3(maxX, 0, minY) };

        mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
        mesh.uv = new Vector2[] { new Vector2(minX, minY), new Vector2(minX, maxY), new Vector2(maxX, maxY), new Vector2(maxX, minY) };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }
    public void Stop()
    {
        if (runCoroutine != null)
        {

            // CardSingleton.Instance.isTouched = false;

            StopCoroutine(runCoroutine);
            meshFilter.mesh.Clear();

        }
        isGotFirstSnap = false;

    }
    public void GetFirstSnapPos()
    {
        if (PlaceButton.Instance.mode != "FloorBuilder")
        {
            return;
        }
        if (PlaceButton.Instance.contentID == 0)
        {
            AutoDelete.Instance.isDeleteTouched = true;

        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            firstSnapPos = hit.point;
        }
        isGotFirstSnap = true;
    }
    public void GetLastSnapPos()
    {
        if (PlaceButton.Instance.mode != "FloorBuilder")
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            lastSnapPos = hit.point;
            if (PlaceButton.Instance.contentID > 0)
            {
                int minX = (int)(math.ceil(firstSnapPos.x <= lastSnapPos.x ? firstSnapPos.x : lastSnapPos.x) - 1);
                int maxX = (int)(math.ceil(firstSnapPos.x >= lastSnapPos.x ? firstSnapPos.x : lastSnapPos.x));
                int minY = (int)(math.ceil(firstSnapPos.z <= lastSnapPos.z ? firstSnapPos.z : lastSnapPos.z) - 1);
                int maxY = (int)(math.ceil(firstSnapPos.z >= lastSnapPos.z ? firstSnapPos.z : lastSnapPos.z));
                floorCount = (maxX - minX) * (maxY - minY);
                int askariPrice = floorData.price * floorCount;
                int moriumPrice = floorData.priceMorium * floorCount;
                if (!EconomyCurrency.Instance.CanAskariDecrease(askariPrice))
                {
                    Debug.Log("askari noo");
                    PlaceButton.Instance.isRun = false;
                    PlaceButton.Instance.isExecute = false;
                }
                if (!EconomyCurrency.Instance.CanMoriumDecrease(moriumPrice))
                {
                    Debug.Log("askari noo");
                    PlaceButton.Instance.isRun = false;
                    PlaceButton.Instance.isExecute = false;
                }
            }
        }

        isGotFirstSnap = false;
        meshFilter.mesh.Clear();
    }
}
