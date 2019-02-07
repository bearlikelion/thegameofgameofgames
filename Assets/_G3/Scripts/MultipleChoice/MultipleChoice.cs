using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChoice : MonoBehaviour {	
	
	private GameShow _gameShow;		
	public GameObject userInput;	
	public GameObject buttonPrefab;

	private string category = "Multiple Choice";	

	[SerializeField]
	private MCQuestions[] _questions;
	private List<MCQuestions> unanswered;

	// Use this for initialization
	void Start () {
		_gameShow = GameObject.Find("GameShow").GetComponent<GameShow>();

		if (unanswered == null) {
			unanswered = _questions.ToList<MCQuestions>();
		}

		SetQuestion();
	}
	
	void SetQuestion() {		
		int r = Random.Range(0, unanswered.Count);
		MCAnswers[] answers = unanswered[r].answers;

		_gameShow.SetCategory(category);
		_gameShow.SetQuestion(unanswered[r].question);

		Debug.Log(answers.Count());

        if (answers.Count() == 2)
        {

        }
        if (answers.Count() > 4) {
			
		}

		// TODO: set answer buttons

		// Button to be evenly spaced and random
		// Support 2, 3, 4 answers
		// If more than 4 answers random select 3 + answer

		// foreach (var answer in answers) {
			// GameObject button = Instantiate(buttonPrefab);
			// button.transform.SetParent(userInput.transform);			
		// }
	}

}
