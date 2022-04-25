using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    float timingOffset;

    bool phase2;
    public bool isGrabbed;

    readonly int[] ids = new int[3] { 0, 1, 2 };
    AudioSource audioSource;
    public AudioClip[] clips;
    public string task;
    int houseID;
    int overHouse = -1;

    public Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    AlienParent parent;
    bool tonce;
    bool toncerb;
    public bool canFloat;
    bool canEnterHouse;

    [SerializeField] float duration = 2;
    public int hitCount = 0;
    bool canHit = true;
    float scaleModifier = 1;

    void Awake()
    {
        timingOffset = Random.value/2 * (Mathf.PI / 2);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent.GetComponent<AlienParent>();
        posOffset = transform.position;

        string spriteName = transform.Find("card").GetComponent<SpriteRenderer>().sprite.name;

        if (spriteName.IndexOf('r') == 0) houseID = ids[0];
        else if (spriteName.IndexOf('r') == spriteName.Length - 1) houseID = ids[2];
        else houseID = ids[1];
    }

    // Update is called once per frame
    void Update()
    {
        phase2 = parent.phase1_done;

        if (!isGrabbed && canFloat)
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * timingOffset * frequency) * amplitude;

            transform.position = tempPos;
        }

        if (phase2)
        {
            if (!toncerb)
            {
                toncerb = true;
                GetComponent<Rigidbody2D>().isKinematic = false;
                transform.GetComponent<Collider2D>().enabled = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canFloat = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    if (hit.transform == transform)
                    {
                        if (!isGrabbed && houseID == overHouse)
                        {
                            parent.remainingAyys_p2.Remove(transform);
                            Destroy(gameObject);
                        }
                    }
                }
                canFloat = true;
            }
        }
        else
        {
            if (parent.remainingAyys_p1.Count != 0)
            {
                if (transform == parent.remainingAyys_p1[0] && !tonce)
                {
                    tonce = true;
                    StartCoroutine(LerpPosition(new Vector2(0, 2), duration));
                }
            }

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
                            StartCoroutine(LerpScale(2, duration));
                            switch (task)
                            {
                                case "count":
                                    audioSource.clip = clips[0];
                                    audioSource.Play();
                                    break;
                                case "laskovo":
                                    audioSource.clip = clips[1];
                                    audioSource.Play();
                                    break;
                                case "parts":
                                    audioSource.clip = clips[2];
                                    audioSource.Play();
                                    break;
                                case "kakoy":
                                    audioSource.clip = clips[3];
                                    audioSource.Play();
                                    break;
                            }
                            //StartCoroutine(PauseForDuration(audioSource.clip.length));
                            parent.busy = true;
                        }

                        if (hitCount == 2)
                        {
                            if (parent.remainingAyys_p1.Count == 1)
                                transform.position = new Vector2(10, 2);
                            else
                            {
                                StartCoroutine(IgnoreRaycast(duration));
                                StartCoroutine(LerpPosition(new Vector2(10, 2), duration));
                                StartCoroutine(LerpScale(0.5f, duration));
                            }

                            hit.transform.GetComponent<Collider2D>().enabled = false;
                            parent.busy = false;
                            parent.remainingAyys_p1.Remove(transform);

                            if (parent.houses.Count != 0)
                                RandomHouseNextSprite();

                        }
                        //Debug.Log("Hit: " + hit.transform.name);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "houseL" && collision.GetComponent<SpriteRenderer>().sprite.name == "house1_m_4")
            overHouse = 0;
        if (collision.gameObject.name == "houseM" && collision.GetComponent<SpriteRenderer>().sprite.name == "house3_m_4")
            overHouse = 1;
        if (collision.gameObject.name == "houseR" && collision.GetComponent<SpriteRenderer>().sprite.name == "house2_m_4")
            overHouse = 2;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        overHouse = -1;
    }

    void RandomHouseNextSprite()
    {
        int i = Random.Range(0, parent.houses.Count);
        parent.houses[i].GetComponent<House>().NextSprite();
    }

    IEnumerator IgnoreRaycast(float duration)
    {
        canHit = false;
        yield return new WaitForSeconds(duration);
        canHit = true;
    }

    IEnumerator PauseForDuration(float duration)
    {
        parent.GetComponent<AudioSource>().Pause();
        yield return new WaitForSeconds(duration);
        parent.GetComponent<AudioSource>().UnPause();
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
