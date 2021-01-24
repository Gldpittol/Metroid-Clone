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

    public GameObject energyPrefab;
    public GameObject sammusDeathPrefab;

    public GameObject zoomerRight;
    public GameObject zoomerLeft;


    private void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        StartCoroutine(InitialCutscene());
        //StartCoroutine(InitialCutsceneDebug());
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;   
    }



    public IEnumerator InitialCutscene()
    {
        eGameState = EGameState.Cutscene;

        zoomerLeft.SetActive(false);
        zoomerRight.SetActive(false);

        yield return null;

        PlayerAnimations.instance.eAnimState = EAnimState.Starting;
        PlayerAnimations.instance.gameObject.GetComponent<Animator>().Play("Starting");


        while (!CharacterMovement.instance.canStart) yield return null;

        zoomerLeft.SetActive(true);
        zoomerRight.SetActive(true);

        PlayerAnimations.instance.eAnimState = EAnimState.Idle;
        eGameState = EGameState.GamePlay;

        yield return null;
    }

    public IEnumerator InitialCutsceneDebug()
    {
        PlayerAnimations.instance.eAnimState = EAnimState.Idle;
        yield return null;
    }

}
