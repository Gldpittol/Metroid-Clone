using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject playerBulletPrefab;
    public GameObject playerWeapon;
    public GameObject playerWeaponUp;

    public float delayBetweenShots;
    private float currentDelay;

    private bool holdingUp;

    void Update()
    {
        currentDelay += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            holdingUp = true;
        }

        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            holdingUp = false;
        }

        if(Input.GetKey(KeyCode.Z) && currentDelay >= delayBetweenShots)
        {
            currentDelay = 0;

            if(!holdingUp)
            {
                if (CharacterMovement.instance.transform.localScale.x > 0)
                    Instantiate(playerBulletPrefab, playerWeapon.transform.position, Quaternion.identity);
                else
                    Instantiate(playerBulletPrefab, playerWeapon.transform.position, Quaternion.Euler(0f, 0f, 180f));
            }
            else
            {
                Instantiate(playerBulletPrefab, playerWeaponUp.transform.position, Quaternion.Euler(0f, 0f, 90f));
            }
        }

    }
}
