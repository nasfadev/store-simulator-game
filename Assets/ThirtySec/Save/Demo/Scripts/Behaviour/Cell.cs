using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ThirtySec
{
    public class Cell : MonoBehaviour
    {

         Text dataDisplayer;

        CellData data;


        private void Awake()
        {
            dataDisplayer = GetComponentInChildren<Text>();
            GetComponentInChildren<Button>().onClick.AddListener(IncreaseData);
        }
        public void IncreaseData()
        {
            data.value++;
            UpdateData();
        }
        void UpdateData()
        {
            dataDisplayer.text = data.value.ToString();
        }
        public void LoadData(CellData data)
        {
            this.data = data;
            UpdateData();
        }

    }
}