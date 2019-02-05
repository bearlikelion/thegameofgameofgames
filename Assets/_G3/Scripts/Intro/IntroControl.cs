using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour {
	
	public Text introText;

	private GameObject menu;
	private GameObject intro;
	private GameObject ready;

	private GameObject risingPlatform;

	float t;

    public bool showStarted = false;
	private bool readyCheck = false;

	private void Start() {	
		menu = GameObject.Find("Canvas/Menu");
		intro = GameObject.Find("Canvas/IntroText");
		ready = GameObject.Find("Canvas/ReadyCheck");
        risingPlatform = GameObject.Find("Platform/Rising");

        if (intro != null) intro.SetActive(false);
        if (ready != null) ready.SetActive(false);   
        
        if (showStarted) {
            BeginShow();
        }
	}

	private void Update() {
		if (readyCheck) {
			if (ready.activeSelf == false) {
				ready.SetActive(true);
			}
		}
	}

	// TODO: Game Intro Music
	public void BeginShow() {
		Debug.Log("Lets start the show!");

        risingPlatform.transform.DOMoveY(3, 1.5f);
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
		yield return new WaitForSeconds(1.5f);
		intro.SetActive(true);
		StartCoroutine(AnimateText("Hi! I'm Ellen DeCube, and welcome to my Game of Game of Games! Today we are going play some trivia games for a chance to win... bragging rights. Are you ready?!"));		
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