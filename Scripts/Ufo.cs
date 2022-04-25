using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    public float duration;
    float scaleModifier = 1;

    [SerializeField] Vector3 m_from = new Vector3(0.0F, 45.0F, 0.0F);
    [SerializeField] Vector3 m_to = new Vector3(0.0F, -45.0F, 0.0F);
    [SerializeField] float m_frequency = 1.0F;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpPosition(new Vector3(-8, -3, 0), duration));
        StartCoroutine(LerpScale(2, duration));
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion from = Quaternion.Euler(m_from);
        Quaternion to = Quaternion.Euler(m_to);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * m_frequency));
        transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    IEnumerator LerpScale(float endValue, float duration)
    {
        float time = 0;
        float startValue = scaleModifier;
        Vector3 startScale = transform.localScale;
        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            transform.localScale = startScale * scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale * endValue;
        scaleModifier = startValue;
    }
}
