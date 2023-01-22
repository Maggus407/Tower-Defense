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
        roundsText.text = PlayerStats.Rounds.ToString() + (" /10");
    }
}
