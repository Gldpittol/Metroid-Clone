using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroScript : MonoBehaviour
{
    public GameObject startContinue;
    public bool isStartContinue;

    // Update is called once per frame
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
