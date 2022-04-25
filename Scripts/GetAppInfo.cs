using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAppInfo : MonoBehaviour
{
    public enum InfoType { version }
    public InfoType type;

    private void Start()
    {
        switch (type)
        {
            case InfoType.version:
                GetComponent<Text>().text = "v " + Application.version;
                break;
        }
    }
}
