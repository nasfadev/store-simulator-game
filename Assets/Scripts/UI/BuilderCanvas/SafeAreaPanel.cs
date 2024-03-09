using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        float targetWidth = 2340f;


        float currentWidth = Screen.width;


        float scaleFactor = targetWidth / currentWidth;

        float newX = (currentWidth - 200f) * scaleFactor;

        if ((Screen.width - Screen.safeArea.width) > 100)
        // if (101 > 100)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width - ((Screen.width - Screen.safeArea.width) * 2), 0f);
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(newX, 0f);

        }

    }



}
