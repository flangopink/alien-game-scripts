using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string prevScene;
    public void Start()
    {
        prevScene = PlayerPrefs.GetString("SceneNumber");
        // if there will be a third scene, etc.
        PlayerPrefs.SetString("SceneNumber", SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Scene_MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Scene_StarGame()
    {
        SceneManager.LoadScene("StarGame");
    }
    public void Scene_RocketGame()
    {
        SceneManager.LoadScene("RocketGame");
    }
    public void Scene_HouseGame()
    {
        SceneManager.LoadScene("HouseGame");
    }
    public void Scene_Intro()
    {
        SceneManager.LoadScene("IntroScene");
    }
    public void Scene_Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
