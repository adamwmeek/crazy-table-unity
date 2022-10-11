using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GenPlayerController : MonoBehaviour
{
    public float speed = 1;
    public TextAsset scoreAsset;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tutorialText;

    public GameObject pickups;
    public GameObject avoids;

    private Rigidbody body;
    private float moveX;
    private float moveZ;
    private float calbX;
    private float calbZ;
    private int score;
    private int highScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        else
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }

        // Randomly generate pickups
        for(int i=0; i < 18; i++)
        {
            float x = Random.Range(-9.5f, 9.5f);
            float z = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(x, 0.5f, z);
            Quaternion rotation = new Quaternion(0.25f, 0.25f, 0.25f, 1.0f);
            GameObject obj = Instantiate(pickups, position, rotation);
        } 

        // Randomly generate avoids
        for(int i=0; i < 4; i++)
        {
            float x = Random.Range(-9.5f, 9.5f);
            float z = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(x, 0.5f, z);
            Quaternion rotation = new Quaternion(0.25f, 0.25f, 0.25f, 1.0f);
            GameObject obj = Instantiate(avoids, position, rotation);
        } 

        score = 0;
        
        UpdateScore();
        StartCoroutine(TutTextWaiter());
        CalibrateAccel();
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }

    void CalibrateAccel()
    {
        calbX = Input.acceleration.x;
        calbZ = Input.acceleration.z;
    }

    IEnumerator TutTextWaiter() 
    {
        Time.timeScale = 0;
        tutorialText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        tutorialText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator LoseWaiter() 
    {
        Time.timeScale = 0;
        Handheld.Vibrate();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", score);
        }
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.acceleration.x - calbX;
        moveZ = Input.acceleration.y - calbZ;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 moveVector = movementValue.Get<Vector2>();

        moveX = moveVector.x;
        moveZ = moveVector.y;
    }

    void FixedUpdate()
    {
        Vector3 moveVector = new Vector3(moveX, 0.0f, moveZ);
        
        body.AddForce(moveVector * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            score++;
            UpdateScore();
        }

        if (other.gameObject.CompareTag("Avoid")) 
        {
            StartCoroutine(LoseWaiter());
        }
    }
}
