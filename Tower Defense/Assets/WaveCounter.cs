using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    public Text roundsText;
    // Update is called once per frame
    void Update()
    {
        switch(WaveSpawner.difficulty)
        {
            case 0:
                roundsText.text = PlayerStats.Rounds.ToString() + (" /10");
                break;
            case 1:
                roundsText.text = PlayerStats.Rounds.ToString() + (" /20");
                break;
            case 2:
                roundsText.text = PlayerStats.Rounds.ToString() + (" /30");
                break;
        }
        
    }
}
