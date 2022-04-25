using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienParent : MonoBehaviour
{
    public List<SpriteRenderer> houses;
    public List<Sprite> cards;
    public List<GameObject> ayys;
    public List<Transform> remainingAyys_p1;
    public List<Transform> remainingAyys_p2;

    public Transform gameManager;

    public bool phase1_done;
    bool triggeronce;
    bool triggeronce2;
    bool triggeronce3;
    public bool busy;

    int debugInt = 0;

    public bool finallyItFuckingEnded;

    readonly List<string> tasks = new List<string>() { "count", "laskovo", "parts", "kakoy" };
    readonly List<float> spawnX = new List<float>() { -5.5f, -3, -0.5f, 2, 4.5f };
    readonly List<float> spawnY = new List<float>() { 3.5f, 1 };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            int ayy = Random.Range(0, ayys.Count - 1);
            Instantiate(ayys[ayy], new Vector2(-10, 2), transform.rotation, transform);
            ayys.Remove(ayys[ayy]);
        }

        int selectedCard;
        foreach(Transform child in transform)
        {
            remainingAyys_p1.Add(child);
            remainingAyys_p2.Add(child);
            selectedCard = Random.Range(0, cards.Count);
            child.Find("card").GetComponent<SpriteRenderer>().sprite = cards[selectedCard];
            cards.Remove(cards[selectedCard]);
            child.GetComponent<Alien>().task = tasks[Random.Range(0, 4)];
        }

        StartCoroutine(WaitThenPlayAudio());

        if (GameObject.FindGameObjectWithTag("Settings") != null)
            if (gameManager.GetComponent<SceneChanger>().prevScene == "MainMenu" && GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().skipToHouses)
            {
                GetComponent<AudioSource>().Stop();
                StopAllCoroutines();
                StartCoroutine(FuckYou());
                remainingAyys_p2[11].localScale *= 2;
            }
    }

    void Update()
    {
        if (phase1_done && !triggeronce)
        {
            triggeronce = true;
            for (int i = 0; i < remainingAyys_p2.Count; i++)
            {
                if (i < 5)
                    remainingAyys_p2[i].position = new Vector2(spawnX[i], spawnY[0]);
                else if (i == 10)
                    remainingAyys_p2[i].position = new Vector2(3.5f, -3.75f);
                else if (i == 11)
                {
                    remainingAyys_p2[i].position = new Vector2(0.75f, -3.75f);
                    remainingAyys_p2[i].localScale /= 2;
                }
                else
                    remainingAyys_p2[i].position = new Vector2(spawnX[i - 5], spawnY[1]);
                remainingAyys_p2[i].GetComponent<Alien>().posOffset = remainingAyys_p2[i].position;
                remainingAyys_p2[i].GetComponent<Alien>().canFloat = true;
            }
        }

        if (!triggeronce2 && remainingAyys_p1.Count == 0)
        {
            phase1_done = true;
            triggeronce2 = true;
            GetComponent<AudioSource>().Stop();
            GetComponent<EventController>().PlayAudio();
            StartCoroutine(WaitThenPlayAudio());
        }

        if (!triggeronce3 && remainingAyys_p2.Count == 0) 
        {
            triggeronce3 = true;
            GetComponent<AudioSource>().Stop();
            GetComponent<EventController>().PlayAudio();
            StartCoroutine(FinalTimer(5));
        }

        if (houses.Count != 0)
        {
            for (int i = 0; i < houses.Count; i++)
            {
                string spritename = houses[i].sprite.name;

                if (spritename.IndexOf('4') == spritename.Length - 1)
                    houses.Remove(houses[i]);
            }
        }

        // holy fuck this is SO difficult to debug
        if (Input.GetKeyDown(KeyCode.F10))
        {
            StopAllCoroutines();
            if (debugInt == 0)
            {
                SkipToHouses();
            }
            if(debugInt == 1)
            {
                GetComponent<EventController>().gameState = 4;
                GetComponent<AudioSource>().Stop();
                remainingAyys_p2.Clear();
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
            }
            debugInt++;
        }
    }

    IEnumerator FinalTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        finallyItFuckingEnded = true;
    }

    // THIS SHIT DRIVES ME INSANE
    public void SkipToHouses()
    {
        StopAllCoroutines();
        GetComponent<EventController>().gameState = 2;
        GetComponent<AudioSource>().Stop();
        remainingAyys_p1.Clear();
        foreach (SpriteRenderer sr in houses)
        {
            sr.gameObject.GetComponent<House>().NextSprite();
            sr.gameObject.GetComponent<House>().NextSprite();
            sr.gameObject.GetComponent<House>().NextSprite();
            sr.gameObject.GetComponent<House>().NextSprite();
        }
        remainingAyys_p2[0].GetComponent<Alien>().StopAllCoroutines();
        remainingAyys_p2[0].position = new Vector2(spawnX[0], spawnY[0]);
        StopAllCoroutines();
        GetComponent<AudioSource>().Stop();
        GetComponent<EventController>().PlayAudio();
        StartCoroutine(WaitThenSTOPALLTHEFUCKINGCOROUTINES());

        // SHUT THE FUCK UP PLEASE AND GET TO YOUR SPOT IMMEDIATELY

    }
    IEnumerator WaitThenPlayAudio()
    {
        yield return new WaitForSeconds(17);
        GetComponent<EventController>().PlayAudio();
    }

    IEnumerator WaitThenSTOPALLTHEFUCKINGCOROUTINES()
    {
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
    }

    IEnumerator FuckYou()
    {
        yield return new WaitForSeconds(0.001f);
        SkipToHouses();
    }
}
