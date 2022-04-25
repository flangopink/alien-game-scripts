using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneScript : MonoBehaviour
{
    SpriteRenderer bg;
    Transform alien;
    Transform house;

    float timer;
    bool triggered;
    [SerializeField] Vector3 m_from = new Vector3(0.0F, 0.0F, 10.0F);
    [SerializeField] Vector3 m_to = new Vector3(0.0F, 0.0F, -10.0F);
    [SerializeField] float m_frequency = 0.5F;

    void Start()
    {
        bg = transform.Find("bg_forest").GetComponent<SpriteRenderer>();
        alien = transform.Find("ayy");
        house = transform.Find("house2");
    }

    void Update()
    {
        Quaternion from = Quaternion.Euler(m_from);
        Quaternion to = Quaternion.Euler(m_to);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * m_frequency));
        alien.localRotation = Quaternion.Lerp(from, to, lerp);

        timer += Time.deltaTime;
        if (timer >= 7 && !triggered)
        {
            triggered = true;
            StartCoroutine("Cutscene");
        }
    }

    IEnumerator LerpPosition(Transform target, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = target.position;
        while (time < duration)
        {
            target.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        target.position = targetPosition;
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1);

        StartCoroutine(SpriteFade(bg, 1, 3));
        StartCoroutine(SpriteFade(alien.GetComponent<SpriteRenderer>(), 1, 3));
        StartCoroutine(SpriteFade(house.GetComponent<SpriteRenderer>(), 1, 3));
        yield return new WaitForSeconds(3);

        StartCoroutine(LerpPosition(alien, new Vector2(0, -1), 3));
        yield return new WaitForSeconds(3);
        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -2.35f), 2));
        yield return new WaitForSeconds(2);

        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -1), 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -2.35f), 0.5f));
        yield return new WaitForSeconds(0.5f);

        house.localScale = new Vector3(house.localScale.x, 0.43f, house.localScale.z);
        house.position = new Vector3(house.position.x, -2.7f, house.position.z);

        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -1), 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -2.35f), 0.5f));
        yield return new WaitForSeconds(0.5f);

        house.localScale = new Vector3(house.localScale.x, 0.3f, house.localScale.z);
        house.position = new Vector3(house.position.x, -3.1f, house.position.z);

        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -1), 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LerpPosition(alien, new Vector2(-3.35f, -2.35f), 0.5f));
        yield return new WaitForSeconds(0.5f);

        house.localScale = new Vector3(house.localScale.x, 0.1f, house.localScale.z);
        house.position = new Vector3(house.position.x, -3.725f, house.position.z);

        yield return new WaitForSeconds(2);
        StartCoroutine(LerpPosition(alien, new Vector2(-9f, -5f), 3));
        StartCoroutine(SpriteFade(bg, 0, 3));
        StartCoroutine(SpriteFade(alien.GetComponent<SpriteRenderer>(), 0, 3));
        StartCoroutine(SpriteFade(house.GetComponent<SpriteRenderer>(), 0, 3));

        yield return new WaitForSeconds(6);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StarGame");
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
