using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI FPSText;
    private float TimeCount;
    private float FPSCount;
    // Update is called once per frame
    private void Update()
    {
        TimeCount += Time.deltaTime;
        FPSCount++;
        if (TimeCount > 1f)
        {
            FPSText.text = $"{FPSCount}";
            TimeCount = 0f;
            FPSCount = 0f;
        }
    }
}
