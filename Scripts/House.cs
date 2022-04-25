using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    int currentSprite;
    public Sprite[] sprites;

    private void OnMouseOver()
    {
        if (GameObject.FindGameObjectWithTag("AlienParent").GetComponent<AlienParent>().finallyItFuckingEnded == true 
            && Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndingRoom");
        }
    }

    public void NextSprite()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
        currentSprite++;
    }
}
