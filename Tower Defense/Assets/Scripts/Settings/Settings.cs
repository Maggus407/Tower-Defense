using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject settingMenu;

    public void OpenSettings()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseSettings()
    {
        Time.timeScale = 1;
        settingMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
