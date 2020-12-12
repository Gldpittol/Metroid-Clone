using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerScript : MonoBehaviour
{
    public float speed = 10;
    private int i = 0;

    public GameObject[] Target;
    public GameObject currentTarget;

    private void Awake()
    {
        currentTarget = Target[i];
        GameObject.FindGameObjectWithTag("TargetZoomer").transform.SetParent(TargetGameObject.instance.gameObject.transform);
    }

    private void Update()
    {
        MoveToTarget();

        if(transform.position == Target[i].transform.position)
        {
            FindNewTarget();
            RotateEnemy();
        }
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target[i].transform.position, speed);
    }

    public void FindNewTarget()
    {
        i++;
        if (i >= Target.Length)
            i = 0;
        currentTarget = Target[i];
    }

    public void RotateEnemy()
    {
        if(Target[i].transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        }
        else if(Target[i].transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        if (Target[i].transform.position.y > transform.position.y)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (Target[i].transform.position.y < transform.position.y)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
}
