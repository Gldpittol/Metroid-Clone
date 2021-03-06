﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchBall : MonoBehaviour
{
    public float animationDuration = 2f;

    private bool firstTouch = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstTouch && collision.CompareTag("Player"))
            StartCoroutine(BallRoutine());
    }


    public IEnumerator BallRoutine()
    {
        GameController.instance.eGameState = EGameState.Cutscene;
        Camera.main.GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().enabled = true;

        firstTouch = false;
        Time.timeScale = 0.0f;

        GetComponent<Animator>().speed = 0f;

        yield return new WaitForSecondsRealtime(animationDuration);

        Time.timeScale = 1.0f;
        CharacterMovement.instance.gotCrouchBall = true;
        GameController.instance.eGameState = EGameState.GamePlay;
        Camera.main.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
