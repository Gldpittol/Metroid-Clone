using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    private Rigidbody2D rb;

    public float horizontal;

    public static CharacterMovement instance;

    public float doubleHeightDelay;
    public float highJumpForce;
    private float currentDelay = 0;

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

        horizontal = Input.GetAxisRaw("Horizontal");

        if((Input.GetKey(KeyCode.LeftShift)) && GroundCheck.instance.canJump)
        {
            currentDelay += Time.deltaTime;
            if (currentDelay >= doubleHeightDelay)
            {
                currentDelay = 0;
                Vector2 jumpVector = new Vector2(0, highJumpForce);
                rb.AddForce(jumpVector, ForceMode2D.Impulse);
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            if(currentDelay < doubleHeightDelay && GroundCheck.instance.canJump)
            { 
                Vector2 jumpVector = new Vector2(0, jumpForce);
                rb.AddForce(jumpVector, ForceMode2D.Impulse);
            }
            currentDelay = 0;
        }

        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if(horizontal > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
    }
}
