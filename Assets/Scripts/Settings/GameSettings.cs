using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
public class GameSettings : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset urpasset;
    [SerializeField] private float renderScale;
    [SerializeField] private TMP_InputField inputField;
    public float tempRenderScale;
    private void Start()
    {
        tempRenderScale = urpasset.renderScale;
    }
    private void Update()
    {
        if (urpasset.renderScale != renderScale)
        {
            urpasset.renderScale = renderScale;
        }
    }
    private void OnDisable()
    {
        urpasset.renderScale = tempRenderScale;
    }
    public void ChangeRenderScale()
    {
        renderScale = int.Parse(inputField.text);
    }
}
