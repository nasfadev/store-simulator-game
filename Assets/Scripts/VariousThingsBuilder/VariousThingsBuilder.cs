using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariousThingsBuilder : MonoBehaviour
{
    [SerializeField] private StoreData _storeData;
    [SerializeField] private TemplateData _templateData;
    [SerializeField] private int dimension;
    [SerializeField] private VariousThingsProductData variousThingsProductData;
    [SerializeField] private MeshCombiner meshCombiner;
    [SerializeField] private TextMeshProUGUI costPriceText;
    public Transform parentCashier;
    private Coroutine runCoroutine;
    private bool isRunning;
    public event Action WhenRun;

    public event Action WhenStop;
    public event Action OnBeforeRender;
    public event Action OnAfterRender;
    public static VariousThingsBuilder Instance;

    [System.Serializable]
    public struct VariousThingsData
    {
        public int id;
        public int rotateId;
        public Vector2Save position;
    }
    [System.Serializable]
    public struct Vector2Save
    {
        public float x;
        public float y;


        public Vector2Save(Vector3 vector3)
        {
            x = vector3.x;

            y = vector3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, 0f, y);
        }
    }
    public class VariousThingsDataSave : ThirtySec.Serializable<VariousThingsDataSave>
    {
        public int pass;
        public VariousThingsData[] data;
        public int[] cashierAskari;

    }
    public class DataTemplate
    {
        public VariousThingsData[] data;
        public int[] cashierAskari;
    }
    private void Awake()
    {
        Instance = this;
        if (VariousThingsDataSave.IsExist() && VariousThingsDataSave.instance != null && UnityEngine.PlayerPrefs.GetInt("variousPass") == VariousThingsDataSave.instance.pass)
        {
            StartCoroutine(Load());
        }
        else
        {
            StateLoaded.initLoaded++;
            DataTemplate dataTemplate = JsonUtility.FromJson<DataTemplate>(_templateData.variousThings1);
            int seed = (int)System.DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int pass = UnityEngine.Random.Range(1000, 3000);
            UnityEngine.PlayerPrefs.SetInt("variousPass", pass);
            VariousThingsDataSave.instance.pass = pass;
            VariousThingsDataSave.instance.data = dataTemplate.data;
            VariousThingsDataSave.instance.cashierAskari = dataTemplate.cashierAskari;
            StartCoroutine(Load());


        }
    }
    public void Run()
    {
        if (runCoroutine != null)
        {
            StartCoroutine(StopIE());

        }
        if (PlaceButton.Instance.contentID > 0)
        {
            costPriceText.text = $"{_storeData.data.variousThings[PlaceButton.Instance.contentID - 1].quantity}x";

        }
        WhenRun?.Invoke();
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
    public void CombineMeshsis()
    {
        Render();
    }
    private IEnumerator StopIE()
    {

        while (isRunning)
        {
            yield return null;
        }

        StopCoroutine(runCoroutine);
        WhenStop?.Invoke();
    }
    private IEnumerator Load()
    {
        VariousThingsDataSave variousThingsDataSave = VariousThingsDataSave.instance;
        for (int i = 0; i < variousThingsDataSave.data.Length; i++)
        {
            if (variousThingsDataSave.data[i].id > 0)
            {
                VariousThingsData data = variousThingsDataSave.data[i];
                InstancePrefab(i, data.id, data.rotateId, data.position.ToVector3());

                yield return null;

            }
        }
        Render();
        StartCoroutine(CheckStateLoadedRequirement());
        WhenStop?.Invoke();

    }
    private void Render()
    {
        OnBeforeRender?.Invoke();
        meshCombiner.CombineMeshes(false);
        OnAfterRender?.Invoke();
    }
    private IEnumerator RunIE()
    {
        VariousThingsGuide variousThingsGuide = VariousThingsGuide.Instance;

        PlaceButton placeButton = PlaceButton.Instance;
        // ini hanya fungsi untuk move
        while (true)
        {
            // kalo yang ini tuh buat add objek cuman juga buat move juga
            if (placeButton.isExecute && placeButton.contentID > 0 && placeButton.mode == "VariousThingsBuilder")
            {
                if (_storeData.data.variousThings[placeButton.contentID - 1].quantity == 0)
                {
                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                    isRunning = false;
                    yield return null;

                    continue;
                }
                if (variousThingsGuide.variousThingsGuidePrefab.isCantPlace)
                {
                    PlaceButton.Instance.isExecute = false;
                    PlaceButton.Instance.isRun = false;
                    isRunning = false;
                    yield return null;

                    continue;

                }
                isRunning = true;

                Vector3 pos = variousThingsGuide.posForBuilder;
                int index = ((int)((dimension * pos.z) + pos.x));
                int rotateId = RotateButton.rotateID;
                int id = placeButton.contentID;

                VariousThingsData variousThingsData = new VariousThingsData();
                variousThingsData.id = id;
                variousThingsData.rotateId = rotateId;
                variousThingsData.position = new Vector2Save(pos);
                VariousThingsDataSave.instance.data[index] = variousThingsData;

                InstancePrefab(index, id, rotateId, pos);
                _storeData.data.variousThings[placeButton.contentID - 1].quantity--;
                costPriceText.text = $"{_storeData.data.variousThings[placeButton.contentID - 1].quantity}x";
                yield return null;
                Render();
                PlaceButton.Instance.isExecute = false;
                PlaceButton.Instance.isRun = false;
                isRunning = false;

            }
            // yang ini buat delete
            else if (variousThingsGuide.isDelete)
            {
                isRunning = true;

                VariousThings variousThings = variousThingsGuide.variousThings;
                VariousThingsDataSave.instance.data[variousThings.index].id = 0;
                int cashierAskari = VariousThingsDataSave.instance.cashierAskari[variousThings.index];

                if (cashierAskari > 0)
                {
                    EconomyCurrency.Instance.AddAskari(cashierAskari);
                    VariousThingsDataSave.instance.cashierAskari[variousThings.index] = 0;
                }

                Destroy(variousThings.gameObject);
                yield return null;
                Render();
                variousThingsGuide.isDelete = false;
                AutoDelete.Instance.isDeleteTouched = false;
                isRunning = false;


            }
            //ini untuk move
            else if (variousThingsGuide.isMove && AutoMove.Instance.isMoveTouched)
            {
                PlaceButton.Instance.isMovingAddedWhenMoveMode = true;
                PlaceButton.Instance.isTouchedWhenMoveModeLoaded = true;
                PlaceButton.Instance.isMoveWhenMoveMode = true;
                isRunning = true;

                // ambil ref data variousthing dari variousthingsguide
                VariousThingsGuide.TempVariousThing variousThings = variousThingsGuide.tempVariousThings;
                int tempaskari = VariousThingsDataSave.instance.cashierAskari[variousThings.index];
                VariousThingsDataSave.instance.cashierAskari[variousThings.index] = 0;
                // hapus data variousthingdatasave yang lama
                VariousThingsDataSave.instance.data[variousThings.index].id = 0;
                // ubah dengan data variousthingsdatasave yang baru 
                Vector3 pos = variousThingsGuide.posForBuilder;
                int index = ((int)((dimension * pos.z) + pos.x));
                int rotateId = RotateButton.rotateID;
                int id = variousThings.id;

                VariousThingsData variousThingsData = new VariousThingsData();
                variousThingsData.id = id;
                variousThingsData.rotateId = rotateId;
                variousThingsData.position = new Vector2Save(pos);
                VariousThingsDataSave.instance.data[index] = variousThingsData;
                VariousThingsDataSave.instance.cashierAskari[index] = tempaskari;


                InstancePrefab(index, id, rotateId, pos);
                yield return null;
                Render();
                variousThingsGuide.isMove = false;
                // ini untuk flag ke automove
                AutoMove.Instance.isMoveTouched = false;
                // matiin variousthisnggude
                isRunning = false;


                break;

            }
            yield return null;

        }
    }
    private void InstancePrefab(int index, int id, int rotateId, Vector3 pos)
    {
        VariousThingsProductData.Data productData = variousThingsProductData.data[id - 1];

        GameObject prefab = Instantiate(
                  productData.prefab,
                 new Vector3(pos.x, 0f, pos.z),
                  Quaternion.Euler(new Vector3(0f, 90 * (rotateId - 1), 0f)),
                  transform);
        VariousThings variousThings = prefab.GetComponent<VariousThings>();
        variousThings.index = index;
        variousThings.id = id;
    }
    private IEnumerator CheckStateLoadedRequirement()
    {
        int numThisState = 6;
        while (!(StateLoaded.Loaded + 1 == numThisState))
        {
            yield return null;
        }
        StateLoaded.Loaded++;

    }
}