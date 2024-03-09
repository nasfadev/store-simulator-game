using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class FloorLevelButtonDown : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

    [SerializeField] private UnityEvent OnUp;
    [SerializeField] private UnityEvent OnUpDeleteMode;

    public void OnPointerDown(PointerEventData data)
    {

        if (FloorBuilder.Instance.floorLevel == 0)
        {
            return;
        }
        FloorBuilder.Instance.floorLevel--;
        if (!(PlaceButton.Instance.contentID > 0))
        {
            OnUpDeleteMode?.Invoke();
            return;
        }
        Debug.Log("level hit");
        OnUp?.Invoke();

    }
}
