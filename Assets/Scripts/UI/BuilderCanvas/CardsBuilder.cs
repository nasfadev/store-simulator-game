using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        SelectedButton.OnSelected += Disappear;
    }
    public void Appear()
    {
        gameObject.SetActive(true);
        SelectedButton.OnSelected -= Disappear;



    }
    private void Disappear()
    {
        gameObject.SetActive(false);

    }
    public void AddDelegate()
    {
        SelectedButton.OnSelected += Disappear;

    }
}
