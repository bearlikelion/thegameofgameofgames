using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Analogies : MonoBehaviour, Category {

    public GameObject answerPrefab, inputFieldPrefab;    

	private GameShow _gameShow;		
	private GameObject answer, userInput;
    private string _category = "Complete the Analogy", blank, aText;
	
	private AGPrompts _current;
	private List<AGPrompts> unanswered;    

    [SerializeField]
    private AGPrompts[] _questions;

    public string Category {
        get { return _category; }
    }

    public int Count {
		get { return unanswered.Count; }
	}

	// Use this for initialization
	void Awake () {
		_gameShow = GameObject.Find("GameShow").GetComponent<GameShow>();
		userInput = GameObject.Find("Canvas/UserInput");

		if (unanswered == null) {
			unanswered = _questions.ToList<AGPrompts>();
		}        
	}

    public void SetQuestion () {        
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

        // TODO: Random Analogy blanks
        List<string> one = _current.one.Split(':').ToList<string>();
        List<string> two = _current.two.Split(':').ToList<string>();

        var leftorRight = Random.Range(0, 2);
        if (leftorRight == 0) {
            // left blank
            leftorRight = Random.Range(0,2);
            if (leftorRight == 0) {
                aText = one[0].Trim();
                one[0] = MakeBlank(one[0]);
            } else if (leftorRight == 1) {
                aText = one[1].Trim();
                one[1] = MakeBlank(one[1]);
            }
        } else if (leftorRight == 1) {
            // right blank
            leftorRight = Random.Range(0, 2);
            if (leftorRight == 0) {
                aText = two[0].Trim();
                two[0] = MakeBlank(two[0]);
            } else if (leftorRight == 1) {
                aText = two[1].Trim();
                two[1] = MakeBlank(two[1]);
            }
        }

        string analogy = one[0] + " : " + one[1] + " :: " + two[0] + " : " + two[1];

        _gameShow.Category = _category;
        _gameShow.Question = analogy; 
		MakeInputField();
	}

    string MakeBlank(string word) {
        int chars = word.Trim().Count();
        string _return = "";

        for (int i = 0; i < chars; i++) {
            _return += "_";
        }

        blank = _return;

        return _return;
    }
   
    void MakeInputField () {
        answer = Instantiate(answerPrefab, userInput.transform);
        answer.GetComponent<Text>().text = "The answer was: " + blank; // TODO: Reveal Answer
        answer.tag = "UserInput";
        answer.SetActive(false);

        GameObject inputFieldGO = Instantiate(inputFieldPrefab, userInput.transform);
        InputField inputField = inputFieldGO.GetComponent<InputField>();        
        inputField.ActivateInputField();
        inputField.tag = "UserInput";
        inputField.Select();        

        inputField.GetComponent<InputField>().onEndEdit.AddListener(delegate { SubmitGuess(inputField); });        
    }

    void SubmitGuess (InputField input) {
        if (input.text != "") {
            input.interactable = false;                      

            if (input.text.ToLower() == aText.ToLower()) {
                _gameShow.CorrectAnswer();
            } else {
                answer.SetActive(true);
                answer.GetComponent<Text>().DOText("The answer was: " + aText, 1f);
                _gameShow.WrongAnswer();
            }
        }        
    }    
}
