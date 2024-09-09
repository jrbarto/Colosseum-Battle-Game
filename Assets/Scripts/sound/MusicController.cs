using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource action1, waiting1, death1, pause1;
    public float defaultVolume = 0.4f;
    public float loweringTransitionSeconds = 3.0f;
    public float raisingTransitionSeconds = 6.0f;
    public enum MusicTheme
    {
        Action,
        Waiting,
        Death,
        Pause
    }
    private AudioSource activeSource;
    private AudioSource stashedSource;

    public void PlayMusic(MusicTheme theme, bool expedite = false)
    {
        AudioSource targetSource = action1;
        if (!GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerCombatController>().alive) {
            targetSource = death1;
        } else if (theme == MusicTheme.Waiting) {
            targetSource = waiting1;
        } else if (theme == MusicTheme.Pause) {
            targetSource = pause1;
        }

        StartCoroutine(FadeInMusic(activeSource, targetSource, expedite));
        activeSource = targetSource;
    }

    public void StashSource()
    {
        stashedSource = activeSource;
    }

    public void ResumeStashedSource(bool expedite = false)
    {
        StartCoroutine(FadeInMusic(activeSource, stashedSource, expedite));
        activeSource = stashedSource;
    }

    IEnumerator FadeInMusic(AudioSource activeSource, AudioSource targetSource, bool expedite)
    {
        float secondsLerped = 0;
        float loweringTransition = loweringTransitionSeconds;
        float raisingTransition = raisingTransitionSeconds;
        if (expedite) {
            loweringTransition = loweringTransition / 3;
            raisingTransition = raisingTransition / 3;
        }

        if (activeSource != null) {
            while (secondsLerped < loweringTransition) {
                // uses unscaledDeltaTime because it is not affected by Time.timeScale during pause
                secondsLerped += Time.unscaledDeltaTime;
                activeSource.volume = Mathf.Lerp(
                    defaultVolume, 
                    0, 
                    secondsLerped / loweringTransition
                );
                yield return null;
            }
            activeSource.Pause();
        }

        if (targetSource.isPlaying == false) {
            targetSource.Play();
        }
        targetSource.UnPause();
        
        secondsLerped = 0;
        while (secondsLerped < raisingTransition) {
            secondsLerped += Time.unscaledDeltaTime;
            targetSource.volume = Mathf.Lerp(
                0, 
                defaultVolume, 
                secondsLerped / raisingTransition
            );
            yield return null;
        }
    }
}
