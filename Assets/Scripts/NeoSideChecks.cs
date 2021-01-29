using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoSideChecks : MonoBehaviour
{

    public enum ESide
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public bool canGoRight = true;
    public bool canGoLeft = true;
    public bool canGoDown = true;
    public bool canGoUp = false;
    public ESide side;

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
        if (collision.CompareTag("Ground") && side == ESide.Bottom)
        {
            canGoUp = true;
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

        if (collision.CompareTag("Ground") && side == ESide.Bottom)
        {
            canGoUp = false;
        }
    }
}
