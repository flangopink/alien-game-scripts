using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroid;
    public float spawnDelay = 1;
    public float spawnCount = 1;
    public bool canSpawn;
    Vector3 spawnOffset;
    float delayTimer;

    private void Start()
    {
        spawnDelay = PlayerPrefs.GetFloat("AsteroidDelay", 2f);
    }

    public void FixedUpdate()
    {
        delayTimer += Time.deltaTime;
        if (canSpawn)
            if (delayTimer >= spawnDelay)
            {
                delayTimer = 0;
                for (int i = 0; i < spawnCount; i++)
                {
                    spawnOffset.x = Random.Range(-6, 7);
                    Instantiate(asteroid, transform.position + spawnOffset, transform.rotation);
                }
            }
    }
}
