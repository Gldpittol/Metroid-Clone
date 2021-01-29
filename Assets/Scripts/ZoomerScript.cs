using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EZoomerType
{
    Zoomer,
    Ripper,
    RedRipper
}
public class ZoomerScript : MonoBehaviour
{
    [Header("Parameters")]
    public int damageToPlayer = 3;
    private int i = 0;
    private int direction;
    public float speed = 10;
    public float chanceToSpawn; //entre 0 e 1
    public float chanceInvertSideAfterSpawn; //entre 0 e 1
    public float health;
    public float chanceToSpawnEnergy;
    public float timeSpeedReducedAfterDamaged;
    public float speedDivisorAfterDamaged;
    public float[] Rotations;
    private float initialPosX = 0;
    private float initialPosY = 0;
    private float originalSpeed;
    private float originalHealth;
    public bool isCyclic;
    private bool beingDamaged = false;
    public EZoomerType eZoomerType;

    [Header("References")]
    public Color newColor;
    private Color originalColor;
    private Quaternion originalRotation;
    public GameObject Targets;
    public GameObject[] Target;
    public GameObject currentTarget;
    public AudioClip onEnemyHit;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (initialPosX == 0) initialPosX = transform.position.x;
        if (initialPosY == 0) initialPosY = transform.position.y;
        originalSpeed = speed;
        speed = 0;
        originalRotation = transform.rotation;
        originalHealth = health;
        originalColor = GetComponent<SpriteRenderer>().color;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if(Random.value > chanceToSpawn)
        {
            this.gameObject.SetActive(false);
        }

        health = originalHealth;

        transform.position = new Vector2(initialPosX, initialPosY);
        transform.rotation = originalRotation;

        if (Random.value > chanceInvertSideAfterSpawn && isCyclic)
        {
            i = Target.Length - 1;
            direction = -1;

        }
        else
        {
            direction = 1;
            i = 0;
        }

        beingDamaged = false;

        if (eZoomerType == EZoomerType.Ripper || eZoomerType == EZoomerType.RedRipper) sr.flipX = false;
    }
    private void Start()
    {
        currentTarget = Target[i];
        Targets.transform.SetParent(TargetGameObject.instance.gameObject.transform);
    }
    private void Update()
    {
        if(GameController.instance.eGameState == EGameState.GamePlay)
        {
            MoveToTarget();

            if (transform.position == Target[i].transform.position)
            {
                if (eZoomerType == EZoomerType.Zoomer)
                {
                    if (direction > 0) RotateEnemy();
                    FindNewTarget();
                    if (direction < 0) RotateEnemy();
                }
                else
                {
                    if (sr.flipX) sr.flipX = false;
                    else sr.flipX = true;
                    FindNewTarget();
                }

            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            collision.GetComponent<BulletScript>().hasCollided = true;
            Destroy(collision.gameObject);

            if (!beingDamaged && eZoomerType != EZoomerType.Ripper)
            {
                if(eZoomerType != EZoomerType.RedRipper) health -= GameController.instance.playerDamage;

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

        if (collision.CompareTag("Player"))
        {
            PlayerEnemyCollision.instance.DamagePlayer(damageToPlayer, gameObject);
        }
    }

    private void OnBecameVisible()
    {
        speed = originalSpeed;
    }

    private void OnBecameInvisible()
    {
        speed = 0;
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target[i].transform.position, speed*Time.deltaTime);
    }

    public void FindNewTarget()
    {

        if(isCyclic)
        {
            i += direction;
            if (i >= Target.Length && direction > 0)
                i = 0;
            if (i < 0 && direction < 0)
                i = Target.Length - 1;
            currentTarget = Target[i];
        }

        if(!isCyclic)
        {
            i += direction;
            if (i >= Target.Length || i < 0)
            {
                direction *= -1;
                i += direction * 2;
                currentTarget = Target[i];
            }
            else
            {
                currentTarget = Target[i];
            }
        }
    }

    public void RotateEnemy()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Rotations[i]);
    }

    public IEnumerator OnDamaged()
    {
        beingDamaged = true;

        SFXManager.instance.PlaySFX(onEnemyHit);

        sr.color = newColor;
        speed /= speedDivisorAfterDamaged;
        yield return new WaitForSeconds(timeSpeedReducedAfterDamaged);
        sr.color = originalColor;
        speed *= speedDivisorAfterDamaged;

        yield return null;
        beingDamaged = false;
    }

    public IEnumerator OnDeath()
    {
        Instantiate(GameController.instance.enemyDeath, transform.position, Quaternion.identity);
        SFXManager.instance.PlaySFX(onEnemyHit);
        if (Random.value < chanceToSpawnEnergy) Instantiate(GameController.instance.energyPrefab, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
