using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    public class BasicGameData : ThirtySec.Serializable<BasicGameData>
    {
        public int gold;
        public string name;
        public float progress;

    }

    public class BasicData : MonoBehaviour
    {

        public int gold = 100;
        public string yourname = "ThirtySec";
        public float progress = 99f;


        void Start()
        {
            GetAndSet();

        }

        void GetAndSet()
        {
            int myGold = BasicGameData.instance.gold; //get your gold
            Debug.Log("my previous gold: " + myGold);
            BasicGameData.instance.gold = gold; //change your gold value

            string myName = BasicGameData.instance.name;//get your name
            Debug.Log("my previous name: " + myName);
            BasicGameData.instance.name = yourname; //change your name

            float myProgress = BasicGameData.instance.progress; //get your progress
            Debug.Log("my previous progress: " + myProgress);
            BasicGameData.instance.progress = progress; //change your progress

            Debug.Log("my current gold: " + BasicGameData.instance.gold);
            Debug.Log("my current name: " + BasicGameData.instance.name);
            Debug.Log("my current progress: " + BasicGameData.instance.progress);

            Debug.Log("Data file path: " + BasicGameData.instance.path);
        }
    }
}