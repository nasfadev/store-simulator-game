using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoFile : MonoBehaviour {

    public class Data : ThirtySec.Serializable<Data>
    {
        public int myInt;
        public string myString = "change it";
    }
    public InputField stringData;
    public InputField intData;

    private void Start()
    {
        //load data from save and assign to the input field
        stringData.text = Data.instance.myString;
        intData.text = Data.instance.myInt.ToString();


        //add event to update data each time input field value changed.
        stringData.onValueChanged.AddListener(OnStringValueChanged);
        intData.onValueChanged.AddListener(OnIntValueChanged);
    }

    public void OnStringValueChanged(string value)
    {
        //get data from input field and assign to the data class
        Data.instance.myString = value;
    }

    public void OnIntValueChanged(string value)
    {
        //get data from input field and assign to the data class
        Data.instance.myInt = System.Int32.Parse(value);
    }

}
