using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameController.instance.playerHealth -= 3;
            if (GameController.instance.playerHealth <= 0)
                Destroy(this.gameObject);
        }
    }
}
