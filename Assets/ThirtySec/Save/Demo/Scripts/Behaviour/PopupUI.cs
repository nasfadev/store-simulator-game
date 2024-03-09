using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThirtySec
{

    public class PopupUI : MonoBehaviour
    {
        public Text goldDisplayer;


        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        void OnEnable()
        {
            PlayerPrefsDataMix.onGoldValueChanged += UpdateGoldData;
        }

        void OnDisable()
        {
            PlayerPrefsDataMix.onGoldValueChanged -= UpdateGoldData;
        }

        void UpdateGoldData(int value)
        {
            goldDisplayer.text = value.ToString(); //gold will automatic update value because we subscribed onGoldValueChanged.
        }
    }
}