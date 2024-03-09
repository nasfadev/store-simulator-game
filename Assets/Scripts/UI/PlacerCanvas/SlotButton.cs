using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class SlotButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UnityEvent onDown;
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultSlotButtonImage;

    private void Start()
    {
        PlaceButton.Instance.TouchedWhenModeMode += Disable;

    }
    public void ChangeImage(Sprite images)
    {

        // int contentID = PlaceButton.Instance.contentID;
        // Debug.Log($"contentid = {contentID}");
        // if (contentID == 0 || contentID == -1)
        // {
        //     GetComponent<Image>().sprite = images;
        //     image.gameObject.SetActive(false);
        // }
        // else
        // {
        //     GetComponent<Image>().sprite = defaultSlotButtonImage;

        //     image.gameObject.SetActive(true);
        //     image.sprite = images;

        // }
    }
    public void OnPointerDown(PointerEventData data)
    {

        onDown?.Invoke();


    }
    private void Disable()
    {
        gameObject.SetActive(false);

    }
    private void Enable()
    {
        gameObject.SetActive(true);

    }
    public void CheckIsMovingMode()
    {
        if (PlaceButton.Instance.isMoving)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

        }
    }
}
