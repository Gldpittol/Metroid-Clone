using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    public static PlayerEnemyCollision instance;
    private bool isInvulnerable;

    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    GameController.instance.playerHealth -= 3;
        //    if (GameController.instance.playerHealth <= 0)
        //        Destroy(this.gameObject);
        //}
    }

    public void DamagePlayer(int damageToPlayer)
    {
        if(!isInvulnerable)
        {
            StartCoroutine(Invulnerability());

            GameController.instance.playerHealth -= damageToPlayer;

            if (GameController.instance.playerHealth <= 0)
            {
                GameController.instance.playerHealth = 0;
                Destroy(this.gameObject);
                GameController.instance.eGameState = EGameState.GameOver;
            }
        }
    }

    public IEnumerator Invulnerability()
    {
        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        isInvulnerable = true;
        yield return new WaitForSeconds(GameController.instance.playerInvulnDuration);
        isInvulnerable = false;
        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
