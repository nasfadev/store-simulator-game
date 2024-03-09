using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RayPointerPickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTweenTime;

    [SerializeField] private UnityEvent WereHouseShelves;

    private RayPointerAble.Type tempTypeMode;
    private bool isDown;
    private IEnumerator Run()
    {
        while (true)
        {
            if (Player.Instance.mode != Player.Mode.Free)
            {
                yield return null;
                continue;
            }
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                RayPointerAble rayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                // kalo tidak ditemukan objek RayPointerAble
                if (rayPointerAble == null)
                {
                    yield return null;
                    continue;
                }
                // kalo ditemukan RayPointerAble
                // munculkan tombol
                Appear();
                tempTypeMode = rayPointerAble.mode;
                if (tempTypeMode == RayPointerAble.Type.WareHouseShelves)
                {
                    yield return WareHouseShelves();
                }

                // hilangkan tombol
                Disappear();

            }
            yield return null;
        }
    }
    private void Appear()
    {
        canvas.enabled = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeTweenTime);
    }
    private void Disappear()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, fadeTweenTime)
        .OnComplete(() => { canvas.enabled = false; });
        isDown = false;
    }
    private IEnumerator WareHouseShelves()
    {
        while (true)
        {
            // kalo kepencet
            if (isDown)
            {

                WereHouseShelves?.Invoke();
                Player.Instance.mode = Player.Mode.PickCardBoard;
                break;

            }
            // cek raycast hit terbaru
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                RayPointerAble rayPointerAble = hit.transform.GetComponent<RayPointerAble>();
                //kalo typenya beda
                if (rayPointerAble.mode != tempTypeMode)
                {

                    //balik ke Run Croutine
                    break;
                }

            }
            // kalo raycast gak terdetek
            else
            {

                //balik ke Run Croutine
                break;
            }

            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isDown = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        isDown = false;
    }

}
