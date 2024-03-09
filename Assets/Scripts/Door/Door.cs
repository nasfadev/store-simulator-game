using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Door : MonoBehaviour
{
    [SerializeField] private float rotateTweenTime;
    [SerializeField] private GameObject door;
    [SerializeField] private bool isDummyDoor;
    private Vector3 tempDoorPos;
    private void OnEnable()
    {
        if (isDummyDoor)
        {
            tempDoorPos = door.transform.localPosition;
            return;
        }

        DoorAndWindowBuilder.Instance.afterRender += Active;
        DoorAndWindowBuilder.Instance.beforeRender += Deactive;
    }
    private void OnDisable()
    {
        if (isDummyDoor)
        {
            return;
        }
        DoorAndWindowBuilder.Instance.afterRender -= Active;
        DoorAndWindowBuilder.Instance.beforeRender -= Deactive;
        Debug.Log("destroycuk");
    }
    private void Active()
    {
        door.SetActive(true);
    }
    private void Deactive()
    {
        Debug.Log("deactive");
        door.SetActive(false);
    }
    public void MoveToLeft()
    {
        door.transform.DOLocalMove(tempDoorPos + Vector3.back, rotateTweenTime).SetEase(Ease.InOutQuad);

    }
    public void MoveToRight()
    {
        door.transform.DOLocalMove(tempDoorPos + Vector3.forward, rotateTweenTime).SetEase(Ease.InOutQuad);

    }
    public void CloseDoorWithMove()
    {
        door.transform.DOLocalMove(tempDoorPos, rotateTweenTime).SetEase(Ease.InOutQuad);

    }
    public void rotatePlus90()
    {
        door.transform.DOLocalRotate(new Vector3(0, 90, 0), rotateTweenTime).SetEase(Ease.InOutQuad);
    }
    public void rotateMines90()
    {
        door.transform.DOLocalRotate(new Vector3(0, -90, 0), rotateTweenTime).SetEase(Ease.InOutQuad);
    }
    public void closeDoor()
    {
        door.transform.DOLocalRotate(new Vector3(0, 0, 0), rotateTweenTime).SetEase(Ease.InOutQuad);
    }
}
