using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    private float moveX;
    private float moveZ;
    private float calbX;
    private float calbZ;

    private int score;

    public float speed = 1;
    public int winScore = 100;
    public int nextLevel = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI winText;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);

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

    IEnumerator WinTextWaiter() 
    {
        Time.timeScale = 0;
        winText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        winText.gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator LoseWaiter() 
    {
        Time.timeScale = 0;
        Handheld.Vibrate();
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

            if (score >= winScore)
            {
                StartCoroutine(WinTextWaiter());
            }
        }

        if (other.gameObject.CompareTag("Avoid")) 
        {
            StartCoroutine(LoseWaiter());
        }
    }
}
