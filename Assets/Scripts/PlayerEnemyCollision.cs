using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    public static PlayerEnemyCollision instance;
    private bool isInvulnerable;
    public bool canMoveHorizontally = true;

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

    public void DamagePlayer(int damageToPlayer, GameObject attacker)
    {
        if(!isInvulnerable)
        {
            StartCoroutine(Invulnerability(attacker));

            GameController.instance.playerHealth -= damageToPlayer;

            if (GameController.instance.playerHealth <= 0)
            {
                GameController.instance.playerHealth = 0;
                Instantiate(GameController.instance.sammusDeathPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameController.instance.eGameState = EGameState.GameOver;
            }
        }
    }

    public IEnumerator Invulnerability(GameObject attacker)
    {
        isInvulnerable = true;

        if(attacker.transform.position.x < transform.position.x)
        {
            Rigidbody2D rb = CharacterMovement.instance.gameObject.GetComponent<Rigidbody2D>();

            Vector2 tempImpulseVector = new Vector2(GameController.instance.playerImpulseVector.x, GameController.instance.playerImpulseVector.y);
            if (!GroundCheck.instance.canJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                tempImpulseVector = new Vector2(GameController.instance.playerImpulseVector.x * 2, 0);
            }

            rb.AddForce(tempImpulseVector, ForceMode2D.Impulse);

            canMoveHorizontally = false;
        }

        else
        {
            Rigidbody2D rb = CharacterMovement.instance.gameObject.GetComponent<Rigidbody2D>();
            Vector2 tempImpulseVector = new Vector2(-GameController.instance.playerImpulseVector.x, GameController.instance.playerImpulseVector.y);
            if (!GroundCheck.instance.canJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                tempImpulseVector = new Vector2(-GameController.instance.playerImpulseVector.x * 1.5f , 0.5f);
            }

            rb.AddForce(tempImpulseVector, ForceMode2D.Impulse);
            canMoveHorizontally = false;
        }

        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(GameController.instance.playerInvulnDuration);
        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        isInvulnerable = false;
        canMoveHorizontally = true;

    }
}
