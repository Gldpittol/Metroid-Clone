using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audSource;
    public void PlayAudio(AudioClip audClip)
    {
        audSource.Stop();
        audSource.clip = audClip;
        audSource.Play();
    }
}
