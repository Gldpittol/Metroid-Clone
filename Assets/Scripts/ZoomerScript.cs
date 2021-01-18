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
    public EZoomerType eZoomerType;

    //[TextArea(3,9)]
    //public string ZoomerTutorial = "Ângulos para usar em ''Rotations'', cada rotação para seu respectivo Target: \n0: Agarrado na parte de cima\n90:Agarrado na parede da esquerda\n180:Agarrado na parte de baixo\n270: Agarrado na parede da direita";
    public float speed = 10;
    private int i = 0;

    public GameObject Targets;
    public GameObject[] Target;
    public float[] Rotations;
    public GameObject currentTarget;

    public bool isCyclic;
    private int direction;

    private float initialPosX = 0;
    private float initialPosY = 0;
    private float originalSpeed;
    private float originalHealth;
    private Quaternion originalRotation;

    public float chanceToSpawn; //entre 0 e 1
    public float chanceInvertSideAfterSpawn; //entre 0 e 1
    public float health;
    public int damageToPlayer = 3;
    public Color newColor;
    private Color originalColor;
    public float timeSpeedReducedAfterDamaged;
    public float speedDivisorAfterDamaged;

    private bool beingDamaged = false;
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
    }
    private void Start()
    {
        currentTarget = Target[i];
        Targets.transform.SetParent(TargetGameObject.instance.gameObject.transform);
    }
    private void Update()
    {
        MoveToTarget();

        if(transform.position == Target[i].transform.position)
        {
            if(eZoomerType == EZoomerType.Zoomer)
            {
                if (direction > 0) RotateEnemy();
                FindNewTarget();
                if (direction < 0) RotateEnemy();
            }
            else
            {
                if(sr.flipX) sr.flipX = false;
                else sr.flipX = true;
                FindNewTarget();
            }

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
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
            PlayerEnemyCollision.instance.DamagePlayer(damageToPlayer);
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
        transform.position = Vector2.MoveTowards(transform.position, Target[i].transform.position, speed);
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
        this.gameObject.SetActive(false);
        yield return null;
    }
}
