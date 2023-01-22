using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediumButton : MonoBehaviour
{
    public void Select(int sceneID)
    {
        WaveSpawner.difficulty = 1;
        PlayerStats.Lives = 50;
        SceneManager.LoadScene(sceneID);
    }
}
