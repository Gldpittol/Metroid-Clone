using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeVisionScript : MonoBehaviour
{
    private bool AIStarted = false;

    private void OnEnable()
    {
        AIStarted = false;
    }

    private void OnDisable()
    {
        AIStarted = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !AIStarted)
        {
            GetComponentInParent<SkreeScript>().AIFinished = false;
            AIStarted = true;
            GetComponentInParent<SkreeScript>().SkreeAIFunction(this.gameObject);
        }
    }
}
