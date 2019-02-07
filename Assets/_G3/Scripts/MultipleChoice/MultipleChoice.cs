﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoice : MonoBehaviour {	
	
	public GameObject buttonPrefab;

	private GameShow _gameShow;		
	private GameObject userInput;
	
	private MCQuestions _current;
	private List<MCQuestions> unanswered;

	[SerializeField] 
	private MCQuestions[] _questions;	

	public int Count {
		get { return unanswered.Count; }
	}

	// Use this for initialization
	void Start () {
		_gameShow = GameObject.Find("GameShow").GetComponent<GameShow>();
		userInput = GameObject.Find("Canvas/UserInput");

		if (unanswered == null) {
			unanswered = _questions.ToList<MCQuestions>();
		}

		SetQuestion();
	}
	
	void SetQuestion() {		
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = "Multiple Choice";
		_gameShow.Question = _current.question;		
		MakeAnswerButtons();
		
		_gameShow.StartQuestion();
	}

	void MakeAnswerButtons() {        
		List<float> positionsX = new List<float>();
        List<float> positionsY = new List<float>();

        if (_current.answers.Count() == 2) {
			positionsX.Add(-150);
			positionsX.Add(150);
        } else if (_current.answers.Count() == 3) {
			positionsX.Add(-225);
			positionsX.Add(0);
			positionsX.Add(225);
        }

		foreach (MCAnswers answer in _current.answers) {
			int r = Random.Range(0, positionsX.Count);			
			GameObject button = Instantiate(buttonPrefab);
			button.transform.SetParent(userInput.transform);
						
			button.transform.localPosition = new Vector3(positionsX[r], 0, 0);
			button.transform.localScale = Vector3.one;
			button.tag = "UserInput";
			
			button.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(answer.correctAnswer));
			button.GetComponentInChildren<Text>().text = answer.choice;
			positionsX.Remove(positionsX[r]);
		}
	}

	void SelectAnswer(bool isCorrect) {
		if (isCorrect) {			
			_gameShow.CorrectAnswer();
		} else {			
			_gameShow.WrongAnswer();
		}		
	}

}
