using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeBullet : MonoBehaviour
{
    public int damageToPlayer = 8;
    public float speed = 1f;
    public float lifeSpan;

    public Rigidbody2D rb;
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
