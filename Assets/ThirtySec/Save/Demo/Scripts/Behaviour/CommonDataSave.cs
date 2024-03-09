using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThirtySec
{
    public class CommonData : ThirtySec.Serializable<CommonData>
    {
        public int intData;
        public string stringData = string.Empty;
        public float floatData;
        public int[] a = new int[900];
        public bool boolData;
        public Vector2 vector2Data;
        public Vector3 vector3Data;
        public Color colorData;
        public Quaternion quaternionData;


    }
    public class CommonDataSave : MonoBehaviour
    {

        public Text intDisplayer, stringDisplayer, floatDisplayer, boolDisplayer, vector2Displayer, vector3Displayer, quaternionDisplayer;
        public Image colorRender;
        public Button randomDataBtn;



        private void Start()
        {
            randomDataBtn.onClick.AddListener(RandomNewData);
            Debug.Log(CommonData.IsExist());

            UpdateData();
            Debug.Log(CommonData.IsExist());

        }
        void UpdateData()
        {
            intDisplayer.text = "Int Data: " + CommonData.instance.intData.ToString();
            stringDisplayer.text = "String Data: " + CommonData.instance.stringData.ToString();
            floatDisplayer.text = "Float Data: " + CommonData.instance.floatData.ToString("0.0");
            boolDisplayer.text = "Bool Data: " + CommonData.instance.boolData.ToString();
            vector2Displayer.text = "Vector2 Data: " + CommonData.instance.vector2Data.ToString();
            vector3Displayer.text = "Vector3 Data: " + CommonData.instance.vector3Data.ToString();
            quaternionDisplayer.text = "Quaternion Data: " + CommonData.instance.quaternionData.ToString();
            colorRender.color = CommonData.instance.colorData;

        }

        void RandomNewData()
        {

            string charset = "abcdefghijklmnotuvpqrswxyz";
            CommonData.instance.intData = Random.Range(-1000, 1000);
            CommonData.instance.stringData = charset[Random.Range(0, charset.Length)].ToString();
            CommonData.instance.floatData = Random.Range(-1000f, 1000f);
            CommonData.instance.boolData = Random.Range(0, 2) == 0;
            CommonData.instance.vector2Data = Random.insideUnitCircle * 999;
            CommonData.instance.vector3Data = Random.insideUnitSphere * 999;
            CommonData.instance.colorData = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f);
            CommonData.instance.quaternionData = new Quaternion(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            CommonData.instance.a[0] = Random.Range(0, 100);


            UpdateData();

        }

    }
}