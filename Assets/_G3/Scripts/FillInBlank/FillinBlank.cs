using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FillinBlank : MonoBehaviour, Category {

    public GameObject answerPrefab, inputFieldPrefab;    

	private GameShow _gameShow;		
	private GameObject answer, userInput;
    private string _category = "Fill in the Blank";    
	
	private FBlank _current;
	private List<FBlank> unanswered;    

    [SerializeField]
    private FBlank[] _questions;

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
			unanswered = _questions.ToList<FBlank>();
		}        
	}

    public void SetQuestion () {        
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = _category;
        _gameShow.Question = _current.question;
		MakeInputField();
	}
   
    void MakeInputField () {
        answer = Instantiate(answerPrefab, userInput.transform);
        answer.GetComponent<Text>().text = _current.question;        
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
            
            if (input.text == _current.blank) {
                _gameShow.CorrectAnswer();
            } else {
                answer.SetActive(true);
                int underscores = _current.question.Count(f => f == '_');
                string _replace = "";

                for (int i = 0; i < underscores; i++) {
                    _replace += "_";
                }

                string solution = _current.question.Replace(_replace, "<i>"+_current.blank+"</i>");
                answer.GetComponent<Text>().DOText(solution, 1f);
                _gameShow.WrongAnswer();
            }
        }
    }    
}
