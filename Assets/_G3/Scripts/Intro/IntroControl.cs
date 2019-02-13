using DG.Tweening;
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

    [SerializeField]
    private float textDelay = 1.5f;


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

        if (_gameManager.isGameOver) {
            QueueEllen();
        }
	}

    private void Update() {
        if (inputField.isFocused && inputField.text != "" && Input.GetKey(KeyCode.Return)) {
            BeginShow();
        }

        if (showStarted && Input.GetKeyDown(KeyCode.Escape) || _gameManager.isGameOver && Input.GetKeyDown(KeyCode.Escape)) {
            if (_gameManager.isGameOver) {
                _gameManager.ShowScores();
            } else {
                LoadQuestionScene();
            }            
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
        if (_gameManager.playerName == "" && inputField.text != "") {
            _gameManager.playerName = inputField.text;
        }
        showStarted = true;        
        QueueEllen();
    }

    void QueueEllen() {
        menu.SetActive(false);
        hiddenLogo.SetActive(true);        

        // Ellen Dance
        ellen.transform.DOShakeRotation(1f, new Vector3(0, 135f, 0), 1).SetLoops(-1);
        ellen.transform.DOPunchPosition(new Vector3(0.25f, 0, 0), 1f, 1).SetLoops(-1);

        risingPlatform.transform.DOMoveY(3, 3f).OnComplete(IntroPanel);
    }

    public void LoadQuestionScene () {
        SceneManager.LoadScene("Questions");
    }

    private void IntroPanel() {
        if (_gameManager.isGameOver) {
            Debug.Log("End of the show bro");
            intro.transform.DOScale(Vector3.one, 1f).OnComplete(EndGameText);
        } else {
            intro.transform.DOScale(Vector3.one, 1f).OnComplete(StartIntroText);
        }
    }

    private void EndGameText() {
        string endText = "";
        if (_gameManager.strikes < 3) {
            endText = "Wow!! " + _gameManager.playerName + " you managed to make it through without getting three strikes!! \n Let's check the <b>scoreboard</b>!";
        } else {
            endText = "Aww sorry " + _gameManager.playerName + ", but three strikes and you're out! \n Why don't we check the <b>scoreboard</b>";
        }

        text.DOText(endText, textDelay).OnComplete(GoToLeaderboard);
    }

    private void GoToLeaderboard() {
        StartCoroutine(ShowScores());
    }

    IEnumerator ShowScores () {
        yield return new WaitForSeconds(textDelay);
        _gameManager.ShowScores();
    }

    private void StartIntroText() {
        string introText = "Hi " + _gameManager.playerName + "! I am Ellen DeCube, and welcome to: \n <b>The Game of Game of Games!</b>";
        text.DOText(introText, textDelay).OnComplete(IntroText1);
    }

    private void IntroText1() {
        StartCoroutine(WriteIntro1());
    }

    IEnumerator WriteIntro1 () {
        yield return new WaitForSeconds(textDelay);
        text.text = "";
        string introText = "Select a category and answer the related questions.\n One wrong is a <b>strike</b>\n Three strikes and you're out!";
        text.DOText(introText, textDelay).OnComplete(IntroText2);
    }

    private void IntroText2 () {
        StartCoroutine(WriteIntro2());
    }

    IEnumerator WriteIntro2 () {
        yield return new WaitForSeconds(textDelay);
        text.text = "";
        string introText = "For a challenge, try playing with shuffled categories!";
        text.DOText(introText, textDelay).OnComplete(ReadyCheck);
    }

    private void ReadyCheck() {
        StartCoroutine(PromptReady());
    }

    IEnumerator PromptReady() {
        yield return new WaitForSeconds(textDelay);
        text.text = "";
        string introText = "Are you ready?!";
        text.DOText(introText, 1f);

        ready1.DOLocalMoveX(-175, 1);
        ready2.DOLocalMoveX(175, 1);
    }
}