using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

	public int timeToRise = 3;
	public Text introText;

	private Camera cam;
	private GameObject ellen;

	private GameObject menu;
	private GameObject intro;
	private GameObject ready;

	private GameObject risingPlatform;

	private Vector3 startPos;
	private Vector3 endPos = new Vector3(0, 3, 0); 

	float t;
	private bool showStarted = false;
	private bool readyCheck = false;

	private void Start() {
		cam = Camera.main;
		
		menu = GameObject.Find("Canvas/Menu");
		intro = GameObject.Find("Canvas/IntroText");
		ready = GameObject.Find("Canvas/ReadyCheck");

		intro.SetActive(false);
		ready.SetActive(false);

		ellen = GameObject.Find("EllenDeCube");		
		risingPlatform = GameObject.Find("Platform/Rising");
	}

	private void Update() {
		if (readyCheck) {
			if (ready.activeSelf == false) {
				ready.SetActive(true);
			}
		}
	}

	private void FixedUpdate() {
		if (showStarted) {
			t += Time.deltaTime/timeToRise;
            risingPlatform.transform.position = Vector3.Lerp(startPos, endPos, t);
		}
	}

	// TODO: Game Intro Music
	public void BeginShow() {
		Debug.Log("Lets start the show!");	
		startPos = risingPlatform.transform.position;
		menu.SetActive(false);		
		showStarted = true;

		StartCoroutine(StartInto());
	}

	// TODO: Show Credits
	public void ShowCredits() {
		Debug.Log("Show credits");
	}

	public void StartQuestions() {
		SceneManager.LoadScene("Questions");
	}

	IEnumerator StartInto() {
		yield return new WaitForSeconds(timeToRise);
		intro.SetActive(true);
		StartCoroutine(AnimateText("Hi! I'm Ellen DeCube, and welcome to my Game of Game of Games! Today we are going play a few word games for a chance to win bragging rights.    Are you ready?!"));		
	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		introText.text = "";
		while( i < strComplete.Length ){
			introText.text += strComplete[i++];
			yield return new WaitForSeconds(0.025F);
		}

		readyCheck = true;
	}
}