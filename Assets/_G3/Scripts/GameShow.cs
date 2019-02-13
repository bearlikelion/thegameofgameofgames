#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameShow : MonoBehaviour {

    public bool skipReady, shuffleCategories;
    public int questionLimit = 5;

    private int questionCount = 0;
    private string challengeString = "Hard Mode: Shuffle All Categories";
    private bool timerStarted = false, ticked = false, readyCount = false;
    private float timeLeft, countdown = 10.0f, waitTime = 1.5f, readyTimer = 3.0f;

    private Text timerText;
    private Image timerImage;
    private Category _category;
    private AudioSource audioSource;
    private GameManager _gameManager;
    private List<GameObject> categoryChoice;
    private System.Random rnd = new System.Random();

    [SerializeField] private GameObject correctPanel, wrongPanel, countdownPanel, timer, buttonPrefab, userInput;
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

    public float Timer {
        set { countdown = value; }
    }

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timerText = timer.GetComponentInChildren<Text>();
        timerImage = timer.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        questionText.text = "";
        correctScore.text = "";
        strikeScore.text = "";

        SelectCategory();
    }

    void Update () {
        if (readyCount) {
            StartCountdown();
        }
        if (timerStarted) {
            CountdownTimer();
        }
    }

    private void SelectCategory() {
        Debug.Log("Select Category");
        List<string> category = new List<string>();
        questionText.text = "";
        shuffleCategories = false;

        for (int i = 0; i < categories.Count; i++) {
            Category _cat = categories[i].GetComponent<Category>();
            if (_cat.Count == 0) {
                categories.Remove(categories[i]);
            }
        }

        if (categories.Count < 1) {
            Debug.Log("Out of categories!");
            _gameManager.GameOver();
        }
        if (categoryChoice == null || categoryChoice.Count < 3) {
            categoryChoice = categories.OrderBy(x => rnd.Next()).ToList();
        }

        categoryText.text = "Select a Category";
        timer.SetActive(false);

        int l = 0;
        foreach (GameObject _go in categoryChoice) {
            if (l < 3) {
                category.Add(_go.name);
                l++;
            } else {
                break;
            }
        }
        category.Add(challengeString);

        // build category buttons
        List<Vector3> positions = new List<Vector3>();
        positions.Add(new Vector3(-225, 0, 0));
        positions.Add(new Vector3(0, 0, 0));
        positions.Add(new Vector3(225, 0, 0));
        positions.Add(new Vector3(0, 0, 0));

        foreach (string catString in category) {
            // Hard Mode button
            if (catString == challengeString) {
                GameObject button = Instantiate(buttonPrefab, GameObject.Find("Canvas/QuestionPanel").transform);
                button.GetComponentInChildren<Text>().text = catString;
                button.tag = "UserInput";
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 75);
                button.GetComponent<Image>().color = Scheme.Red;
                button.GetComponent<Button>().onClick.AddListener(() => CategoryIs(catString));
            } else {
                GameObject button = Instantiate(buttonPrefab, userInput.transform);
                button.transform.localPosition = positions.First();
                button.tag = "UserInput";
                positions.RemoveAt(0);

                button.GetComponentInChildren<Text>().text = catString;
                button.GetComponent<Button>().onClick.AddListener(() => CategoryIs(catString));
            }
        }
    }

    void CategoryIs(string category){
        if (category == "Hard Mode: Shuffle All Categories") {
            shuffleCategories = true;
        } else {
            for (int i=0; i < categories.Count; i++){
                if (categories[i].name == category) {
                    _category = categories[i].GetComponent<Category>();
                }
            }

            for (int i=0; i < categoryChoice.Count; i++){
                if (categoryChoice[i].name == category) {
                    categoryChoice.Remove(categoryChoice[i]);
                }
            }
        }

        questionCount = 0;
        RemoveUIChildren();
        if (shuffleCategories) {
            readyCount = true;
        } else {
            NewQuestion();
        }
    }

    private void NewQuestion () {
        if (_gameManager.strikes < 3) {
            if (shuffleCategories) {
                int r = Random.Range(0, categories.Count);
                _category = categories[r].GetComponent<Category>();
            }
            _category.SetQuestion();

            if (!timer.activeSelf) {
                timer.SetActive(true);
            }

            timerImage.fillAmount = 1f;
            timeLeft = countdown;
            timerStarted = true;

            audioSource.PlayOneShot(timerSound);
            questionCount++;
        } else {
            _gameManager.GameOver();
        }
    }

	public void CorrectAnswer() {
		Debug.Log("Correct!");

        StopTimer();
        _gameManager.correct++;
        audioSource.PlayOneShot(correctSound);

        correctPanel.SetActive(true);
        _gameManager.speed += (int) Mathf.Round(timeLeft);

        if (_gameManager.correct > 0) {
            correctScore.text += "✔️";
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
            strikeText += " X ";
        }

        strikeScore.text = strikeText;

        wrongPanel.GetComponentInChildren<Text>().text = strikeText;
        wrongPanel.SetActive(true);

        if (shuffleCategories) {
            StartCoroutine(NextQuestion());
        } else {
            StartCoroutine(CategorySelect());
        }
	}

    private void StopTimer() {
        audioSource.Stop();
        timerStarted = false;
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

        if (shuffleCategories) {
            NewQuestion();
        } else if (questionCount < questionLimit && _category.Count > 0) {
            NewQuestion();
        } else {
            SelectCategory();
        }
	}

    IEnumerator CategorySelect() {
		yield return new WaitForSeconds(waitTime);
        RemoveUIChildren();
        HideResult();
        SelectCategory();
	}

    IEnumerator TickClock () {
        audioSource.PlayOneShot(clockTick);
        yield return new WaitForSeconds(1f);
        ticked = false;
    }
}
