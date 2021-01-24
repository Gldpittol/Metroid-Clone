using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public int energyToAdd = 5;
    private bool canAddEnergy = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && canAddEnergy)
        {
            canAddEnergy = false;

            GameController.instance.playerHealth += energyToAdd;

            if (GameController.instance.playerHealth > 99)
            {
                GameController.instance.playerHealth = 99;
            }

            Destroy(gameObject);
        }
    }
}
