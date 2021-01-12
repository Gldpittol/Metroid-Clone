using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public float highJumpForce;
    private Rigidbody2D rb;

    public float horizontal;

    public static CharacterMovement instance;

    public float doubleHeightDelay;
    private float currentDelay = 0;
    [HideInInspector]public bool hasJumped = false;

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

        if((Input.GetKey(KeyCode.LeftShift)) && GroundCheck.instance.canJump && !hasJumped)
        {
            currentDelay += Time.deltaTime;
            if (currentDelay >= doubleHeightDelay)
            {
                currentDelay = 0;
                Vector2 jumpVector = new Vector2(0, highJumpForce);
                rb.AddForce(jumpVector, ForceMode2D.Impulse);
                hasJumped = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            if(currentDelay < doubleHeightDelay && GroundCheck.instance.canJump && !hasJumped)
            {
                Vector2 jumpVector = new Vector2(0, jumpForce);
                rb.AddForce(jumpVector, ForceMode2D.Impulse);
            }
            currentDelay = 0;
            hasJumped = false;
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
