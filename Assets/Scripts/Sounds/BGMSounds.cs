
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClips;
    private int currentSoundId;
    private void Start()
    {
        StartCoroutine(Run());
    }
    private IEnumerator Run()
    {

        while (true)
        {
            if (currentSoundId == 0)
            {
                int n = audioClips.Length;


                for (int i = n - 1; i > 0; i--)
                {

                    int j = Random.Range(0, i + 1);

                    AudioClip temp = audioClips[i];
                    audioClips[i] = audioClips[j];
                    audioClips[j] = temp;
                    yield return null;
                }

            }
            audioSource.PlayOneShot(audioClips[currentSoundId]);
            yield return new WaitForSeconds(audioClips[currentSoundId].length);
            currentSoundId++;
            if (currentSoundId == audioClips.Length)
            {
                currentSoundId = 0;
            }

        }
    }
}
