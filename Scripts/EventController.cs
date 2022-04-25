using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public int gameState;
    bool canSkip = false;
    public AudioClip[] clips;
    public bool finishedTalking;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        PlayAudio();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "EndingRoom")
            sr = transform.Find("fireworks").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canSkip)
        {
            PlayAudio();
            gameState++;
        }
        if (!GetComponent<AudioSource>().isPlaying) 
            finishedTalking = true;
        if (GetComponent<AudioSource>().isPlaying)
            finishedTalking = false;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "EndingRoom"
            && finishedTalking)
        {
            sr.gameObject.GetComponent<Animator>().enabled = true;
            StartCoroutine(SpriteFade(sr, 1, 3));
        }
    }

    public void PlayAudio()
    {
        if (gameState < clips.Length)
        {
            GetComponent<AudioSource>().PlayOneShot(clips[gameState]);
            gameState++;
        }
        else
        {
            canSkip = false;
            Debug.Log("GameState > AudioClips.");
        }
    }

    IEnumerator SpriteFade(SpriteRenderer sr, float endValue, float duration)
    {
        float elapsedTime = 0;
        float startValue = sr.color.a;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            yield return null;
        }
    }
}
