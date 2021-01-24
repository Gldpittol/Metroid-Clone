using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAnimState
{
    Starting,
    Idle,
    IdleUp,
    IdleForward,
    Running,
    RunningGunUp,
    RunningShootingForward,
    JumpNormal,
    JumpSideways,
    JumpShootUp,
    JumpShootStraight,
    Fall,
    Crouch,
    Death
}
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    public EAnimState eAnimState;
    private CharacterMovement charMovement;
    private PlayerShoot playerShoot;
    private Animation anim;

    private Animation startAnimation;
    public Sprite starting;
    private string idle = "Idle";
    private string idleUp = "IdleGunUp";
    private string idleForward = "IdleGunSideways";
    private string running = "Running";
    private string runningGunUp = "RunningGunUp";
    private string runningGunSideways = "RunningGunSideways";
    private string jumpNormal = "Jumping";
    private string jumpSideways = "JumpSideways";
    private string jumpShootUp = "JumpShootUp";
    private string jumpShootStraight = "JumpShootStraight";

    private Animation fall;
    private string crouch = "StartCrouch";
    private Animation death;

    private float horizontal;
    private float vertical;
    private float frame;

    public static PlayerAnimations instance;
    void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        charMovement = GetComponent<CharacterMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        anim = GetComponent<Animation>();

        sr.sprite = starting;
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

        CheckAnimation();

    }

    public void CheckAnimation()
    {
        if(GameController.instance.eGameState == EGameState.GamePlay)
        {
            if (eAnimState == EAnimState.Idle)
            {
                if (horizontal != 0 && PlayerEnemyCollision.instance.canMoveHorizontally)
                {
                    animator.Play(running);
                    eAnimState = EAnimState.Running;
                }
            }

            if (eAnimState == EAnimState.Running || eAnimState == EAnimState.RunningGunUp || eAnimState == EAnimState.RunningShootingForward)
            {
                if (horizontal == 0)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                }

                if(!PlayerEnemyCollision.instance.canMoveHorizontally)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                }
            }

            if (eAnimState == EAnimState.JumpNormal || eAnimState == EAnimState.JumpSideways)
            {
                if (!charMovement.jumped && GroundCheck.instance.canJump)
                {
                    if (horizontal != 0)
                    {
                        animator.Play(running);
                        eAnimState = EAnimState.Running;

                        ChangeHitbox("Normal");
                    }

                    if (horizontal == 0)
                    {
                        animator.Play(idle);
                        eAnimState = EAnimState.Idle;
                       ChangeHitbox("Normal");
                    }
                }
            }

            if(eAnimState == EAnimState.JumpSideways)
            {
                if(Input.GetKey(KeyCode.Z))
                {
                    CharacterMovement.instance.jumpedSideways = false;
                    eAnimState = EAnimState.JumpNormal;
                    animator.Play(jumpNormal);
                    ChangeHitbox("JumpNormal");
                }

                if (!PlayerEnemyCollision.instance.canMoveHorizontally)
                {
                    CharacterMovement.instance.jumpedSideways = false;
                    eAnimState = EAnimState.JumpNormal;
                    animator.Play(jumpNormal);
                    ChangeHitbox("JumpNormal");
                }
            }

            if (eAnimState == EAnimState.Running || eAnimState == EAnimState.RunningGunUp || eAnimState == EAnimState.RunningShootingForward || 
                eAnimState == EAnimState.Idle || eAnimState == EAnimState.IdleUp || eAnimState == EAnimState.IdleForward)
            {
                if (charMovement.jumped)
                {
                    if (horizontal == 0)
                    {
                        animator.Play(jumpNormal);
                        eAnimState = EAnimState.JumpNormal;
                        ChangeHitbox("JumpNormal");
                    }
                    else
                    {
                        animator.Play(jumpSideways);
                        eAnimState = EAnimState.JumpSideways;
                        ChangeHitbox("JumpSideways");
                    }
                }

                else if(charMovement.jumped == false && !GroundCheck.instance.canJump)
                {
                    animator.Play(jumpNormal);
                    eAnimState = EAnimState.JumpNormal;
                    ChangeHitbox("JumpNormal");

                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && CharacterMovement.instance.gotCrouchBall)
                {
                    animator.Play(crouch);
                    eAnimState = EAnimState.Crouch;
                    ChangeHitbox("Crouch");
                }
            }

            if (eAnimState == EAnimState.Running)
            {
                if (vertical > 0)
                {
                    animator.Play(runningGunUp, 0 ,frame);

                    eAnimState = EAnimState.RunningGunUp;
                }

                if (playerShoot.isShooting && vertical == 0)
                {
                    animator.Play(runningGunSideways, 0, frame);
                    eAnimState = EAnimState.RunningShootingForward;
                }
            }

            if (eAnimState == EAnimState.RunningGunUp)
            {
                if (playerShoot.isShooting && vertical == 0)
                {
                    animator.Play(runningGunSideways, 0, frame);
                    eAnimState = EAnimState.RunningShootingForward;
                }

                if (!playerShoot.isShooting && vertical == 0)
                {
                    animator.Play(running, 0, frame);
                    eAnimState = EAnimState.Running;
                }
            }

            if (eAnimState == EAnimState.RunningShootingForward)
            {
                if (playerShoot.isShooting && vertical > 0)
                {
                    animator.Play(runningGunUp, 0, frame);
                    eAnimState = EAnimState.RunningGunUp;
                }

                if (!playerShoot.isShooting && vertical == 0)
                {
                    animator.Play(running, 0, frame);
                    eAnimState = EAnimState.Running;
                }
            }

            if (eAnimState == EAnimState.Idle)
            {
                if (vertical > 0 && horizontal == 0)
                {
                    animator.Play(idleUp);
                    eAnimState = EAnimState.IdleUp;
                }

                if (playerShoot.isShooting && vertical == 0 && horizontal == 0)
                {
                    animator.Play(idleForward);
                    eAnimState = EAnimState.IdleForward;
                }
            }

            if (eAnimState == EAnimState.IdleUp)
            {
                if (horizontal != 0)
                {
                    animator.Play(runningGunUp);
                    eAnimState = EAnimState.RunningGunUp;
                }

                else if (playerShoot.isShooting && vertical == 0 && horizontal == 0)
                {
                    animator.Play(idleForward);
                    eAnimState = EAnimState.IdleForward;
                }

                else if (!playerShoot.isShooting && vertical == 0 && horizontal == 0)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                }
            }

            if (eAnimState == EAnimState.IdleForward)
            {
                if(horizontal != 0)
                {
                    animator.Play(runningGunSideways);
                    eAnimState = EAnimState.RunningShootingForward;
                }

                else if (playerShoot.isShooting && vertical > 0 && horizontal == 0)
                {
                    animator.Play(idleUp);
                    eAnimState = EAnimState.IdleUp;
                }

                else if (!playerShoot.isShooting && vertical == 0 && horizontal == 0)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                }
            }

            if (eAnimState == EAnimState.Crouch)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && CeilingCheck.instance.canGetUp)
                {
                    if (vertical > 0 && GroundCheck.instance.canJump)
                    {
                        if (horizontal != 0)
                        {
                            animator.Play(running);
                            eAnimState = EAnimState.Running;
                            ChangeHitbox("Normal");
                        }
                        if (horizontal == 0)
                        {
                            animator.Play(idle);
                            eAnimState = EAnimState.Idle;
                            ChangeHitbox("Normal");
                        }
                    }
                    else if (!GroundCheck.instance.canJump)
                    {
                        animator.Play(jumpNormal);
                        eAnimState = EAnimState.JumpNormal;

                        CharacterMovement.instance.jumpedSideways = false;
                        ChangeHitbox("Normal");
                    }
                }
            }  

            if(eAnimState == EAnimState.JumpNormal)
            {
                if(playerShoot.holdingUp)
                {
                    animator.Play(jumpShootUp);
                    eAnimState = EAnimState.JumpShootUp;
                    ChangeHitbox("JumpNormal");
                }

                else if(playerShoot.isShooting)
                {
                    animator.Play(jumpShootStraight);
                    eAnimState = EAnimState.JumpShootStraight;
                    ChangeHitbox("JumpNormal");
                }
            }

            if (eAnimState == EAnimState.JumpShootUp)
            {
                if (!playerShoot.holdingUp && !playerShoot.isShooting)
                {
                    animator.Play(jumpNormal);
                    eAnimState = EAnimState.JumpNormal;
                    ChangeHitbox("JumpNormal");
                }

                else if (!playerShoot.holdingUp && playerShoot.isShooting)
                {
                    animator.Play(jumpShootStraight);
                    eAnimState = EAnimState.JumpShootStraight;
                    ChangeHitbox("JumpNormal");
                }
                if (!charMovement.jumped && GroundCheck.instance.canJump)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                    ChangeHitbox("Normal");
                }
            }


            if (eAnimState == EAnimState.JumpShootStraight)
            {
                if (playerShoot.holdingUp)
                {
                    animator.Play(jumpShootUp);
                    eAnimState = EAnimState.JumpShootUp;
                    ChangeHitbox("JumpNormal");
                }

                else if (!playerShoot.holdingUp && !playerShoot.isShooting)
                {
                    animator.Play(jumpNormal);
                    eAnimState = EAnimState.JumpNormal;
                    ChangeHitbox("JumpNormal");
                }
                
                if(!charMovement.jumped && GroundCheck.instance.canJump)
                {
                    animator.Play(idle);
                    eAnimState = EAnimState.Idle;
                    ChangeHitbox("Normal");
                }
            }
        }
    }

    public void ChangeHitbox(string size)
    {

        switch (size)
        {


            case "Crouch":
                GetComponent<BoxCollider2D>().size = new Vector2(0.7294931f, 0.8070598f);
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.2203183f, -0.7474638f);

               break;

            case "Normal":
                GetComponent<BoxCollider2D>().size = new Vector2(0.7294931f, 1.862906f);
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.2203183f, -0.2195407f);
                break;

            case "JumpSideways":
                GetComponent<BoxCollider2D>().size = new Vector2(0.7294931f, 1.321976f);
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.2203183f, -0.4900056f);
                break;


            case "JumpNormal":
                GetComponent<BoxCollider2D>().size = new Vector2(0.7294931f, 1.862906f);
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.2203183f, -0.2195407f);
                break;
        }
    }
}
