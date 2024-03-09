using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Sprite clickedSprite;
    private Sprite tempSprite;
    private bool isClicked;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();

        tempSprite = image.sprite;
    }
    public void Click()
    {
        if (!isClicked)
        {
            image.sprite = clickedSprite;
            isClicked = true;
            return;
        }
        image.sprite = tempSprite;
        isClicked = false;
    }
}
