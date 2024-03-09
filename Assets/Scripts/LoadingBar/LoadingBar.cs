using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LoadingBar : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private Image _fillImage;
    [Header("Configs")]
    [SerializeField] private float _offsetWait;
    private void Start()
    {
        StartCoroutine(WaitIE());
    }
    private IEnumerator WaitIE()
    {
        bool isFinish = false;
        _fillImage.fillAmount = 0;
        _fillImage.DOFillAmount(1f, _offsetWait).OnComplete(() => isFinish = true);
        while (!isFinish)
        {
            yield return null;
        }
        while (true)
        {

            if (StateLoaded.Loaded == StateLoaded.maxLoaded)
            {
                break;
            }
            yield return null;

        }
        gameObject.SetActive(false);
    }
}
