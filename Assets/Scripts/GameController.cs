using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject gameOverImage;

    public AudioClip startOSTClip;
    public AudioClip brinstarClip;
    public AudioClip playerDeathClip;
    public AudioClip playerHitClip;

    public float delayBeforeGameOver = 2f;

    private bool audioSourceOn = false;
    private void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        //StartCoroutine(InitialCutscene());
        StartCoroutine(InitialCutsceneDebug());
    }

    private void Update()
    {
        if(playerHealth < 20 && !audioSourceOn)
        {
            GetComponent<AudioSource>().enabled = true;
                audioSourceOn = true;
}
        else if (playerHealth >= 20 && audioSourceOn)
        {
            GetComponent<AudioSource>().enabled = false;
            audioSourceOn = false;
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;   
    }

    public IEnumerator InitialCutscene()
    {
        eGameState = EGameState.Cutscene;
        Camera.main.GetComponent<AudioManager>().PlayAudio(startOSTClip);

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

        Camera.main.GetComponent<AudioManager>().PlayAudio(brinstarClip);
        Camera.main.GetComponent<AudioManager>().audSource.loop = true;

        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield return null;
    }

    public IEnumerator InitialCutsceneDebug()
    {
        yield return null;
        PlayerAnimations.instance.eAnimState = EAnimState.Idle;
        CharacterMovement.instance.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return null;
    }

    public void GameOverFunction()
    {
        StartCoroutine(GameOverRoutine());
    }

    public IEnumerator GameOverRoutine()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
        SFXManager.instance.PlaySFX(playerDeathClip);

        yield return new WaitForSeconds(delayBeforeGameOver);
        gameOverImage.SetActive(true);
        yield return new WaitForSeconds(delayBeforeGameOver);
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

}
