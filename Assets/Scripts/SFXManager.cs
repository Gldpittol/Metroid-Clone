using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource audSource;
    public AudioSource shootSource;


    private void Awake()
    {
        instance = this;
    }
    public void PlaySFX(AudioClip audClip)
    {
        audSource.PlayOneShot(audClip);
    }    
    public void PlayShoot(AudioClip audClip)
    {
        shootSource.PlayOneShot(audClip);
    }
}
