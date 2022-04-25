using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    public float rotSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = PlayerPrefs.GetFloat("AsteroidSpeed", 0.02f);
        transform.localRotation = Quaternion.Euler(0,0,Random.Range(0,360));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -moveSpeed, 0, Space.World);
        transform.Rotate(0,0,1, Space.Self);

        if (transform.position.y <= 3) Destroy(gameObject);
    }
}
