using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    public static PlayerEnemyCollision instance;

    [HideInInspector] public bool canMoveHorizontally = true;

    private bool isInvulnerable;

    public SpriteRenderer sr;

    private Coroutine flashRoutine;
    private void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
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
                GameController.instance.GameOverFunction();
            }
        }
    }

    public IEnumerator Invulnerability(GameObject attacker)
    {
        isInvulnerable = true;

        SFXManager.instance.PlaySFX(GameController.instance.playerHitClip);

        if(attacker.transform.position.x < transform.position.x)
        {
            Rigidbody2D rb = CharacterMovement.instance.gameObject.GetComponent<Rigidbody2D>();

            Vector2 tempImpulseVector = new Vector2(GameController.instance.playerImpulseVector.x, GameController.instance.playerImpulseVector.y);
            if (!GroundCheck.instance.canJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                tempImpulseVector = new Vector2(GameController.instance.playerImpulseVector.x * 1f, 0);
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
                tempImpulseVector = new Vector2(-GameController.instance.playerImpulseVector.x * 1f , 0f);
            }

            rb.AddForce(tempImpulseVector, ForceMode2D.Impulse);
            canMoveHorizontally = false;
        }


        flashRoutine = StartCoroutine(FlashRoutine());
        //CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(GameController.instance.playerInvulnDuration);
        //CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        StopCoroutine(flashRoutine);
        sr.color = new Color(1, 1, 1, 1);

        isInvulnerable = false;
        canMoveHorizontally = true;
    }

    public IEnumerator FlashRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.05f);
        sr.color = new Color(1, 1, 1, 1);
        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
