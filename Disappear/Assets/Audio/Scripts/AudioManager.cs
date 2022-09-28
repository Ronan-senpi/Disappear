using System;
using System.Collections;
using Audio.Scripts.Footsteps;
using UnityEngine;

namespace Audio.Scripts {
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private Transform freeAudioContainer;
    private Transform usedAudioContainer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        freeAudioContainer = transform.GetChild(0);
        usedAudioContainer = transform.GetChild(1);
    }

    public void PlaySpatializeSoundOnce(FootstepEvent fse, AudioClip spatializeClip, Vector3 position, float pitch)
    {
        if (freeAudioContainer.childCount > 0)
        {
            AudioSource sound = freeAudioContainer.GetChild(0).GetComponent<AudioSource>();
            sound.clip = spatializeClip;
            sound.transform.position = position;
            sound.pitch = pitch;
            sound.transform.parent = usedAudioContainer;
            sound.Play();
            StartCoroutine(ResetAudioSource(fse, sound, sound.clip.length));
        }
        else
        {
            throw new Exception("No AudioSource available");
        }
    }

    private IEnumerator ResetAudioSource(FootstepEvent fse, AudioSource source, float soundDuration)
    {
        yield return new WaitForSeconds(0.1f);
        fse.IsPlaying = false;
        yield return new WaitForSeconds(soundDuration);
        source.transform.parent = freeAudioContainer;

    }
}
}