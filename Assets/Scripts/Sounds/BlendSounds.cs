using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool vibrate;
    [SerializeField] private BlendSoundsData[] blendSounds;

    [System.Serializable]
    public class BlendSoundsData
    {
        public AudioClip audioClip;
        public float playWhenPreAudioclip;
    }
    public void run()
    {
        StartCoroutine(RunIE());
    }
    private IEnumerator RunIE()
    {
        if (vibrate)
        {
            Handheld.Vibrate();

        }

        for (int i = 0; i < blendSounds.Length; i++)
        {
            audioSource.PlayOneShot(blendSounds[i].audioClip);
            float waitTime = i + 1 >= blendSounds.Length ? 0f : blendSounds[i + 1].playWhenPreAudioclip;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
