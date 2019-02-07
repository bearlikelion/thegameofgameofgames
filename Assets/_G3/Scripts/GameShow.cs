using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameShow : MonoBehaviour {

	private Text timerText;
	private AudioSource audioSource;
	private bool timerStarted = false;
	private float timeLeft, countdown = 10.0f, waitTime = 1.0f;

	private int correct, strikes;
	public int Correct {
		get { return correct; }
	}

	public int Strikes {
		get { return strikes; }
	}

	[SerializeField] private Text categoryText, questionText;
	public string Category {
		get { return categoryText.text; }
		set { categoryText.text = value; }
	}

	public string Question {
		get { return questionText.text; }
		set { questionText.text = value; }
	}

	[SerializeField] private Image timerImage;
	[SerializeField] private AudioClip correctSound, wrongSound, missedSound, timerSound;

	// Use this for initialization
	void Start () {
		timerText = timerImage.GetComponentInChildren<Text>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		if (timerStarted) {
			CountdownTimer();
		}
	}

	// TODO: Category and question selection

	public void StartQuestion() {
		timerStarted = true;
		timeLeft = countdown;
		audioSource.PlayOneShot(timerSound);
	}

	public void CorrectAnswer() {
		Debug.Log("Correct!");
		audioSource.PlayOneShot(correctSound);
		timerStarted = false;
		correct++;

		StartCoroutine(NextQuestion());
	}

	public void WrongAnswer() {
		Debug.Log("Wrong!");
		audioSource.PlayOneShot(wrongSound);
		timerStarted = false;
		strikes++;

		StartCoroutine(NextQuestion());
	}

	private void TimesUp() {
		Debug.Log("Times Up!");
		audioSource.PlayOneShot(missedSound);
		timerStarted = false;
	}

	private void CountdownTimer() {
		timeLeft -= Time.deltaTime;
		timerText.text = Mathf.Round(timeLeft).ToString();
		timerImage.fillAmount -= 1.0f / countdown * Time.deltaTime;

		if (timerImage.fillAmount <= 0.66f && timerImage.fillAmount > 0.33f) {
			timerImage.DOColor(Color.yellow, 1);
			timerText.DOColor(Color.yellow, 1);
		} else if (timerImage.fillAmount <= 0.33f) {
			timerImage.DOColor(Color.red, 1);
			timerText.DOColor(Color.red, 1);
		}

		if (timerImage.fillAmount == 0) {
			// TimesUp();
			WrongAnswer();
		}
	}

	IEnumerator NextQuestion() {
		Debug.Log("Next Question!");
		yield return new WaitForSeconds(waitTime);
		// TODO: Go to the next question
	}
}
