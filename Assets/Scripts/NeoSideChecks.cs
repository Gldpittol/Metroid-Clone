using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoSideChecks : MonoBehaviour
{

    public enum ESide
    {
        Left,
        Right,
        Top
    }

    public ESide side;
    public bool canGoRight = true;
    public bool canGoLeft = true;
    public bool canGoDown = true;

    private void Awake()
    {
        canGoLeft = true;
        canGoRight = true;  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground") && side == ESide.Left)
        {
            canGoLeft = false;
        }
        if (collision.CompareTag("Ground") && side == ESide.Right)
        {
            canGoRight = false;
        }
        if (collision.CompareTag("Ground") && side == ESide.Top)
        {
            canGoDown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && side == ESide.Left)
        {
            canGoLeft = true;
        }
        if (collision.CompareTag("Ground") && side == ESide.Right)
        {
            canGoRight = true;
        }

        if (collision.CompareTag("Ground") && side == ESide.Top)
        {
            canGoDown = false;
        }
    }
}
