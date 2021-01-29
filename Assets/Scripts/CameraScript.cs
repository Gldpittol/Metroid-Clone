using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;

    [Header("Config Parameters")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float cameraTransitionSpeed;
    [HideInInspector] public bool routineFinished;

    [Header("Object References")]
    public Transform player;
    public Transform newCameraPosition;


    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    //if(GameController.instance.canCameraMove)
        //    //StartCoroutine(MoveCameraCutscene());
        //}


        if(player && GameController.instance.eGameState == EGameState.GamePlay)
        {
            float tempX;
            float tempY;

            tempX = player.position.x;
            tempY = player.position.y;

            if (player.position.x >= maxX) tempX = maxX;
            if (player.position.x <= minX) tempX = minX;
            if (player.position.y >= maxY) tempY = maxY;
            if (player.position.y <= minY) tempY = minY;

            Vector3 temp = new Vector3(tempX, tempY, -10);
            transform.position = temp;
        }
    }

    public IEnumerator MoveCameraCutscene(Vector3 newPosition)
    {
        routineFinished = false;

        while(transform.position != newPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, cameraTransitionSpeed * Time.deltaTime);
            yield return null;
        }

        routineFinished = true;
        yield return null;
    }
}
