using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUi : MonoBehaviour
{
    public Text livesText;

    private void Update()
    {
        if (PlayerStats.Lives < 0) { return; } 
        livesText.text = PlayerStats.Lives.ToString();
        if(PlayerStats.Lives <= 3)
        {
            livesText.color = Color.red;
        }
        
    }
}
