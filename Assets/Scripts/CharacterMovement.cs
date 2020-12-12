using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    private Rigidbody2D rb;

    public static CharacterMovement instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && GroundCheck.instance.canJump)
        {
            Vector2 jumpVector = new Vector2(0, jumpForce);
            rb.AddForce(jumpVector, ForceMode2D.Impulse);
        }    
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, rb.velocity.y);
    }
}
