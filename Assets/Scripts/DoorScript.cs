using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Config Parameters")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float moveSpeed;
    public float delayBeforeEnteringDoor = 0.5f;
    public float doorCountdown = 4f;
    [HideInInspector] public int interactionState = 0;
    [HideInInspector] public bool canInteract = false;
    public Vector3 newCameraPosition;
    public Vector3 newPlayerPosition1;
    public Vector3 newPlayerPosition2;
    public bool flipPlayer;

    [Header("References")]
    public GameObject otherDoor;
    public GameObject[] enemiesToRespawn;
    private GameObject player;
    private AudioSource audSource;
    public AudioClip doorClip;

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
        player = CharacterMovement.instance.gameObject;     
    }

    private void Update()
    {
        doorCountdown -= Time.deltaTime;
        if(doorCountdown < 0 && interactionState == 9)
        {
            GetComponentInParent<Animator>().Play("DoorStill");
            audSource.PlayOneShot(doorClip);
            interactionState = 0;
        }

        if(Mathf.Abs(Camera.main.transform.position.x - transform.position.x) > 8)
        {
            GetComponentInParent<Animator>().Play("DoorStill");
            interactionState = 0;
        }
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Z) && canInteract && GameController.instance.eGameState == EGameState.GamePlay)
    //    {
    //        StartCoroutine(DoorCutscene());
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet") && interactionState == 0)
        {
            canInteract = true;
            doorCountdown = 4;
            GetComponentInParent<Animator>().Play("DoorOpen");
            audSource.PlayOneShot(doorClip);
            interactionState = 9;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }

        if (collision.CompareTag("Player") && interactionState == 9)
        {
            interactionState = 1;
            canInteract = false;
            StartCoroutine(DoorCutscene());
        }
    }



    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        canInteract = false;
    //    }
    //}

    public IEnumerator DoorCutscene()
    {

        GameController.instance.eGameState = EGameState.Cutscene;

        //while (!GroundCheck.instance.canJump) yield return null;

        for (int i = 0; i < enemiesToRespawn.Length; i++)
        {
            enemiesToRespawn[i].gameObject.SetActive(false);
            enemiesToRespawn[i].gameObject.SetActive(true);
        }

        Animator temp = player.GetComponent<Animator>();
        temp.speed = 0f;

        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(delayBeforeEnteringDoor);

        player.GetComponent<BoxCollider2D>().isTrigger = true;

        if(flipPlayer)
        {
            player.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            player.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        newPlayerPosition1 = new Vector2(newPlayerPosition1.x, player.transform.position.y);
        newPlayerPosition2 = new Vector2(newPlayerPosition2.x, player.transform.position.y);

        while (player.transform.position.x != newPlayerPosition1.x)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newPlayerPosition1, moveSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(CameraScript.instance.MoveCameraCutscene(newCameraPosition));

        while(!CameraScript.instance.routineFinished)
        {
            yield return null;
        }

        CameraScript.instance.maxX = maxX;
        CameraScript.instance.minX = minX;
        CameraScript.instance.maxY = maxY;
        CameraScript.instance.minY = minY;

        GetComponentInParent<Animator>().Play("DoorStill");

        otherDoor.GetComponentInParent<Animator>().Play("DoorOpen"); 
        audSource.PlayOneShot(doorClip);

        //yield return new WaitForSeconds(0.5f);

        while (player.transform.position.x != newPlayerPosition2.x)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newPlayerPosition2, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;

        player.GetComponent<BoxCollider2D>().isTrigger = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 1.3f;
        temp.speed = 1f;

        otherDoor.GetComponentInParent<Animator>().Play("DoorStill");
        GetComponentInParent<Animator>().Play("DoorStill");

        GameController.instance.eGameState = EGameState.GamePlay;
        interactionState = 0;
    }
}
