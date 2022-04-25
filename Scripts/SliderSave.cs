using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSave : MonoBehaviour
{
    Slider slider;
    public enum SliderType { Speed, Delay, Timer };
    public SliderType type;

    private void Start()
    {
        slider = GetComponent<Slider>();

        switch (type) 
        {
            case SliderType.Speed:
                slider.value = PlayerPrefs.GetFloat("AsteroidSpeed", 0.02f) * 1000;
                UpdateSpeedText();
                break;

            case SliderType.Delay:
                slider.value = PlayerPrefs.GetFloat("AsteroidDelay", 2f) * 10;
                UpdateDelayText();
                break;

            case SliderType.Timer:
                slider.value = PlayerPrefs.GetInt("AsteroidTime", 27);
                UpdateTimeText();
                break;
        }
    }

    public void UpdateSpeedText()
    {
        transform.Find("Value").GetComponent<Text>().text = (slider.value / 1000).ToString();
    }
    public void UpdateDelayText()
    {
        transform.Find("Value").GetComponent<Text>().text = (slider.value / 10).ToString();
    }
    public void UpdateTimeText()
    {
        transform.Find("Value").GetComponent<Text>().text = (slider.value+8).ToString();
    }

    public void SaveSpeedValue()
    {
        PlayerPrefs.SetFloat("AsteroidSpeed", slider.value / 1000);
        PlayerPrefs.Save();
    }
    public void SaveDelayValue()
    {
        PlayerPrefs.SetFloat("AsteroidDelay", slider.value / 10);
        PlayerPrefs.Save();
    }
    public void SaveTimeValue()
    {
        PlayerPrefs.SetInt("AsteroidTime", (int)slider.value);
        PlayerPrefs.Save();
    }
}
