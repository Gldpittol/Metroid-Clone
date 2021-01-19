using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool canJump = false;
    public static GroundCheck instance;
    public float cooldown = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        transform.position = CharacterMovement.instance.transform.position;
        cooldown += Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground") && cooldown >= 0.1f)
        {
            canJump = true;
            CharacterMovement.instance.jumpedSideways = false;
            CharacterMovement.instance.jumped = false;
            //PlayerEnemyCollision.instance.canMoveHorizontally = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            PlayerEnemyCollision.instance.canMoveHorizontally = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
