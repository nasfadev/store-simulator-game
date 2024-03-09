
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UIElements;

public class DoorAndWindowBuilder : MonoBehaviour
{
    [SerializeField] private TemplateData _templateData;
    [SerializeField] private int dimension;
    [SerializeField] private float thick;
    [SerializeField] private DoorAndWindowProductData doorAndWindowProductData;
    [SerializeField] private MeshCombiner meshCombiner;
    [SerializeField] private float offset;
    [SerializeField] private float addOffset;
    public static DoorAndWindowBuilder Instance;
    public event Action beforeRender;
    public event Action afterRender;
    public event Action resetDoor;
    private int wallCount;
    private bool isRunning;
    private Coroutine runCoroutine;
    [System.Serializable]
    public struct DoorAndWindowData
    {
        public bool isExixt;
        public IdAndRotate data1;
        public IdAndRotate data2;
        public IdAndRotate data3;
        public IdAndRotate data4;

    }
    [System.Serializable]
    public struct IdAndRotate
    {
        public int Id;
        public int rotateId;
        public SerializableVector2 pos;

    }
    [System.Serializable]
    public struct SerializableVector2
    {
        public float x;
        public float z;

        public SerializableVector2(Vector3 vector3)
        {
            x = vector3.x;
            z = vector3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, 0, z);
        }
    }
    public void ResetDoor()
    {
        resetDoor?.Invoke();
    }
    public class DoorAndWindowSaveData : ThirtySec.Serializable<DoorAndWindowSaveData>
    {
        public int pass;
        public DoorAndWindowData[] datas;
    }
    public class DataTemplate
    {
        public DoorAndWindowData[] datas;

    }
    private void Awake()
    {
        Instance = this;
        if (DoorAndWindowSaveData.IsExist() && DoorAndWindowSaveData.instance != null && UnityEngine.PlayerPrefs.GetInt("doorPass") == DoorAndWindowSaveData.instance.pass)
        {
            StartCoroutine(LoadDataIE());


        }
        else
        {
            StateLoaded.initLoaded++;


            DataTemplate dataTemplate = JsonUtility.FromJson<DataTemplate>(_templateData.windowAndDoor);
            DoorAndWindowSaveData.instance.datas = dataTemplate.datas;
            int seed = (int)System.DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int pass = UnityEngine.Random.Range(1000, 3000);
            UnityEngine.PlayerPrefs.SetInt("doorPass", pass);
            DoorAndWindowSaveData.instance.pass = pass;
            StartCoroutine(LoadDataIE());
        }
    }
    public void Run()
    {
        runCoroutine = StartCoroutine(RunIE());
    }
    public void Stop()
    {
        if (runCoroutine == null)
        {
            return;
        }
        StartCoroutine(StopIE());
    }
    private IEnumerator StopIE()
    {
        while (isRunning)
        {
            yield return null;
        }

        StopCoroutine(runCoroutine);

    }
    private IEnumerator RunIE()
    {
        DoorAndWindowGuide doorAndWindowGuide = DoorAndWindowGuide.Instance;

        while (true)
        {
            PlaceButton placeButton = PlaceButton.Instance;


            if (placeButton.isExecute && placeButton.contentID > 0 && placeButton.mode == "DoorAndWindowBuilder")
            {

                if (doorAndWindowGuide.doorAndBuilderGuidePrefab.isCantPlace)
                {
                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                    isRunning = false;
                    yield return null;
                    continue;

                }
                isRunning = true;
                CalculateIndexAndInstance(placeButton);
                int askariPrice = doorAndWindowProductData.data[placeButton.contentID - 1].askariPrice * wallCount;
                int moriumPrice = doorAndWindowProductData.data[placeButton.contentID - 1].moriumPrice * wallCount;
                EconomyNotif.Instance.Append("askari", "Askari", askariPrice, false);
                EconomyNotif.Instance.Append("morium", "Morium", moriumPrice, false);
                EconomyCurrency.Instance.DecreaseAskari(askariPrice);
                EconomyCurrency.Instance.DecreaseMorium(moriumPrice);

                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                Debug.Log("destroyRender");
                beforeRender?.Invoke();
                meshCombiner.CombineMeshes(false);
                afterRender?.Invoke();


                isRunning = false;
            }
            if (doorAndWindowGuide.isDelete && placeButton.contentID == 0 && placeButton.mode == "DoorAndWindowBuilder")
            {

                isRunning = true;

                int index = doorAndWindowGuide.tempDoorAndWindow.index;
                int dataNum = doorAndWindowGuide.tempDoorAndWindow.dataNum;
                GameObject thePrefab = doorAndWindowGuide.tempDoorAndWindow.gameObject;
                DeleteData(index, dataNum);
                Destroy(thePrefab);
                yield return null;
                Debug.Log("destroyRender");
                beforeRender?.Invoke();
                meshCombiner.CombineMeshes(false);
                afterRender?.Invoke();
                doorAndWindowGuide.isDelete = false;

                AutoDelete.Instance.isDeleteTouched = false;
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;

                isRunning = false;
            }
            yield return null;
        }
    }
    private void DeleteData(int index, int dataNum)
    {
        if (dataNum == 1)
        {
            DoorAndWindowSaveData.instance.datas[index].data1.Id = 0;
            Debug.Log("added door 1");
        }
        else if (dataNum == 2)
        {
            DoorAndWindowSaveData.instance.datas[index].data2.Id = 0;

            Debug.Log("added door 2");


        }
        else if (dataNum == 3)
        {
            DoorAndWindowSaveData.instance.datas[index].data3.Id = 0;

            Debug.Log("added door 3");


        }
        else
        {
            DoorAndWindowSaveData.instance.datas[index].data4.Id = 0;

            Debug.Log("added door 4");


        }
        ReCheckDataExistForDelete(index);
    }
    private void CalculateIndexAndInstance(PlaceButton placeButton)
    {
        DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[placeButton.contentID - 1];

        DoorAndWindowGuide doorAndWindowGuide = DoorAndWindowGuide.Instance;
        Vector3 firstPos = doorAndWindowGuide.firstSnapPos;
        Vector3 lastPos = doorAndWindowGuide.lastSnapPos;
        int minX = (int)(Mathf.Floor(firstPos.x <= lastPos.x ? firstPos.x : lastPos.x));
        int maxX = (int)(Mathf.Floor(firstPos.x >= lastPos.x ? firstPos.x : lastPos.x));
        int minY = (int)(Mathf.Floor(firstPos.z <= lastPos.z ? firstPos.z : lastPos.z));
        int maxY = (int)(Mathf.Floor(firstPos.z >= lastPos.z ? firstPos.z : lastPos.z));
        if (doorAndWindowGuide.rotateIdButton == 1 || doorAndWindowGuide.rotateIdButton == 3)
        {
            minX = (int)(Mathf.Floor(firstPos.x));
            maxX = minX;



        }
        else
        {
            minY = (int)(Mathf.Floor(firstPos.z));
            maxY = minY;


        }
        if (!productData.isMultipleAdded)
        {
            int trueIndex = GetIndex(productData.longInMeter, new Vector3(minX + 1, 0f, minY + 1), doorAndWindowGuide.rotateIdButton);
            Debug.Log("IndexOfDoor " + trueIndex);

            int index = trueIndex < 0 ? 0 : trueIndex;
            SetDataAndInstance(index, doorAndWindowGuide.tempPos, placeButton, doorAndWindowGuide);
            wallCount = 1;
            return;
        }
        for (int y = minY; y < maxY + 1; y++)
        {
            for (int x = minX; x < maxX + 1; x++)
            {
                int trueIndex = GetIndex(productData.longInMeter, new Vector3(x, 0f, y), doorAndWindowGuide.rotateIdButton);
                Debug.Log("IndexOfDoor " + trueIndex);

                int index = trueIndex < 0 ? 0 : trueIndex;
                // if (!DoorAndWindowSaveData.instance.datas[index].isExixt)
                // {
                // }
                SetDataAndInstance(index, placeButton, doorAndWindowGuide);

            }

        }
        wallCount = (maxX - minX + 1) * (maxY - minY + 1);
    }
    private void SetDataAndInstance(int index, PlaceButton placeButton, DoorAndWindowGuide doorAndWindowGuide)
    {
        ReCheckDataExistForAdd(index);

        if (doorAndWindowGuide.rotateIdSaved == 1)
        {
            DoorAndWindowSaveData.instance.datas[index].data1.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data1.rotateId = RotateButton.rotateID;


            Debug.Log("added door 1");
        }
        else if (doorAndWindowGuide.rotateIdSaved == 2)
        {
            DoorAndWindowSaveData.instance.datas[index].data2.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data2.rotateId = RotateButton.rotateID;


            Debug.Log("added door 2");


        }
        else if (doorAndWindowGuide.rotateIdSaved == 3)
        {
            DoorAndWindowSaveData.instance.datas[index].data3.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data3.rotateId = RotateButton.rotateID;


            Debug.Log("added door 3");


        }
        else
        {
            DoorAndWindowSaveData.instance.datas[index].data4.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data4.rotateId = RotateButton.rotateID;


            Debug.Log("added door 4");


        }


        Debug.Log($"add door index {index}, id {placeButton.contentID}, rotateId {RotateButton.rotateID}, dataNum {doorAndWindowGuide.rotateIdSaved}");
        InstancePrefab(index, placeButton.contentID, RotateButton.rotateID, doorAndWindowGuide.rotateIdSaved);
    }
    private void SetDataAndInstance(int index, Vector3 pos, PlaceButton placeButton, DoorAndWindowGuide doorAndWindowGuide)
    {
        ReCheckDataExistForAdd(index);

        if (doorAndWindowGuide.rotateIdSaved == 1)
        {
            DoorAndWindowSaveData.instance.datas[index].data1.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data1.rotateId = RotateButton.rotateID;
            DoorAndWindowSaveData.instance.datas[index].data1.pos = new SerializableVector2(pos);

            Debug.Log("added door 1");
        }
        else if (doorAndWindowGuide.rotateIdSaved == 2)
        {
            DoorAndWindowSaveData.instance.datas[index].data2.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data2.rotateId = RotateButton.rotateID;
            DoorAndWindowSaveData.instance.datas[index].data2.pos = new SerializableVector2(pos);

            Debug.Log("added door 2");


        }
        else if (doorAndWindowGuide.rotateIdSaved == 3)
        {
            DoorAndWindowSaveData.instance.datas[index].data3.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data3.rotateId = RotateButton.rotateID;
            DoorAndWindowSaveData.instance.datas[index].data3.pos = new SerializableVector2(pos);

            Debug.Log("added door 3");


        }
        else
        {
            DoorAndWindowSaveData.instance.datas[index].data4.Id = placeButton.contentID;
            DoorAndWindowSaveData.instance.datas[index].data4.rotateId = RotateButton.rotateID;
            DoorAndWindowSaveData.instance.datas[index].data4.pos = new SerializableVector2(pos);

            Debug.Log("added door 4");


        }


        Debug.Log($"add door index {index}, id {placeButton.contentID}, rotateId {RotateButton.rotateID}, dataNum {doorAndWindowGuide.rotateIdSaved}");
        InstancePrefab(index, placeButton.contentID, RotateButton.rotateID, doorAndWindowGuide.rotateIdSaved, pos);
    }
    private void ReCheckDataExistForDelete(int index)
    {

        if (DoorAndWindowSaveData.instance.datas[index].data1.Id > 0)
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = true;
            return;
        }
        else if (DoorAndWindowSaveData.instance.datas[index].data2.Id > 0)
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = true;
            return;
        }
        else if (DoorAndWindowSaveData.instance.datas[index].data3.Id > 0)
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = true;
            return;
        }
        else if (DoorAndWindowSaveData.instance.datas[index].data4.Id > 0)
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = true;
            return;
        }
        else
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = false;

        }

    }
    private void ReCheckDataExistForAdd(int index)
    {
        if (!DoorAndWindowSaveData.instance.datas[index].isExixt)
        {
            DoorAndWindowSaveData.instance.datas[index].isExixt = true;
        }
    }
    private int GetIndex(int longInMeter, Vector3 point, int rotateID)
    {
        int index = 0;
        if (longInMeter % 2 == 0)
        {
            float x = Mathf.Floor(point.x - .5f) + 1f;
            float y = Mathf.Floor(point.z - .5f) + 1f;

            if (rotateID == 1 || rotateID == 3)
            {
                index = ((int)((34 * (y - (longInMeter / 2))) + x));


            }
            else
            {
                index = ((int)((34 * y) + (x - (longInMeter / 2))));


            }
        }
        else
        {
            float x = Mathf.Floor(point.x - offset) + addOffset;
            float y = Mathf.Floor(point.z - offset) + addOffset;
            if (rotateID == 1 || rotateID == 3)
            {
                index = ((int)((34 * (y - (longInMeter / 2))) + x));


            }
            else
            {
                index = ((int)((34 * y) + (x - (longInMeter / 2))));


            }
        }

        return index;
    }
    private void InstancePrefab(int index, int Id, int rotateId, int dataNum, Vector3 pos)
    {
        DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[Id - 1];

        GameObject prefab = Instantiate(
      productData.prefab,
        pos,
      Quaternion.Euler(0f, GetRotation(rotateId), 0f),
      transform);
        DoorAndWindow doorAndWindow = prefab.GetComponent<DoorAndWindow>();
        doorAndWindow.index = index;
        doorAndWindow.Id = Id;
        doorAndWindow.dataNum = dataNum;



    }
    private void InstancePrefab(int index, int Id, int rotateId, int dataNum)
    {
        DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[Id - 1];

        GameObject prefab = Instantiate(
      productData.prefab,
     GetPosition(index, dataNum, productData.longInMeter),
      Quaternion.Euler(0f, GetRotation(rotateId), 0f),
      transform);
        DoorAndWindow doorAndWindow = prefab.GetComponent<DoorAndWindow>();
        doorAndWindow.index = index;
        doorAndWindow.Id = Id;
        doorAndWindow.dataNum = dataNum;


    }

    private Vector3 GetPosition(int index, int dataNum, int longInMeter)
    {
        Vector2 ids = new Vector2(index % dimension, Mathf.Floor(index / dimension));

        Debug.Log($"ids walls {ids}");
        if (longInMeter % 2 == 0)
        {
            if (dataNum == 1 || dataNum == 3)
            {
                ids += new Vector2(0, longInMeter / 2);
            }
            else
            {
                ids += new Vector2(longInMeter / 2, 0);
            }

        }
        else
        {
            if (dataNum == 1)
            {
                ids += new Vector2(0, (longInMeter / 2) + .5f);
            }
            else if (dataNum == 2)
            {
                ids += new Vector2((longInMeter / 2) + .5f, 1f);
            }
            else if (dataNum == 3)
            {
                ids += new Vector2(1f, (longInMeter / 2) + .5f);
            }
            else
            {
                ids += new Vector2((longInMeter / 2) + .5f, 0f);

            }
        }



        return new Vector3(ids.x, 0, ids.y);
    }

    private float GetRotation(int rotateId)
    {
        return 90 * (rotateId - 1);
    }
    private IEnumerator LoadDataIE()
    {

        for (int i = 0; i < DoorAndWindowSaveData.instance.datas.Length; i++)
        {
            DoorAndWindowData datas = DoorAndWindowSaveData.instance.datas[i];
            if (datas.isExixt)
            {
                if (datas.data1.Id > 0)
                {
                    IdAndRotate data = datas.data1;
                    DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[data.Id - 1];

                    if (productData.isMultipleAdded)
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 1);

                    }
                    else
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 1, data.pos.ToVector3());

                    }
                    yield return null;

                }
                if (datas.data2.Id > 0)
                {
                    IdAndRotate data = datas.data2;
                    DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[data.Id - 1];

                    if (productData.isMultipleAdded)
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 2);

                    }
                    else
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 2, data.pos.ToVector3());

                    }
                    yield return null;


                }
                if (datas.data3.Id > 0)
                {
                    IdAndRotate data = datas.data3;
                    DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[data.Id - 1];

                    if (productData.isMultipleAdded)
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 3);

                    }
                    else
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 3, data.pos.ToVector3());

                    }
                    yield return null;

                }
                if (datas.data4.Id > 0)
                {
                    IdAndRotate data = datas.data4;
                    DoorAndWindowProductData.Data productData = doorAndWindowProductData.data[data.Id - 1];

                    if (productData.isMultipleAdded)
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 4);

                    }
                    else
                    {
                        InstancePrefab(i, data.Id, data.rotateId, 4, data.pos.ToVector3());

                    }
                    yield return null;

                }

            }
        }
        beforeRender?.Invoke();

        meshCombiner.CombineMeshes(false);
        afterRender?.Invoke();
        StartCoroutine(CheckStateLoadedRequirement());
    }
    private IEnumerator CheckStateLoadedRequirement()
    {
        int numThisState = 3;
        while (!(StateLoaded.Loaded + 1 == numThisState))
        {
            yield return null;
        }
        StateLoaded.Loaded++;

    }

}
