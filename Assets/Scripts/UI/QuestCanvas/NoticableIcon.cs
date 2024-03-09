using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticableIcon : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasScaler canvasScaler;
    [Header("Configs")]
    [SerializeField] private bool isImportant;
    [Header("Preview")]
    public Transform target;
    private Vector3 pos;
    private float rectWidth;
    private float rectHeight;
    private float maxScreeWidth;
    private float minScreeWidth;
    private float maxScreeHeight;
    private float minScreeHeight;

    private void Start()
    {
        float rectFactorScale = Screen.width / canvasScaler.referenceResolution.x;
        rectWidth = rectTransform.sizeDelta.x * rectFactorScale / 2;
        rectHeight = rectTransform.sizeDelta.y * rectFactorScale / 2;
        minScreeWidth = 0 + rectWidth;
        maxScreeWidth = Screen.width - rectWidth;
        minScreeHeight = 0 + rectHeight;
        maxScreeHeight = Screen.height - rectHeight;
    }
    public void SetTarget(Transform tr)
    {
        target = tr;
    }
    private void LateUpdate()
    {
        if (Camera.main == null)
        {
            return;
        }
        pos = Camera.main.WorldToScreenPoint(target.position, Camera.MonoOrStereoscopicEye.Mono);


        if (pos.x > minScreeWidth && pos.x < maxScreeWidth && pos.y > minScreeHeight && pos.y < maxScreeHeight && pos.z >= 0)
        {
            rectTransform.position = pos;
        }
        else
        {
            if (pos.z < 0)
            {
                if (isImportant)
                {
                    float xPos = Mathf.Lerp(0, Screen.width, 1f - Mathf.Clamp(pos.x, minScreeWidth, maxScreeWidth) / Screen.width);

                    rectTransform.position = new Vector3(xPos, rectHeight, 0f);
                }
                else
                {
                    rectTransform.position = Vector3.one * 100000;

                }

            }
            else
            {
                rectTransform.position = new Vector3(Mathf.Clamp(pos.x, minScreeWidth, maxScreeWidth), Mathf.Clamp(pos.y, minScreeHeight, maxScreeHeight), 0);

            }
        }




    }
}
