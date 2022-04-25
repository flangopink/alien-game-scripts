using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessSettingsSingleton : MonoBehaviour
{
    public void DeleteSave()
    {
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().DeleteSave();
    }
    public void DisableButtons()
    {
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().DisableButtons();
    }
    public void ToggleSkipToAsteroids()
    {
        Toggle clickedToggle = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        if (clickedToggle == null)
            return;
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().ToggleSkipToAsteroids();
    }
    public void ToggleSkipToHouses()
    {
        Toggle clickedToggle = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        if (clickedToggle == null)
            return;
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().ToggleSkipToHouses();
    }
}
