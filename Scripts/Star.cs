using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Transform center;
    StarParent parent;
    public float duration;
    int hitCount = 0;
    bool canHit = true;

    float scaleModifier = 1;

    void Start()
    {
        parent = transform.parent.GetComponent<StarParent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.transform == transform && canHit)
                {
                    if (hitCount == 0 && parent.busy) return;

                    hitCount++;

                    if (hitCount == 1)
                    {
                        StartCoroutine(IgnoreRaycast(duration));
                        StartCoroutine(LerpPosition(center.position, duration));
                        StartCoroutine(LerpScale(5, duration));
                        StartCoroutine(LerpRotate(duration));
                        parent.busy = true;
                    }

                    if (hitCount == 2)
                    {
                        StartCoroutine(IgnoreRaycast(duration/2));
                        StartCoroutine(LerpPosition(new Vector2(Random.Range(-6,7), Random.Range(-4, 5)), duration/2));
                        StartCoroutine(LerpScale(0.1f, duration/2));
                        StartCoroutine(LerpRotate(duration/2));
                        StartCoroutine(SortToBack(duration / 2));

                        hit.transform.GetComponent<Collider2D>().enabled = false;
                        parent.busy = false;
                        parent.stars.Remove(GetComponent<Collider2D>());
                    }
                    Debug.Log("Hit: " + hit.transform.name);
                }
            }

            if (parent.stars.Count == 0) StartCoroutine(RotateMultiple());
        }
    }

    IEnumerator RotateMultiple()
    {
        StartCoroutine(LerpRotate(duration));
        yield return new WaitForSeconds(2);
        StartCoroutine(LerpRotate(duration));
        yield return new WaitForSeconds(2);
        StartCoroutine(LerpRotate(duration));
    }

    IEnumerator SortToBack(float duration)
    {
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().sortingOrder = -3;
        transform.Find("card").GetComponent<SpriteRenderer>().sortingOrder = -2;
    }

    IEnumerator IgnoreRaycast(float duration)
    {
        canHit = false;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        canHit = true;
    }

    IEnumerator LerpRotate(float duration)
    {
        Vector3 startRotation = transform.eulerAngles;
        float endRotation = startRotation.y + 360.0f;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation.y, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(startRotation.x, startRotation.y, zRotation);
            yield return null;
        }
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
