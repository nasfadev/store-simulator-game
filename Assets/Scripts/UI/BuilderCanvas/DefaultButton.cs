using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour
{


    // Start is called before the first frame update
    [SerializeField] private Sprite selectedImage;

    private void Awake()
    {
        GetComponent<Image>().sprite = selectedImage;

    }

}
