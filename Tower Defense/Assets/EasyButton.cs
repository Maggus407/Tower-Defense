using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyButton : MonoBehaviour

{
    public void Select(int sceneID)
    {
        WaveSpawner.difficulty = 0;
        PlayerStats.Lives = 100;
        SceneManager.LoadScene(sceneID);
    }
}
