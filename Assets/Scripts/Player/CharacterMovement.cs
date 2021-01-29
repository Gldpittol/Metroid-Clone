using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;

    [Header("Config Parameters")]
    public float playerSpeed;
    public float jumpForce;
    public float highJumpForce;
    public float horizontal;
    public float doubleHeightDelay;
    public float delayBetweenMovementClips;
    private float currentDelay = 0;
    private float currentDelayBetweenClips;
    [HideInInspector] public bool canCrouch = true;
    [HideInInspector] public bool hasJumped = false;
    [HideInInspector] public bool jumped;
    [HideInInspector] public bool jumpedSideways;
    [HideInInspector] public bool gotCrouchBall = false;
    [HideInInspector] public bool canStart = false;

    [Header("References")]
    private Rigidbody2D rb;
    private PlayerAnimations playerAnim;
    private AudioSource audSource;
    public AudioClip movementClip;
    public AudioClip jumpClip;

    private void Awake()
    {
        instance = this;
        playerAnim = GetComponent<PlayerAnimations>();
        audSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameController.instance.eGameState == EGameState.GamePlay)
        {
           if(GroundCheck.instance.canJump) horizontal = Input.GetAxisRaw("Horizontal");
            else horizontal = Input.GetAxis("Horizontal");

            if (playerAnim.eAnimState != EAnimState.Crouch)
            {
                if ((Input.GetKey(KeyCode.X)) && GroundCheck.instance.canJump && !hasJumped && PlayerEnemyCollision.instance.canMoveHorizontally)
                {
                    currentDelay += Time.deltaTime;
                    if (currentDelay >= doubleHeightDelay)
                    {
                        currentDelay = 0;
                        Vector2 jumpVector = new Vector2(0, highJumpForce);
                        rb.AddForce(jumpVector, ForceMode2D.Impulse);
                        audSource.PlayOneShot(jumpClip);
                        hasJumped = true;
                        jumped = true;
                        GroundCheck.instance.cooldown = 0;

                        if (horizontal != 0) jumpedSideways = true;
                        else jumpedSideways = false;
                    }
                }

                if (Input.GetKeyUp(KeyCode.X) && PlayerEnemyCollision.instance.canMoveHorizontally)
                {
                    if (currentDelay < doubleHeightDelay && GroundCheck.instance.canJump && !hasJumped)
                    {
                        float newJumpForce = jumpForce + (highJumpForce - jumpForce) * (currentDelay / doubleHeightDelay);
                        //print(newJumpForce);
                        if (newJumpForce < 7.5f) newJumpForce = 7.5f;

                        Vector2 jumpVector = new Vector2(0, newJumpForce);
                        //Vector2 jumpVector = new Vector2(0, jumpForce);
                        rb.AddForce(jumpVector, ForceMode2D.Impulse);
                        audSource.PlayOneShot(jumpClip);
                        jumped = true;
                        GroundCheck.instance.cooldown = 0;

                        if (horizontal != 0) jumpedSideways = true;
                        else jumpedSideways = false;
                    }
                    currentDelay = 0;
                    hasJumped = false;
                }
            }

            if(playerAnim.eAnimState != EAnimState.JumpSideways)
            {
                if (horizontal < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                if (horizontal > 0)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, 1f, 1f);
                }
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) jumped = false;
    }


    private void FixedUpdate()
    {
        if(GameController.instance.eGameState == EGameState.GamePlay)
        {
            if (PlayerEnemyCollision.instance.canMoveHorizontally)
            {
                if (playerAnim.eAnimState != EAnimState.JumpSideways)
                {
                    rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
                }
                else
                {
                    if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                    else
                        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
                }

                currentDelayBetweenClips += Time.deltaTime;

                if (currentDelayBetweenClips > delayBetweenMovementClips && horizontal != 0 && GroundCheck.instance.canJump) 
                {
                    currentDelayBetweenClips = 0;
                    audSource.PlayOneShot(movementClip);
                }
            }
        }
    }

    public void StartGame()
    {
        canStart = true;
    }
}
