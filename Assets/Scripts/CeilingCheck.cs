using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
    public static CeilingCheck instance;
    [HideInInspector] public bool canGetUp = true;
    private Vector3 offset;
    private void Awake()
    {
        instance = this;
        offset = new Vector3(0, 1.4f, 0);
    }
    private void Update()
    {
        transform.position = CharacterMovement.instance.transform.position + offset;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canGetUp = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canGetUp = true;
        }
    }
}
