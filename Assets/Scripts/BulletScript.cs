using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D bulletRb;
    public bool isBulletGoingUp;
    public float timeUntilDestruction;

    private void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, timeUntilDestruction);
    }
    void Update()
    {   
        if(!isBulletGoingUp)
            bulletRb.velocity = transform.right * bulletSpeed;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }

}
