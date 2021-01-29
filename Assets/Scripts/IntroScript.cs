using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroScript : MonoBehaviour
{
    public bool isStartContinue;

    public GameObject startContinue;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && !isStartContinue)
        {
            StartContinue();
        }

        if (Input.GetKeyDown(KeyCode.Return) && isStartContinue)
        {
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
    }

    public void StartContinue()
    {
        startContinue.SetActive(true);
        gameObject.SetActive(false);
    }
}
