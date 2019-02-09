using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour {
    public GameObject ellen;
    public Button startButton;
    public RectTransform ready1, ready2;

    private Text text;
    private InputField inputField;
    private GameManager _gameManager;
    private GameObject menu, intro, risingPlatform;

    public bool showStarted = false;

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
	private void BeginShow() {
        Debug.Log("Lets start the show!");
        showStarted = true;
        menu.SetActive(false);
        _gameManager.playerName = inputField.text;

        ellen.transform.DOShakeRotation(25f, new Vector3(0, 90, 0), 1).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
        ellen.transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 25f, 3).SetSpeedBased().SetLoops(-1, LoopType.Yoyo);
        risingPlatform.transform.DOMoveY(3, 5f).OnComplete(IntroPanel);
    }

    private void IntroPanel() {
        intro.transform.DOScale(Vector3.one, 1).OnComplete(IntroText);
    }

    private void IntroText() {
        string intro = "Hi "+_gameManager.playerName+"! I'm Ellen DeCube, and welcome to my Game of Game of Games! Today we are going play some trivia games for a chance to win... <i>bragging rights</i>. <b>Are you ready?!</b>";
        text.DOText(intro, 25f).SetEase(Ease.Unset).SetSpeedBased().OnComplete(ReadyCheck);
    }

    private void ReadyCheck() {
        ready1.DOLocalMoveX(-150, 1);
        ready2.DOLocalMoveX(150, 1);
    }

	private void StartQuestions() {
        text.text = "";
        text.DOText("Perfect! Let's get to it!", 25f).SetEase(Ease.Unset).SetSpeedBased().OnComplete(LoadQuestionScene);
	}

    private void LoadQuestionScene() {
        SceneManager.LoadScene("Questions");
    }
}