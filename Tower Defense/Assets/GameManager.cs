using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded;

    public GameObject gameOverUI;
    public GameObject gameWonUI;

    private void Start()
    {
        gameEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
        {
            return;
        }

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
            this.enabled = false;
        }

        if(WaveSpawner.over)
        {
            WinLevel();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        gameEnded = true;
        gameWonUI.SetActive(true);
        Debug.Log("Level Completed");
    }
}
