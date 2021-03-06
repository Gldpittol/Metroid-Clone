﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static PlayerShoot instance;

    [Header("Parameters")]
    public float delayBetweenShots;
    private float currentDelay;
    private float horizontal;
    public bool holdingUp;
    public bool isShooting;

    [Header("References")]
    public GameObject playerBulletPrefab;
    public GameObject bulletDestruction;
    public GameObject playerWeapon;
    public GameObject playerWeaponIdle;
    public GameObject playerWeaponUp;
    public GameObject playerWeaponJumping;
    public GameObject playerWeaponJumpingUp;
    private PlayerAnimations playerAnim;

    private void Awake()
    {
        instance = this;
        playerAnim = GetComponent<PlayerAnimations>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            holdingUp = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            holdingUp = false;
        }

        if (GameController.instance.eGameState == EGameState.GamePlay)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            currentDelay += Time.deltaTime;

            if (playerAnim.eAnimState != EAnimState.Crouch)
            {
                if (Input.GetKey(KeyCode.Z) && currentDelay >= delayBetweenShots && !CharacterMovement.instance.jumpedSideways)
                {
                    isShooting = true;

                    currentDelay = 0;

                    if (GroundCheck.instance.canJump)
                    {
                        if (!holdingUp)
                        {
                            if (horizontal == 0)
                            {
                                if (CharacterMovement.instance.transform.localScale.x > 0)
                                    Instantiate(playerBulletPrefab, playerWeaponIdle.transform.position, Quaternion.identity);
                                else
                                    Instantiate(playerBulletPrefab, playerWeaponIdle.transform.position, Quaternion.Euler(0f, 0f, 180f));
                            }
                            else
                            {
                                if (CharacterMovement.instance.transform.localScale.x > 0)
                                    Instantiate(playerBulletPrefab, playerWeapon.transform.position, Quaternion.identity);
                                else
                                    Instantiate(playerBulletPrefab, playerWeapon.transform.position, Quaternion.Euler(0f, 0f, 180f));
                            }

                        }
                        else
                        {
                            Instantiate(playerBulletPrefab, playerWeaponUp.transform.position, Quaternion.Euler(0f, 0f, 90f));
                        }
                    }

                    else
                    {
                        if (!holdingUp)
                        {

                            if (CharacterMovement.instance.transform.localScale.x > 0)
                                Instantiate(playerBulletPrefab, playerWeaponJumping.transform.position, Quaternion.identity);
                            else
                                Instantiate(playerBulletPrefab, playerWeaponJumping.transform.position, Quaternion.Euler(0f, 0f, 180f));
                        }


                        else
                        {
                            Instantiate(playerBulletPrefab, playerWeaponJumpingUp.transform.position, Quaternion.Euler(0f, 0f, 90f));
                        }
                    }


                }
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                isShooting = false;
            }
        }   
    }


}
