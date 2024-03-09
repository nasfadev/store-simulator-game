using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    public class NestedAnotherClass : MonoBehaviour
    {

        void Start()
        {
            CallNested();
        }
        void CallNested()
        {
            int value = NestedData.Data.instance.myInt;//get value from nested data class
            Debug.Log("Get nested data: " + value);

            //NestedData.Data.instance.myInt = 10;//change value from nested data
            //Debug.Log("Change nested data to: " + NestedData.Data.instance.myInt);

        }
    }
}