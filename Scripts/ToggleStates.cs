using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStates : MonoBehaviour
{
    Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        switch (gameObject.name) 
        {
            case "ToggleAsteroids":
                toggle.isOn = PlayerPrefs.GetInt("SkipToAsteroids", 1) == 1;
                break;
            case "ToggleHouses":
                toggle.isOn = PlayerPrefs.GetInt("SkipToHouses", 0) == 1;
                break;
        }
    }
}
