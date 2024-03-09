using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CashierProductPackingPrefab : MonoBehaviour
{
    [Header("Previews")]
    public Vector3 EndPos;
    public float speed;
    private bool _isinit;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        Animate();
    }
    private void Animate()
    {
        CashierProductPacking data = CashierProductPacking.Instance;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one, speed / 4).SetEase(Ease.OutBack));
        seq.Append(transform.DOLocalMoveZ(EndPos.z, speed).SetEase(Ease.InOutQuad));
        seq.Append(transform.DOScale(Vector3.zero, speed / 4).SetEase(Ease.InBack));
        seq.OnComplete(() => { Destroy(gameObject); });

    }
    private void OnEnable()
    {
        if (_isinit)
        {
            Destroy(gameObject);
            return;
        }
        _isinit = true;

    }
}
