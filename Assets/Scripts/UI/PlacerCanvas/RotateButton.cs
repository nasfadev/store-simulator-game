using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RotateButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public static int rotateID;
    [SerializeField] private UnityEvent OnUp;

    private void Awake()
    {
        rotateID = 1;
    }

    public void OnPointerDown(PointerEventData data)
    {
        rotateID++;
        rotateID = rotateID > 4 ? rotateID = 1 : rotateID;
        Debug.Log("Rotate ID" + rotateID);
        OnUp?.Invoke();

    }
    private void Update()
    {
#if UNITY_EDITOR
        OnPointerDownForWindows();

#elif UNITY_STANDALONE_WIN
                    OnPointerDownForWindows();


#elif UNITY_ANDROID
      return;
#endif

    }
    private void OnPointerDownForWindows()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotateID++;
            rotateID = rotateID > 4 ? rotateID = 1 : rotateID;
            Debug.Log("Rotate ID" + rotateID);
            OnUp?.Invoke();
        }

    }
}
