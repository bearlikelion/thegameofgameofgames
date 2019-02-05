using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public GameObject ellen;
    public Transform textPanel;

    private GameManager _gameManager;
    private int correct = 5;
    private int strikes = 3;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("--- GameManager ---")) {
            _gameManager = GameObject.Find("--- GameManager ---").GetComponent<GameManager>();            
            correct = _gameManager.correctAnswers;
            strikes = _gameManager.strikeCount;            
        }

        textPanel.DOScale(1, 1).OnComplete(EndGame);
    }

    void EndGame() {
        string outro = "";
        Text text = textPanel.GetComponentInChildren<Text>();

        if (strikes < 3) {
            outro = "WOW, YOU GOT THROUGH EVERY QUESTION! Getting <b>" + correct.ToString() + "</b> correct! Congratulations!! Thanks for playing!!";
        } else {
            if (correct > 0) {
                outro = "WOAH, tough break! Congratulations on getting <b>" + correct.ToString() + "</b> questions right! Thanks for playing!!";
            } else {
                outro = "AWW, I'm sorry! You didn't get <i>any</i> questions correct! Better luck next time! Thanks for playing!!";                
            }            
        }

        text.DOText(outro, 7).OnComplete(EllenDance);
    }

    void EllenDance() {
        ellen.transform.DOShakeRotation(5, new Vector3(0, 90, 0), 1).SetLoops(-1, LoopType.Yoyo);
        // ellen.transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 5, 3);
    }
    
    // TODO: ROLL CREDITS
}
