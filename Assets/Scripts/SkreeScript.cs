﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeScript : MonoBehaviour
{
    public float health = 4;
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
        health = originalHealth;
        GetComponent<Animator>().SetFloat("speedMultiplier", 1);
        vision.SetActive(true);
        speed = originalSpeed;
        if(sr) sr.color = Color.white;
    }

    public void SkreeAIFunction()
    {
        StartCoroutine(SkreeAI());
    }

    public IEnumerator SkreeAI()
    {
        canCollide = true;
        GetComponent<Animator>().SetFloat("speedMultiplier", 2);

        Vector2 initialPlayerPos = CharacterMovement.instance.transform.position;

        bool isFirstStage = true;

        while(!AIFinished)
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
            GameController.instance.playerHealth -= 20;
        }

        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            health -= 1;

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

    public IEnumerator OnDamaged()
    {

        sr.color = newColor;
        speed /= speedDivisorAfterDamaged;
        yield return new WaitForSeconds(timeSpeedReducedAfterDamaged);
        sr.color = Color.white;
        speed *= speedDivisorAfterDamaged;

        yield return null;
    }

    public IEnumerator OnDeath()
    {
        this.gameObject.SetActive(false);
        yield return null;
    }
}
