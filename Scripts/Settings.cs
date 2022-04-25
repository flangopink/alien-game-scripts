using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public static int unlockedGames; // 0 - none, 1 - stars, 2 - rocket, 3 - houses

    public bool skipToAsteroids = true;
    public bool skipToHouses;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name + " say hello!");
        unlockedGames = PlayerPrefs.GetInt("UnlockedGames", 0);
        skipToAsteroids = PlayerPrefs.GetInt("SkipToAsteroids", 1) == 1;
        skipToHouses = PlayerPrefs.GetInt("SkipToHouses", 0) == 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(unlockedGames);
        Debug.Log("Pre-Load: " + PlayerPrefs.GetInt("UnlockedGames"));
        //Debug.Log("OnSceneLoaded: " + scene.name + ". Mode: " + mode);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Transform buttonPanel = GameObject.FindGameObjectWithTag("Canvas").transform.Find("MenuPanel");
            if (unlockedGames >= 1)
            {
                buttonPanel.Find("Button_StarGame").gameObject.SetActive(true);
            }
            if (unlockedGames >= 2)
            {
                buttonPanel.Find("Button_RocketGame").gameObject.SetActive(true);
            }
            if (unlockedGames == 3)
            {
                buttonPanel.Find("Button_HouseGame").gameObject.SetActive(true);
            }
        }
        Debug.Log("Mid-Load: " + PlayerPrefs.GetInt("UnlockedGames"));

        if (unlockedGames > 0)
        {
            PlayerPrefs.SetInt("UnlockedGames", unlockedGames);
            PlayerPrefs.Save();
        }


        if (SceneManager.GetActiveScene().name == "RocketGame" && unlockedGames == 0)
            unlockedGames++;
        if (SceneManager.GetActiveScene().name == "HouseGame" && unlockedGames == 1)
            unlockedGames++;
        if (SceneManager.GetActiveScene().name == "EndingRoom" && unlockedGames == 2)
            unlockedGames++;

        Debug.Log("Post-Load: " + PlayerPrefs.GetInt("UnlockedGames"));
    }

    public void DeleteSave()
    {
        unlockedGames = 0;
        PlayerPrefs.SetInt("UnlockedGames", 0);
        PlayerPrefs.Save();

        //Debug.Log(PlayerPrefs.GetInt("UnlockedGames"));
    }

    public void DisableButtons()
    {
        if (unlockedGames == 0)
        {
            Transform buttonPanel = GameObject.FindGameObjectWithTag("Canvas").transform.Find("MenuPanel");
            buttonPanel.Find("Button_StarGame").gameObject.SetActive(false);
            buttonPanel.Find("Button_RocketGame").gameObject.SetActive(false);
            buttonPanel.Find("Button_HouseGame").gameObject.SetActive(false);
        }
    }

    public void ToggleSkipToAsteroids()
    {
        skipToAsteroids = !skipToAsteroids;
        PlayerPrefs.SetInt("SkipToAsteroids", skipToAsteroids ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSkipToHouses()
    {
        skipToHouses = !skipToHouses;
        PlayerPrefs.SetInt("SkipToHouses", skipToHouses ? 1 : 0);
        PlayerPrefs.Save();
    }
}
