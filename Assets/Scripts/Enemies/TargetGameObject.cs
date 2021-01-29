using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGameObject : MonoBehaviour
{
    public static TargetGameObject instance;
    private void Awake()
    {
        instance = this;
    }
}
