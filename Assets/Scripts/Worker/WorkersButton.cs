using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkersButton : MonoBehaviour
{
    [Header("Required Things")]

    [SerializeField] private Sprite defaultButton;
    [SerializeField] private Sprite clickedButton;
    [SerializeField] private Image image;
    [SerializeField] private bool isDefaultButton;

    public static WorkersButton Instance;
    [HideInInspector] public Image tempImage;
    [HideInInspector] public Sprite tempDefaultButton;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        if (isDefaultButton)
        {
            Instance.tempImage = image;
            Instance.tempDefaultButton = defaultButton;
        }
    }
    public void Clicked()
    {
        Debug.Log("bag click");

        if (BagCardGenerator.Instance.isGenerating)
        {
            return;
        }
        Debug.Log("bag click");
        if (Instance.tempImage != null)
        {
            Instance.tempImage.sprite = Instance.tempDefaultButton;

        }
        image.sprite = clickedButton;
        Instance.tempImage = image;
        Instance.tempDefaultButton = defaultButton;
    }
}
