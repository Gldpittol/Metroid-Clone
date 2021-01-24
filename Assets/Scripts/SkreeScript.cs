using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeScript : MonoBehaviour
{
    public float health = 4;
    public float chanceToSpawnEnergy;
    public int damageToPlayer = 20;
    public float speed;
    private Transform playerLocation;
    public bool AIFinished = false;
    public GameObject vision;
    private Vector2 originalPosition;
    private bool canCollide = false;


    public float firstFallMinY = 2f;
    public float distanceToSecondPhase = 1.5f;
    public float amountBelowPlayer = 5f;
    public Color newColor;
    public float timeSpeedReducedAfterDamaged;
    public float speedDivisorAfterDamaged;
    private float originalHealth;
    private float originalSpeed;
    private SpriteRenderer sr;
    private bool beingDamaged = false;

    public GameObject skreeDeathSpawn;

    public AudioClip onEnemyHit;
    private void Awake()
    {
        originalPosition = transform.position;
        originalHealth = health;
        originalSpeed = speed;
    }

    private void Start()
    {
        playerLocation = CharacterMovement.instance.gameObject.transform;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        transform.position = originalPosition;
        AIFinished = false;
        canCollide = false;
        beingDamaged = false;
        health = originalHealth;
        GetComponent<Animator>().SetFloat("speedMultiplier", 1);
        vision.SetActive(true);
        speed = originalSpeed;
        if(sr) sr.color = Color.white;
    }

    private void OnDisable()
    {
        if (sr) sr.color = Color.white;
        if (health > 0 && !GameController.instance.isQuitting && GameController.instance.eGameState == EGameState.GamePlay) Instantiate(skreeDeathSpawn, transform.position, Quaternion.identity);
    }

    public void SkreeAIFunction(GameObject vision)
    {
        vision.SetActive(false);
        StartCoroutine(SkreeAI());
    }

    public IEnumerator SkreeAI()
    {
        GetComponent<Animator>().SetFloat("speedMultiplier", 2);

        Vector2 initialPlayerPos = CharacterMovement.instance.transform.position;

        bool isFirstStage = true;

        yield return null;

        canCollide = true;

        while (!AIFinished)
        {
            if(CharacterMovement.instance != null)
            {
                if (transform.position.y > playerLocation.transform.position.y)
                {
                    while (Mathf.Abs(transform.position.x - initialPlayerPos.x) > distanceToSecondPhase && isFirstStage)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(initialPlayerPos.x, transform.position.y - firstFallMinY), speed);
                        yield return null;
                    }

                    isFirstStage = false;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerLocation.position.x, playerLocation.position.y - amountBelowPlayer), speed);
                    yield return null;
                }
            }

            if (CharacterMovement.instance == null || transform.position.y <= playerLocation.transform.position.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - 1000), speed);
                yield return null;
            }

        }
        yield return new WaitForSeconds(1f);

        GetComponent<Animator>().SetFloat("speedMultiplier", 1);
        vision.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Ground"))
        {
            AIFinished = true;
        }

        if (collision.CompareTag("Player") && canCollide)
        {
            PlayerEnemyCollision.instance.DamagePlayer(damageToPlayer, gameObject);
        }

        if (collision.CompareTag("PlayerBullet") && canCollide)
        {
            collision.GetComponent<BulletScript>().hasCollided = true;
            Destroy(collision.gameObject);
            if (!beingDamaged)
            {
                health -= GameController.instance.playerDamage;

                if (health <= 0)
                {
                    StartCoroutine(OnDeath());
                }
                else
                {
                    StartCoroutine(OnDamaged());
                }
            }
        }
    }

    public IEnumerator OnDamaged()
    {
        beingDamaged = true;
        SFXManager.instance.PlaySFX(onEnemyHit);
        sr.color = newColor;
        speed /= speedDivisorAfterDamaged;
        yield return new WaitForSeconds(timeSpeedReducedAfterDamaged);
        sr.color = Color.white;
        speed *= speedDivisorAfterDamaged;
        yield return null;
        beingDamaged = false;
    }

    public IEnumerator OnDeath()
    {
        SFXManager.instance.PlaySFX(onEnemyHit);
        Instantiate(GameController.instance.enemyDeath, transform.position, Quaternion.identity);
        if (Random.value < chanceToSpawnEnergy) Instantiate(GameController.instance.energyPrefab, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
