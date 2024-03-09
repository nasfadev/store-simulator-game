using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class KeyDownControl : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private UnityEvent onKeyDown;
    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            this.enabled = false;

            onKeyDown?.Invoke();
        }
    }
}
