using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoScript : MonoBehaviour
{ 
    enum ENeoState
    {
        Downing,
        OnGround,
        GoingUp, 
        Idle
    }

    [SerializeField]private ENeoState eNeoState;
    private bool canMove;
    public float speedX, speedY;
    private Rigidbody2D rb;
    public bool isGoingRight = true;
    public float delayBeforeDowning;
    public bool isStarting = true;
    private float originalSpeedX, originalSpeedY;
    private Vector2 tempPlayerPos;
    private bool firstUp = false;

    public NeoSideChecks rightSide, leftSide, topSide, bottomSide;

    private float timeSinceLastReset = 0;

    public Color newColor;
    public float timeSpeedReducedAfterDamaged;
    public float speedDivisorAfterDamaged;
    private float originalHealth;
    private SpriteRenderer sr;
    public float health = 4;
    public float chanceToSpawnEnergy;
    public int damageToPlayer = 20;
    private Vector2 originalPosition;
    private bool beingDamaged = false;

    public AudioClip onEnemyHit;

    private void Awake()
    {
        originalPosition = transform.position;
        originalHealth = health;
        originalSpeedY = speedY;
        originalSpeedX = speedX;
    }

    private void OnEnable()
    {
        transform.position = originalPosition;
        health = originalHealth;
        GetComponent<Animator>().SetFloat("speedMultiplier", 1);
        speedX = originalSpeedX;
        speedY = originalSpeedY;
        isGoingRight = true;
        firstUp = false;
        isStarting = true;
        beingDamaged = false;
        if (sr) sr.color = Color.white;
        eNeoState = ENeoState.Downing;

    }

    private void OnDisable()
    {
        if (sr) sr.color = Color.white;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(GameController.instance.eGameState == EGameState.GamePlay && canMove)
        {
            if (!beingDamaged) UpdateSpeed();

            timeSinceLastReset += Time.deltaTime;

            if (!GroundCheck.instance.canJump && CharacterMovement.instance.transform.position.y > transform.position.y + 1 && !firstUp && timeSinceLastReset > 0.5f)
            {
                firstUp = true;
                tempPlayerPos = new Vector2(0, CharacterMovement.instance.transform.position.y + 1);
                eNeoState = ENeoState.GoingUp;
                GetComponent<Animator>().SetFloat("speedMultiplier", 2);
                if (isGoingRight && !rightSide.canGoRight) isGoingRight = false;
                if (!isGoingRight && !leftSide.canGoLeft) isGoingRight = true;
            }

            if (Mathf.Abs(CharacterMovement.instance.transform.position.y - transform.position.y) > 1 && eNeoState == ENeoState.OnGround && !firstUp)
            {
                firstUp = true;
                StartCoroutine(DelayBeforeUpping());
                if (isGoingRight && !rightSide.canGoRight) isGoingRight = false;
                if (!isGoingRight && !leftSide.canGoLeft) isGoingRight = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if(canMove && GameController.instance.eGameState == EGameState.GamePlay)
        {
            if(eNeoState == ENeoState.Downing)
            {
                tempPlayerPos = CharacterMovement.instance.transform.position;
                firstUp = false;

                if (isGoingRight && rightSide.canGoRight)
                    rb.velocity = new Vector2(speedX, -speedY);
                else if (isGoingRight && !rightSide.canGoRight)
                    rb.velocity = new Vector2(0, -speedY);
                else if (!isGoingRight && leftSide.canGoLeft)
                    rb.velocity = new Vector2(-speedX, -speedY);
                else if(!isGoingRight && !leftSide.canGoLeft)
                    rb.velocity = new Vector2(0, -speedY);
            }

            if (eNeoState == ENeoState.OnGround)
            {

                if (isGoingRight && rightSide.canGoRight)
                    rb.velocity = new Vector2(speedX, 0);
                else if (isGoingRight && !rightSide.canGoRight)
                    rb.velocity = new Vector2(0, 0);
                else if (!isGoingRight && leftSide.canGoLeft)
                    rb.velocity = new Vector2(-speedX, 0);
                else if (!isGoingRight && !leftSide.canGoLeft)
                    rb.velocity = new Vector2(0, 0);
            }
            if (eNeoState == ENeoState.GoingUp)
            {
                if (isGoingRight && rightSide.canGoRight)
                    rb.velocity = new Vector2(speedX, speedY);
                else if (isGoingRight && !rightSide.canGoRight)
                    rb.velocity = new Vector2(0, speedY);
                else if (!isGoingRight && leftSide.canGoLeft)
                    rb.velocity = new Vector2(-speedX, speedY);
                else if (!isGoingRight && !leftSide.canGoLeft)
                    rb.velocity = new Vector2(0, speedY);

            }
            if (eNeoState == ENeoState.Idle)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground") && bottomSide.canGoUp && eNeoState == ENeoState.Downing && !isStarting)
        {
            eNeoState = ENeoState.OnGround;
            GetComponent<Animator>().SetFloat("speedMultiplier", 1);

        }
        if (collision.CompareTag("Ground") && eNeoState == ENeoState.GoingUp)
        {
            StartCoroutine(DelayBeforeDowning());
        }

        if (collision.CompareTag("Player"))
        {
            PlayerEnemyCollision.instance.DamagePlayer(damageToPlayer, gameObject);
        }

        if (collision.CompareTag("PlayerBullet"))
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && eNeoState == ENeoState.Downing)
            isStarting = false;
    }

    private void OnBecameVisible()
    {
        canMove = true;
    }

    private void OnBecameInvisible()
    {
        canMove = false;
    }

    public void UpdateSpeed()
    {
        if (Mathf.Abs(tempPlayerPos.y - transform.position.y) < 1)
            speedY = originalSpeedY / 2;
        else if (Mathf.Abs(tempPlayerPos.y - transform.position.y) < 1.5)
            speedY = originalSpeedY / 1.66f;
        else if (Mathf.Abs(tempPlayerPos.y - transform.position.y) < 2)
            speedY = originalSpeedY / 1.33f;
        else
            speedY = originalSpeedY;
    }

    public IEnumerator DelayBeforeUpping()
    {
        yield return new WaitForSeconds(delayBeforeDowning / 3);
        eNeoState = ENeoState.GoingUp;
        GetComponent<Animator>().SetFloat("speedMultiplier", 2);
    }


    public IEnumerator DelayBeforeDowning()
    {
        while (!topSide.canGoDown)
        {
            yield return null;
        }

        eNeoState = ENeoState.Idle;

        GetComponent<Animator>().SetFloat("speedMultiplier", 1);

        yield return new WaitForSeconds(delayBeforeDowning);

        if (CharacterMovement.instance != null && CharacterMovement.instance.transform.position.x > transform.position.x) isGoingRight = true;
        else isGoingRight = false;

        if (isGoingRight && !rightSide.canGoRight) isGoingRight = false;
        if (!isGoingRight && !leftSide.canGoLeft) isGoingRight = true;

        //isStarting = true;

        eNeoState = ENeoState.Downing;
        GetComponent<Animator>().SetFloat("speedMultiplier", 2);
        timeSinceLastReset = 0;
    }


    public IEnumerator OnDamaged()
    {
        beingDamaged = true;
        SFXManager.instance.PlaySFX(onEnemyHit);
        sr.color = newColor;
        speedX /= speedDivisorAfterDamaged;
        speedY /= speedDivisorAfterDamaged;
        print(speedY);
        yield return new WaitForSeconds(timeSpeedReducedAfterDamaged);
        sr.color = Color.white;
        speedX *= speedDivisorAfterDamaged;
        speedY *= speedDivisorAfterDamaged;
        yield return null;
        beingDamaged = false;
    }

    public IEnumerator OnDeath()
    {
        SFXManager.instance.PlaySFX(onEnemyHit);
        Instantiate(GameController.instance.enemyDeath, transform.position, Quaternion.identity);
        if(Random.value < chanceToSpawnEnergy) Instantiate(GameController.instance.energyPrefab, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
