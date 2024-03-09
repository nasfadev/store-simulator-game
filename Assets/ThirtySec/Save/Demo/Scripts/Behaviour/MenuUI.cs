using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ThirtySec
{
    public class MenuUI : MonoBehaviour
    {
        public Text goldDisplayer;
        public Button randomGoldBtn;

        PopupUI popupUI;

        void OnEnable()
        {
            PlayerPrefsDataMix.onGoldValueChanged += UpdateGoldData;
        }

        void OnDisable()
        {
            PlayerPrefsDataMix.onGoldValueChanged -= UpdateGoldData;
        }

        void Start()
        {
            popupUI = FindObjectOfType<PopupUI>();
            popupUI.Hide();

            PlayerPrefsDataMix.instance.LoadGold(); //load gold from data
            randomGoldBtn.onClick.AddListener(RandomGold);
        }

        void RandomGold()
        {
            PlayerPrefsDataMix.instance.gold = Random.Range(0, 1000);
        }
        void UpdateGoldData(int value)
        {
            goldDisplayer.text = value.ToString();
        }


        void GetSetData()
        {
            //get gold 
            int gold = PlayerPrefsDataMix.instance.gold;

            //change gold
            PlayerPrefsDataMix.instance.gold = Random.Range(0, 1000);

        }
    }
}