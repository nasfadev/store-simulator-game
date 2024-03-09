using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DummyAds : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Header("Configs")]
    [SerializeField] private float waitTime;
    [SerializeField] private UnityEvent whenBeforeRun;

    [SerializeField] private UnityEvent whenRewardEvent;
    [Header("Previews")]
    public UnityEvent rewardEvent;
    private bool isWait;
    public void Run()
    {
        if (isWait)
        {
            return;
        }
        whenBeforeRun?.Invoke();
        isWait = true;

        StartCoroutine(RunIE());
    }
    private IEnumerator RunIE()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            timerText.text = "" + (int)(time);
            if (time >= waitTime)
            {
                break;
            }
            yield return null;
        }
        rewardEvent?.Invoke();
        whenRewardEvent?.Invoke();
        isWait = false;
    }

}
