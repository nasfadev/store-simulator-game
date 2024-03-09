using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class FloorLevelButtonUp : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

    [SerializeField] private UnityEvent OnUp;
    [SerializeField] private UnityEvent OnUpDeleteMode;

    public void OnPointerDown(PointerEventData data)
    {

        Debug.Log("level hit");

        if (FloorBuilder.Instance.floorLevel == 1)
        {
            return;
        }

        FloorBuilder.Instance.floorLevel++;
        if (!(PlaceButton.Instance.contentID > 0))
        {
            OnUpDeleteMode?.Invoke();
            return;
        }
        OnUp?.Invoke();


    }
}
