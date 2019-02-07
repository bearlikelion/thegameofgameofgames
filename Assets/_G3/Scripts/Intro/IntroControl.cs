using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour {
    public GameObject ellen;
    public RectTransform ready1, ready2;
	private GameObject menu, intro, risingPlatform;
    private Text text;

    public bool showStarted = false;	

	private void Start() {
        menu = GameObject.Find("Canvas/Menu");
        intro = GameObject.Find("Canvas/IntroText");        
        risingPlatform = GameObject.Find("Platform/Rising");
        text = GameObject.Find("Canvas/IntroText/Text").GetComponent<Text>();

        if (showStarted) {
            BeginShow();
        }
	}

	// Begin the show
	private void BeginShow() {
        Debug.Log("Lets start the show!");                
        showStarted = true;
        menu.SetActive(false);
        ellen.transform.DOShakeRotation(25f, new Vector3(0, 90, 0), 1).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
        ellen.transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 25f, 3).SetSpeedBased().SetLoops(-1, LoopType.Yoyo);
        risingPlatform.transform.DOMoveY(5, 5f).OnComplete(IntroPanel);        
    }

    private void IntroPanel() {
        intro.transform.DOScale(Vector3.one, 1).OnComplete(IntroText);        
    }

    private void IntroText() {        
        string intro = "Hi! I'm Ellen DeCube, and welcome to my Game of Game of Games! Today we are going play some trivia games for a chance to win... <i>bragging rights</i>. <b>Are you ready?!</b>";
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