using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SammusDeath : MonoBehaviour
{
    public float delayBetweenRotations;

    public Vector2 force;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(RotateRoutine());
    }

    public IEnumerator RotateRoutine()
    {
        yield return new WaitForSeconds(delayBetweenRotations);
        transform.Rotate(0f, 0f, 90f);
        StartCoroutine(RotateRoutine());
    }
}
