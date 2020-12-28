using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public Transform player;

    public Transform newCameraPosition;
    public float cameraTransitionSpeed;

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(GameController.instance.canCameraMove)
            StartCoroutine(MoveCameraCutscene());
        }


        if(GameController.instance.canCameraMove)
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

    public IEnumerator MoveCameraCutscene()
    {
        GameController.instance.canCameraMove = false;
        yield return new WaitForSeconds(1f);
        while(transform.position != newCameraPosition.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, newCameraPosition.position, cameraTransitionSpeed);
            yield return null;
        }


        //GameController.instance.canCameraMove = true;
        yield return null;
    }
}
