﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeBullet : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb;
    public float lifeSpan;
    public int damageToPlayer = 8;
    private void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerEnemyCollision.instance.DamagePlayer(damageToPlayer, gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

}
