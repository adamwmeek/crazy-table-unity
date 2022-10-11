using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartButtonController : MonoBehaviour
{
    public TextAsset scoreAsset;
    public TextMeshProUGUI scoreText;
    public Button tutorialButton;
    public int freePlayLevel = 5;
    public int tutorialLevel = 1;
    
    private bool finishedTutorial = false;
    private int highScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            finishedTutorial = true;
            highScore = PlayerPrefs.GetInt("HighScore");

            scoreText.text = $"High Score: {highScore}";
        }

        scoreText.enabled = finishedTutorial;
        tutorialButton.gameObject.SetActive(finishedTutorial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene(tutorialLevel);
    }

    public void PlayGame()
    {
        if(finishedTutorial)
        {
            SceneManager.LoadScene(freePlayLevel);
        }
        else
        {
            PlayTutorial();
        }
    }
}
