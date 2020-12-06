using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    private Rigidbody2D rb;
    public GroundCheck groundCheck;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && groundCheck.canJump)
        {
            Vector2 jumpVector = new Vector2(0f, jumpForce);
            rb.AddForce(jumpVector, ForceMode2D.Impulse);
        }
        float velY = rb.velocity.y;

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, velY);
    }
}
