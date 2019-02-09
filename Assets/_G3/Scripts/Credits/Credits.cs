using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public GameObject ellen;
    public Transform textPanel;
    public GameObject creditsPanel;
    public Text creditsScroll;
    public Text creditsLogo;


    private GameShow _gameShow;
    private GameManager _gameManager;
    public int correct = 5;
    public int strikes = 3;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("--- GameManager ---")) {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _gameShow = GameObject.Find("GameShow").GetComponent<GameShow>();
            correct = _gameManager.correct;
            strikes = _gameManager.strikes;
        }

        ellen.transform.DOShakeRotation(1, new Vector3(10f, 90f, 0f), 1).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
        textPanel.DOScale(1, 1).OnComplete(EndGame);
    }

    void EndGame() {
        string outro = "";
        Text text = textPanel.GetComponentInChildren<Text>();

        if (strikes < 3) {
            outro = "WOW, YOU GOT THROUGH EVERY QUESTION! Getting <b>" + correct.ToString() + "</b> correct! Congratulations!!";
        } else {
            if (correct > 0) {
                outro = "WOAH, tough break! Congratulations on getting <b>" + correct.ToString() + "</b> questions right!";
            } else {
                outro = "AWW, I'm sorry! You didn't get <i>any</i> questions correct! Better luck next time!";
            }
        }

        text.DOText(outro, 25f).SetSpeedBased().OnComplete(RollCredits);
    }

    // ROLL CREDITS
    void RollCredits() {
        textPanel.gameObject.SetActive(false);
        creditsPanel.transform.DOLocalMoveY(-53f, 5f);
    }
}
