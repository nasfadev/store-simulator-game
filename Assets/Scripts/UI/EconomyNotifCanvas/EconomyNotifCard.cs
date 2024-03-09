using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
public class EconomyNotifCard : MonoBehaviour
{
    [SerializeField] private float moveTweenTime;
    [SerializeField] private float fadeTweenTime;
    public RectTransform thisRect;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI notifText;
    public Image spriteImage;
    public int slotQueueNumber;
    private void Start()
    {
        if (spriteImage.sprite != null)
        {
            spriteImage.gameObject.SetActive(true);
        }
        StartCoroutine(Run());
    }
    private IEnumerator Run()
    {
        canvasGroup.DOFade(1f, fadeTweenTime);
        EconomyNotif economyNotif = EconomyNotif.Instance;
        while (true)
        {
            if (slotQueueNumber == 0)
            {
                yield return new WaitForSeconds(1f);
                canvasGroup.DOFade(0, fadeTweenTime).OnComplete(
                    () =>
                    {
                        economyNotif.slotClaim[slotQueueNumber] = false;
                        economyNotif.slotQueueNumber--;
                        economyNotif.ECdata.RemoveAt(0);
                        Destroy(gameObject);

                    });
                break;

            }
            if (!economyNotif.slotClaim[slotQueueNumber - 1])
            {
                economyNotif.slotClaim[slotQueueNumber] = false;
                slotQueueNumber--;
                economyNotif.slotClaim[slotQueueNumber] = true;

                thisRect.DOAnchorPos(economyNotif.slotRect[slotQueueNumber].anchoredPosition, moveTweenTime);
            }
            yield return null;

        }
    }
}
