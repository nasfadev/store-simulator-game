using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    public class PlayerPrefsDataMix : ThirtySec.Serializable<PlayerPrefsDataMix>
    {
        public static event System.Action<int> onGoldValueChanged = delegate { };

        public int gold
        {
            get
            {
                return ThirtySec.PlayerPrefs.GetInt("Gold");
            }
            set
            {
                ThirtySec.PlayerPrefs.SetInt("Gold", value);
                onGoldValueChanged(value);
            }
        }


        public void LoadGold()
        {
            onGoldValueChanged(gold); //update your gold.
        }

    }
}