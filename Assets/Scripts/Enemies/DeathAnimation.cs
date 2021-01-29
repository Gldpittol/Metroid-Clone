using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    
   public void OnAnimationEnd()
    {
        Destroy(this.gameObject);
    }

}
