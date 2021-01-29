using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Config Parameters")]
    public float bulletSpeed;
    public float timeUntilDestruction;
    [HideInInspector] public bool isBulletGoingUp;
    [HideInInspector] public bool hasCollided = false;

    [Header("Object References")]
    public Rigidbody2D bulletRb;
    public AudioClip shootClip;
    private void Start()
    {
        SFXManager.instance.PlayShoot(shootClip);
        bulletRb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, timeUntilDestruction);
    }
    void Update()
    {   
        if(!isBulletGoingUp)
            bulletRb.velocity = transform.right * bulletSpeed;
    }


    private void OnDestroy()
    {
       if(hasCollided && !GameController.instance.isQuitting && GameController.instance.eGameState == EGameState.GamePlay)  Instantiate(PlayerShoot.instance.bulletDestruction, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
                hasCollided = true;
                Destroy(gameObject);
        }
    }

}
