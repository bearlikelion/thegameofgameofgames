#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameShow : MonoBehaviour {

    public bool skipReady;
    private bool timerStarted = false, ticked = false, readyCount = false;
    private float timeLeft, countdown = 10.0f, waitTime = 1.5f, readyTimer = 3.0f;

    private Text timerText;
    private Image timerImage;
    private AudioSource audioSource;
    private GameManager _gameManager;
    private System.Random rnd = new System.Random();
    private Category lastCategory = null, previousCategory = null;

    [SerializeField] private GameObject correctPanel, wrongPanel, countdownPanel, timer;
    [SerializeField] private AudioClip correctSound, wrongSound, timerSound, clockTick;
    [SerializeField] private Text categoryText, questionText, correctScore, strikeScore;
    [SerializeField] private List<GameObject> categories;

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
        questionText.text = "";
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timerText = timer.GetComponentInChildren<Text>();
        timerImage = timer.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        if (!skipReady) {
            readyCount = true;
        } else {
            NewQuestion();
        }
    }

    void Update () {
        if (readyCount) {
            StartCountdown();
        }
        if (timerStarted) {
            CountdownTimer();
        }
    }

    private void StartCountdown() {
        readyTimer -= Time.deltaTime;
        if (!countdownPanel.activeSelf) {
            countdownPanel.SetActive(true);
        }

        if (!ticked) {
            ticked = true;
            StartCoroutine(TickClock());
        }
        if (readyTimer > 0) {
            countdownPanel.transform.Find("Text").GetComponent<Text>().text = Mathf.Round(readyTimer).ToString();
        } else if(readyTimer < 0) {
            countdownPanel.SetActive(false);
            readyCount = false;
            NewQuestion();
        }
    }

    private void NewQuestion () {
        if (_gameManager.strikes < 3) {
            if (categories.Count > 0) {
                int r = rnd.Next(categories.Count);
                Category category = categories[r].GetComponent<Category>();

                if (lastCategory != null) previousCategory = lastCategory;
                lastCategory = category;

                // Triple repeat
                if (category == lastCategory && category == previousCategory && categories.Count > 1) {
                    NewQuestion();
                } else {
                    if (category.Count > 0) {
                        category.SetQuestion();

                        if (!timer.activeSelf) {
                            timer.SetActive(true);
                        }
                        timerImage.fillAmount = 1f;
                        timeLeft = countdown;
                        timerStarted = true;

                        audioSource.PlayOneShot(timerSound);
                    } else {
                        // Out of questions for category
                        categories.Remove(categories[r]);
                        NewQuestion();
                    }
                }
            } else {
                Debug.Log("Out of questions");
                // _gameManager.GameOver();
            }
        } else {
            _gameManager.GameOver();
        }
    }

	public void CorrectAnswer() {
		Debug.Log("Correct!");

        _gameManager.correct++;
        StopTimer();
        audioSource.PlayOneShot(correctSound);

        correctPanel.SetActive(true);
        _gameManager.speed += (int) Mathf.Round(timeLeft);

        if (_gameManager.correct > 0) {
            correctScore.text = "Correct: <color='#ffffff'>"+_gameManager.correct+"</color>";
        }

        StartCoroutine(NextQuestion());
	}

	public void WrongAnswer() {
        Debug.Log("Wrong!");
        string strikeText = "";

        _gameManager.strikes++;
        StopTimer();
        audioSource.PlayOneShot(wrongSound);

        for (int i = 0; i < _gameManager.strikes; i++) {
            strikeText += " [X] ";
        }

        strikeScore.text = strikeText;

        wrongPanel.GetComponentInChildren<Text>().text = strikeText;
        wrongPanel.SetActive(true);
        StartCoroutine(NextQuestion());
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
        GameObject[] inputs = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (GameObject input in inputs) {
            Destroy(input);
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
		yield return new WaitForSeconds(waitTime);
        RemoveUIChildren();
        HideResult();
        NewQuestion();
	}

    IEnumerator TickClock () {
        audioSource.PlayOneShot(clockTick);
        yield return new WaitForSeconds(1f);
        ticked = false;
    }
}
