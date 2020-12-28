using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool canCameraMove = true;

    public int playerHealth = 10;

    private void Awake()
    {
        instance = this;
        canCameraMove = true;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
}
