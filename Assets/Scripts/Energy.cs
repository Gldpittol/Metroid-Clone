using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public int energyToAdd = 5;
    private bool canAddEnergy = true;

    public AudioClip energyPickUpClip;
    public AudioSource audSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && canAddEnergy)
        {
            canAddEnergy = false;

            GameController.instance.playerHealth += energyToAdd;

            audSource.PlayOneShot(energyPickUpClip);

            if (GameController.instance.playerHealth > 99)
            {
                GameController.instance.playerHealth = 99;
            }

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            Destroy(gameObject,1f);
        }
    }
}
