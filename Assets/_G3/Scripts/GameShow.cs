﻿#pragma warning disable 0649

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameShow : MonoBehaviour {
    
    private Text timerText;
    private AudioSource audioSource;
    private bool timerStarted = false;
    private int correct, strikes, speed;
    private System.Random rnd = new System.Random();
    private float timeLeft, countdown = 10.0f, waitTime = 1.0f;
    
    [SerializeField] private Image timerImage;
    [SerializeField] private Text categoryText, questionText;
    [SerializeField] private GameObject correctPanel, wrongPanel;
    [SerializeField] private AudioClip correctSound, wrongSound, timerSound;
    [SerializeField] private List<GameObject> categories;

    public int Correct {
        get { return correct; }
    }

    public int Strikes {
        get { return strikes; }
    }

    public string Category {
        get { return categoryText.text; }
        set { categoryText.text = value; }
    }

    public string Question {
        get { return questionText.text; }
        set { questionText.text = value; }
    }        

    // Use this for initialization
    void Start () {        
        timerText = timerImage.GetComponentInChildren<Text>();
        audioSource = GetComponent<AudioSource>();        
        NewQuestion();
    }

    void Update () {
        if (timerStarted) {
            CountdownTimer();
        }
    }   

	public void CorrectAnswer() {
		Debug.Log("Correct!");

        correct++;
        StopTimer();
        audioSource.PlayOneShot(correctSound);

        correctPanel.SetActive(true);
        StartCoroutine(NextQuestion());
	}

	public void WrongAnswer() {        
        Debug.Log("Wrong!");
        string strikeText = "";

        strikes++;
        StopTimer();
        audioSource.PlayOneShot(wrongSound);
                
        for (int i = 0; i < strikes; i++) {
            strikeText += " [X] ";
        }

        wrongPanel.GetComponentInChildren<Text>().text = strikeText;
        wrongPanel.SetActive(true);
        StartCoroutine(NextQuestion());
	}

    private void NewQuestion () {
        if (strikes < 3) {
            if (categories.Count > 0) {
                int r = rnd.Next(categories.Count);
                Category category = categories[r].GetComponent<Category>();
                if (category.Count > 0) {
                    category.SetQuestion();

                    timerImage.fillAmount = 1f;
                    timeLeft = countdown;
                    timerStarted = true;

                    audioSource.PlayOneShot(timerSound);
                } else {
                    // Out of questions for category
                    categories.Remove(categories[r]);
                    NewQuestion();
                }
            } else {
                GameOver(true);
            }
        } else {
            GameOver();
        }
    }

    private void GameOver(bool noQuestions = false) {
        Debug.Log("Game Over! " + "Out of questions? " + noQuestions);
    }

    private void StopTimer() {
        audioSource.Stop();
        timerStarted = false;
    }

	private void CountdownTimer() {
		timeLeft -= Time.deltaTime;
		timerText.text = Mathf.Round(timeLeft).ToString();
		timerImage.fillAmount -= 1.0f / countdown * Time.deltaTime;

        if (timerImage.fillAmount <= 1.0f && timerImage.fillAmount > 0.66f && timerImage.color != Color.white) {
            timerImage.color = Color.white;
            timerText.color = Color.white;
        } else if (timerImage.fillAmount <= 0.66f && timerImage.fillAmount > 0.33f && timerImage.color != Scheme.Yellow) {
			timerImage.color = Scheme.Yellow;
			timerText.color = Scheme.Yellow;
		} else if (timerImage.fillAmount <= 0.33f && timerImage.color != Scheme.Red) {
			timerImage.color = Scheme.Red;
			timerText.color = Scheme.Red;
		}

		if (timerImage.fillAmount == 0) {            
			WrongAnswer();
		}
	}

    void RemoveUIChildren() {
        GameObject userInput = GameObject.Find("Canvas/UserInput");
        foreach (Transform child in userInput.transform) {
            Destroy(child.gameObject);
        }
    }

    void HideResult() {
        if (correctPanel.gameObject.activeSelf) {
            correctPanel.gameObject.SetActive(false);
        }

        if (wrongPanel.gameObject.activeSelf) {
            wrongPanel.gameObject.SetActive(false);
        }
    }

	IEnumerator NextQuestion() {		
        speed += (int) Mathf.Round(timeLeft);        
		yield return new WaitForSeconds(waitTime);        
        RemoveUIChildren();
        HideResult();
        NewQuestion();
	}
}
