using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public bool canInteract = false;

    public Vector3 newCameraPosition;
    public Vector3 newPlayerPosition1;
    public Vector3 newPlayerPosition2;

    public float moveSpeed;

    private GameObject player;

    public bool flipPlayer;

    private void Start()
    {
        player = CharacterMovement.instance.gameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && canInteract && GameController.instance.eGameState == EGameState.GamePlay)
        {
            StartCoroutine(DoorCutscene());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    public IEnumerator DoorCutscene()
    {
        GameController.instance.eGameState = EGameState.Cutscene;
        player.GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;

        if(flipPlayer)
        {
            player.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            player.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        while (player.transform.position.x != newPlayerPosition1.x)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newPlayerPosition1, moveSpeed);
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



        while (player.transform.position.x != newPlayerPosition2.x)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newPlayerPosition2, moveSpeed);
            yield return null;
        }

        yield return null;

        player.GetComponent<BoxCollider2D>().isTrigger = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        GameController.instance.eGameState = EGameState.GamePlay;
    }
}
