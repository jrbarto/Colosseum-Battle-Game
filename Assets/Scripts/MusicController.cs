using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource action1, waiting1;
    public float defaultVolume = 0.4f;
    public float loweringTransitionSeconds = 3.0f;
    public float raisingTransitionSeconds = 6.0f;

    public void PlayMusic(bool action)
    {
        AudioSource activeSource = action1;
        AudioSource targetSource = waiting1;

        if (action) {
            activeSource = waiting1;
            targetSource = action1;
        }
        StartCoroutine(FadeInMusic(activeSource, targetSource));
    }

    IEnumerator FadeInMusic(AudioSource activeSource, AudioSource targetSource)
    {
        float secondsLerped = 0;

        while (secondsLerped < loweringTransitionSeconds) {
            secondsLerped += Time.deltaTime;
            activeSource.volume = Mathf.Lerp(
                defaultVolume, 
                0, 
                secondsLerped / loweringTransitionSeconds
            );
            yield return null;
        }
        activeSource.Pause();

        if (targetSource.isPlaying == false) {
            targetSource.Play();
        }
        targetSource.UnPause();
        
        secondsLerped = 0;
        while (secondsLerped < raisingTransitionSeconds) {
            secondsLerped += Time.deltaTime;
            targetSource.volume = Mathf.Lerp(
                0, 
                defaultVolume, 
                secondsLerped / raisingTransitionSeconds
            );
            yield return null;
        }
    }
}
