using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirtySec
{
    public class PlayerPrefsExample : MonoBehaviour
    {
        public int myInt = 100;
        public string myString = "ThirtySec";
        public float myFloat = 99f;
        public bool myBool;

        void Start()
        {

            GetAndSet();
        }

        public void GetAndSet()
        {

            //get data 
            int myInt = ThirtySec.PlayerPrefs.GetInt("myKey");
            Debug.Log("My previous int: " + myInt);
            ThirtySec.PlayerPrefs.SetInt("myKey", this.myInt);


            string myString = ThirtySec.PlayerPrefs.GetString("myKey");
            Debug.Log("My previous string: " + myString);
            ThirtySec.PlayerPrefs.SetString("myKey", this.myString);




            float myFloat = ThirtySec.PlayerPrefs.GetFloat("myKey");
            Debug.Log("My previous float: " + myFloat);
            ThirtySec.PlayerPrefs.SetFloat("myKey", this.myFloat);



            bool myBool = ThirtySec.PlayerPrefs.GetBool("myKey");
            Debug.Log("My previous bool: " + myBool);
            ThirtySec.PlayerPrefs.SetBool("myKey", this.myBool);


            Debug.Log("My current int: " + this.myInt);
            Debug.Log("My current string: " + this.myString);
            Debug.Log("My current float: " + this.myFloat);
            Debug.Log("My current bool: " + this.myBool);



            Debug.Log("PlayerPrefs file path: " + ThirtySec.PlayerPrefs.instance.path);

        }
    }
}