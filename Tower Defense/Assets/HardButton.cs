using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HardButton : MonoBehaviour
{
    public void Select(int sceneID)
    {
        WaveSpawner.difficulty = 2;
        PlayerStats.Lives = 15;
        SceneManager.LoadScene("GameScene");
    }
}
