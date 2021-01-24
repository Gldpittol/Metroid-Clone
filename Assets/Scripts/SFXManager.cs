using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource audSource;
    public static SFXManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void PlaySFX(AudioClip audClip)
    {
        audSource.PlayOneShot(audClip);
    }
}
