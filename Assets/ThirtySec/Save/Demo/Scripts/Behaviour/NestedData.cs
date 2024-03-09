using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ThirtySec
{
    public class NestedData : MonoBehaviour
    {
        [Header("Please open this script and see the comment")]

        public Text intDisplayer;
        public Button changeDataBtn;

        //nested data can using inside behaviour class for easy maintenance, you can access from anywhere.
        public class Data : ThirtySec.Serializable<Data>
        {
            public int myInt;

        }

        private void Start()
        {
            changeDataBtn.onClick.AddListener(RandomData);
            LoadData();
        }

        void RandomData()
        {
            Data.instance.myInt = Random.Range(-1000, 1000);
            LoadData();

        }

        void LoadData()
        {
            intDisplayer.text = Data.instance.myInt.ToString();

        }



    }

   
}