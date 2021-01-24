using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergyText : MonoBehaviour
{
    private Text energyText;
    private string colorTag1 = "<color=#5fabffff>";
    private string colorTag2 ="<color=orange>";
    private string endTag = "</color>";
    private void Awake()
    {
        energyText = GetComponent<Text>();
    }
    void Update()
    {
        if(GameController.instance.playerHealth < 10)
        {
            energyText.text = colorTag1 + "EN" + endTag + colorTag2 + " " + endTag + "0" + GameController.instance.playerHealth.ToString();
        }
        else
        {
            energyText.text = colorTag1 + "EN" + endTag + colorTag2 + " " + endTag + GameController.instance.playerHealth.ToString();
        }
    }
}
