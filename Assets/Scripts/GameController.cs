using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    GamePlay,
    Cutscene,
    GameOver,
    Victory
}
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int playerHealth = 10;
    public float playerDamage;
    public float playerInvulnDuration;

    public EGameState eGameState;

    public GameObject enemyDeath;

    public Vector2 playerImpulseVector;

    [HideInInspector]public bool isQuitting;

    private void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;   
    }

}
