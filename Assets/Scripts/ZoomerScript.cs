using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerScript : MonoBehaviour
{
    [TextArea(3,9)]
    public string ZoomerTutorial = "Ângulos para usar em ''Rotations'', cada rotação para seu respectivo Target: \n0: Agarrado na parte de cima\n90:Agarrado na parede da esquerda\n180:Agarrado na parte de baixo\n270: Agarrado na parede da direita";
    public float speed = 10;
    private int i = 0;

    public GameObject[] Target;
    public float[] Rotations;
    public GameObject currentTarget;

    public bool isCyclic;
    private int direction;


    private void Start()
    {
        direction = 1;
        currentTarget = Target[i];
        GameObject.FindGameObjectWithTag("TargetZoomer").transform.SetParent(TargetGameObject.instance.gameObject.transform);
    }

    private void Update()
    {
        MoveToTarget();

        if(transform.position == Target[i].transform.position)
        {
            if(direction > 0) RotateEnemy();
            FindNewTarget();
            if(direction < 0) RotateEnemy();
        }
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target[i].transform.position, speed);
    }

    public void FindNewTarget()
    {
        print("oi");
        if(isCyclic)
        {
            i += 1;
            if (i >= Target.Length)
                i = 0;
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
}
