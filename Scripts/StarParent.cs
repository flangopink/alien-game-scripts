using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarParent : MonoBehaviour
{
    public List<Sprite> cards;
    public List<Collider2D> stars;
    public bool busy;
    bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCard;
        foreach(Transform child in transform)
        {
            stars.Add(child.GetComponent<Collider2D>());
            selectedCard = Random.Range(0, cards.Count);
            child.Find("card").GetComponent<SpriteRenderer>().sprite = cards[selectedCard];
            cards.Remove(cards[selectedCard]);
        }
    }

    void Update()
    {
        if (stars.Count == 0 && !triggered) 
        {
            triggered = true;
            GetComponent<EventController>().PlayAudio();
            StartCoroutine(LoadSceneTimer(10)); 
        }
    }

    IEnumerator LoadSceneTimer(float duration)
    {
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("RocketGame");
    }
}
