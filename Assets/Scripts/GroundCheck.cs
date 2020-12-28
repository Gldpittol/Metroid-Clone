using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool canJump = false;
    public static GroundCheck instance;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        transform.position = CharacterMovement.instance.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
