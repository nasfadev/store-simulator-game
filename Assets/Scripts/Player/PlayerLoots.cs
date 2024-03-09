using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
public class PlayerLoots : MonoBehaviour
{
    [SerializeField] private float startRotY;
    [SerializeField] private float targetRotY;
    [SerializeField] private float rotateTweenTime;
    public Mode mode;
    [SerializeField] private GameObject[] LootsObj;
    private GameObject tempLoot;
    public enum Mode
    {
        Free = -1,
        Cardboard = 0,
        Broom = 1
    }
    public void ChangeLoot(PlayerChangeLootsMode playerChangeLootsMode)
    {
        mode = playerChangeLootsMode.mode;

        if (mode == Mode.Free)
        {
            Free();
            return;
        }
        else
        {
            Change();
        }
    }
    private void Free()
    {
        if (tempLoot != null)
        {
            transform.DOLocalRotate(Vector3.right * startRotY, rotateTweenTime).SetEase(Ease.InQuad)
            .OnComplete(() => { tempLoot.SetActive(false); });
        }
    }
    private void Change()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(Vector3.right * startRotY, rotateTweenTime).
        SetEase(Ease.InQuad).OnComplete(() =>
        { if (tempLoot != null) tempLoot.SetActive(false); tempLoot = LootsObj[(int)mode]; tempLoot.SetActive(true); }));
        seq.Append(transform.DOLocalRotate(Vector3.right * targetRotY, rotateTweenTime).
        SetEase(Ease.OutBack));


    }


}
