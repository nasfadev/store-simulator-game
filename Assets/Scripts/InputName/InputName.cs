using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Lean.Localization;
public class InputName : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private StoreData storeData;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private LeanToken token;
    [Header("Configs")]
    [SerializeField] private Canvas canvas;
    private void Start()
    {
        Debug.Log("inputname" + (storeData.data.name == ""));
        if (storeData.data.name == "")
        {
            canvas.enabled = true;
            return;
        }
        token.Value = storeData.data.name;
    }
    public void ChangeName()
    {
        storeData.data.name = inputField.text;
        token.Value = storeData.data.name;

    }
}
