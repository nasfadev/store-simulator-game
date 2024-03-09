using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTriger : MonoBehaviour
{
    [SerializeField] private UnityEvent openDoorPlus90;
    [SerializeField] private UnityEvent openDoorMines90;
    [SerializeField] private UnityEvent closeDoor;

    [SerializeField] private bool isDummyDoor;

    private int status;
    private void Awake()
    {
        if (isDummyDoor)
        {
            return;
        }
        DoorAndWindowBuilder.Instance.resetDoor += ResetDoor;
    }
    private void ResetDoor()
    {
        status = 0;
        closeDoor?.Invoke();
    }
    private void OnDestroy()
    {
        if (isDummyDoor)
        {
            return;
        }
        DoorAndWindowBuilder.Instance.resetDoor -= ResetDoor;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (status == 0)
        {
            Vector3 targetPosUn = transform.InverseTransformPoint(other.transform.position);
            Vector2 targetPos = new Vector2(targetPosUn.x, targetPosUn.z);
            Vector2 polar = Polar(Vector2.zero, targetPos);
            Debug.Log($"angle {polar}");
            if (polar.y >= 0f && polar.y <= 180f)
            {
                openDoorPlus90?.Invoke();

            }
            else
            {
                openDoorMines90?.Invoke();

            }
        }
        status++;
        Debug.Log($"status {status}");
    }
    private void OnTriggerExit(Collider other)
    {
        status--;
        if (status <= 0)
        {
            status = 0;
            closeDoor?.Invoke();
        }
        Debug.Log($"status {status}");

    }
    private Vector2 Polar(Vector2 centerPos, Vector2 targetPos)
    {
        float centerX = centerPos.x; // koordinat x pusat
        float centerY = centerPos.y; // koordinat y pusat
        float x = targetPos.x; // koordinat x
        float y = targetPos.y; // koordinat y

        // menghitung nilai jarak dari pusat koordinat
        float deltaX = x - centerX;
        float deltaY = y - centerY;
        float r = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // menghitung sudut dengan menggunakan fungsi atan2
        float theta = Mathf.Atan2(deltaY, deltaX);

        // mengubah sudut dari radian ke derajat
        float angleInDegrees = ((theta * Mathf.Rad2Deg) + 270f) % 360f;

        return new Vector2(r, angleInDegrees);
    }
}
