using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    // Welcome to spaghetti code!

    Transform cam;
    public Transform ui;
    public Transform centerPos;
    public GameObject spawner;
    public Sprite explosionSprite;
    public GameObject gameManager;
    public AudioClip boom;

    public bool rocketMoving;
    public bool camMoving;
    bool canStrafe;
    bool isDead;
    bool isFinished;
    bool enableTimer;
    bool enableFinalTimer;
    bool enableGameOverTimer;
    bool triggered;
    bool incutscene;
    bool triggeronce;

    public float timer;
    public float finalTimer;
    public float gameOverTimer;

    Vector3 moveDir;

    [SerializeField] float acceleration = 0.005f;
    [SerializeField] float maxSpeed = 0.1f;
    [SerializeField] float speed = 0;
    [SerializeField] float strafeSpeed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        moveDir = new Vector3(0, 0, 0);

        timer = PlayerPrefs.GetInt("AsteroidTime", 27);

        if (GameObject.FindGameObjectWithTag("Settings") != null)
            if ((gameManager.GetComponent<SceneChanger>().prevScene == "MainMenu" && GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>().skipToAsteroids)
                || gameManager.GetComponent<SceneChanger>().prevScene == "RocketGame")
                SkipToAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        // "Cutscene" movement and acceleration
        if (speed < maxSpeed && rocketMoving && !isDead)
        {
            speed += acceleration * Time.deltaTime;
            moveDir = new Vector3(0, speed, 0);
        }

        // Asteroid timer
        if(enableTimer) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (finalTimer <= 5 && gameManager.activeSelf && !triggered)
            {
                triggered = true;
                gameManager.GetComponent<EventController>().PlayAudio();
            }
            enableTimer = false;
            timer = 0;
            spawner.GetComponent<AsteroidSpawner>().canSpawn = false;
            if(!isDead)
                enableFinalTimer = true;
        }

        // Final "cutscene" timer
        if (enableFinalTimer) finalTimer -= Time.deltaTime;
        if (finalTimer <= 0)
        {
            enableFinalTimer = false;
            finalTimer = 0;
            isFinished = true;
            transform.position = Vector3.Slerp(transform.position, new Vector3(0, 20, 0), Time.deltaTime/5);
        }

        // UI Timer
        if (ui.Find("TimerText").gameObject.activeSelf)
        {
            ui.Find("TimerText").GetComponent<Text>().text = (timer + finalTimer).ToString("N0");
        }

        // "Cutscene"
        if ((rocketMoving || incutscene) && !isDead)
        {
            incutscene = true;
            // Enable gameplay
            if (transform.position.y >= 5.95f) 
            {
                camMoving = false;
                rocketMoving = false;
                transform.position = Vector3.Slerp(transform.position, centerPos.position, speed / 2);

                if (gameManager.GetComponent<EventController>().finishedTalking)
                {
                    incutscene = false;
                    canStrafe = true;
                    spawner.GetComponent<AsteroidSpawner>().enabled = true;
                    enableTimer = true;
                    ui.Find("TimerText").gameObject.SetActive(true);
                }

            }
            // Center during "cutscene"
            else if (camMoving)
            {
                cam.Translate(moveDir);
                transform.position = Vector3.Slerp(transform.position, centerPos.position, speed / 2);
                transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(0.1f, 0.1f, 1), speed / 2);
            }
            // Go up
            else
                transform.Translate(moveDir);
        }

        // Disable timers and restart on gameover
        if (isFinished)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(0f, 0f, 1), Time.deltaTime / 4);
            enableTimer = false;
            enableFinalTimer = false;

            if (isDead)
            {
                enableGameOverTimer = true;
                ui.Find("GameOverPanel").gameObject.SetActive(true);

                if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        // Click to start
        if (Input.GetMouseButtonDown(0) && !rocketMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.transform.name == "Rocket")
                {
                    rocketMoving = true;
                    GetComponent<Animator>().enabled = true;
                    GetComponent<AudioSource>().enabled = true;
                    ui.Find("HowToPlayPanel").gameObject.SetActive(false);
                    if (!triggeronce)
                    {
                        triggeronce = true;
                        gameManager.GetComponent<AudioSource>().Stop();
                        gameManager.GetComponent<EventController>().PlayAudio();
                    }
                }
                Debug.Log("Hit: " + hit.transform.name);
            }
        }

        // Left/Right movement
        if (canStrafe && !isDead)
        {
            if (transform.position.x >= -6.5f && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
            {
                transform.Translate(-strafeSpeed, 0, 0);
            }
            if (transform.position.x <= 6.5f && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
            {
                transform.Translate(strafeSpeed, 0, 0);
            }
        }

        // Gameover timer
        if (enableGameOverTimer) gameOverTimer -= Time.deltaTime;
        if (gameOverTimer <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Go to next scene
        if (transform.position.y >= 16) SceneManager.LoadScene("HouseGame");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log("Trigger: " + collision.transform.name);

            // Start "cutscene"
            if (collision.transform.name == "StartTrigger")
            {
                camMoving = true;
            }

            // Gameover on collision
            else if (collision.transform.CompareTag("Asteroid"))
            {
                isFinished = true;
                isDead = true;
                canStrafe = false;
                GetComponent<Animator>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = explosionSprite;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().clip = null;
                GetComponent<AudioSource>().PlayOneShot(boom);
            }
        }
    }

    private void SkipToAsteroids()
    {
        transform.position = centerPos.position;
        transform.localScale = new Vector3(0.1f, 0.1f, 1);

        cam.position = new Vector3(0, 9.1f, -10);

        GetComponent<Animator>().enabled = true;
        GetComponent<AudioSource>().enabled = true;
        spawner.GetComponent<AsteroidSpawner>().enabled = true;
        gameManager.GetComponent<EventController>().gameState = 2;
        gameManager.GetComponent<AudioSource>().Stop();

        ui.Find("HowToPlayPanel").gameObject.SetActive(false);
        ui.Find("TimerText").gameObject.SetActive(true);

        enableTimer = true;
        canStrafe = true;
    }
}
