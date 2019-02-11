﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour {
    public bool showStarted = false;
    public GameObject ellen, hiddenLogo;
    public Button startButton;
    public RectTransform ready1, ready2;

    private Text text;
    private InputField inputField;
    private GameManager _gameManager;
    private GameObject menu, intro, risingPlatform;

    private float textDelay = 3.0f;


    private void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        menu = GameObject.Find("Canvas/Menu");
        intro = GameObject.Find("Canvas/IntroText");
        risingPlatform = GameObject.Find("Platform/Rising");
        text = GameObject.Find("Canvas/IntroText/Text").GetComponent<Text>();
        inputField = GameObject.Find("Canvas/Menu/InputField").GetComponent<InputField>();

        inputField.ActivateInputField();
        inputField.Select();

        if (showStarted) {
            BeginShow();
        }
	}

    private void Update() {
        if (inputField.isFocused && inputField.text != "" && Input.GetKey(KeyCode.Return)) {
            BeginShow();
        }
    }

    public void EnableStart() {
        if (inputField.text != "") {
            if (Input.GetKey(KeyCode.Return)) {
                BeginShow();
            }
            startButton.interactable = true;
        } else {
            startButton.interactable = false;
        }
    }

	// Begin the show
	public void BeginShow() {                      
        showStarted = true;
        menu.SetActive(false);
        hiddenLogo.SetActive(true);
        _gameManager.playerName = inputField.text;

        // Ellen Dance
        ellen.transform.DOShakeRotation(1f, new Vector3(0, 135f, 0), 1).SetLoops(-1);
        ellen.transform.DOPunchPosition(new Vector3(0.25f, 0, 0), 1f, 1).SetLoops(-1);
        
        risingPlatform.transform.DOMoveY(3, 3f).OnComplete(IntroPanel);
    }

    public void LoadQuestionScene () {
        SceneManager.LoadScene("Questions");
    }

    private void IntroPanel() {
        intro.transform.DOScale(Vector3.one, 1f).OnComplete(StartIntroText);        
    }
    
    private void StartIntroText() {
        string introText = "Hi " + _gameManager.playerName + "! I am Ellen DeCube, and welcome to: \n <b>The Game of Game of Games!</b>";
        text.DOText(introText, textDelay).OnComplete(IntroText1);
    }
    
    private void IntroText1() {
        StartCoroutine(WriteIntro1());        
    }

    IEnumerator WriteIntro1 () {        
        yield return new WaitForSeconds(3f);
        text.text = "";
        string introText = "Select a category and answer the related questions.\n One wrong is a <b>strike</b>\n Three strikes and you lose!";
        text.DOText(introText, textDelay).OnComplete(IntroText2);
    }

    private void IntroText2 () {
        StartCoroutine(WriteIntro2());
    }

    IEnumerator WriteIntro2 () {
        yield return new WaitForSeconds(3f);
        text.text = "";
        string introText = "For a challenge, try playing with shuffled categories!";
        text.DOText(introText, textDelay).OnComplete(ReadyCheck);
    }

    private void ReadyCheck() {
        StartCoroutine(PromptReady());
    }

    IEnumerator PromptReady() {
        yield return new WaitForSeconds(3f);
        text.text = "";
        string introText = "Are you ready?!";
        text.DOText(introText, 1f);

        ready1.DOLocalMoveX(-175, 1);
        ready2.DOLocalMoveX(175, 1);        
    }
}