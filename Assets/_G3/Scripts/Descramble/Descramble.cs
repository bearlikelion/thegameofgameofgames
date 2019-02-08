using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Descramble : MonoBehaviour, Category {

    public GameObject answerPrefab, inputFieldPrefab;    

	private GameShow _gameShow;		
	private GameObject answer, userInput;
    private string _category = "Descramble the Word", _scrambled;    
	
	private DSWords _current;
	private List<DSWords> unanswered;
    private System.Random rnd = new System.Random();

    [SerializeField]
    private DSWords[] _words;

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
			unanswered = _words.ToList<DSWords>();
		}        
	}

    public void SetQuestion () {        
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = _category;
        _gameShow.Question = ScrambleWord(_current.word);
		MakeInputField();
	}
   
    void MakeInputField () {
        answer = Instantiate(answerPrefab, userInput.transform);
        answer.GetComponent<Text>().text = "The answer was: " + _scrambled;        
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

            if (input.text.ToLower() == _current.word.ToLower()) {
                _gameShow.CorrectAnswer();
            } else {
                answer.SetActive(true);
                answer.GetComponent<Text>().DOText("The  answer was: " + _current.word, 1f);
                _gameShow.WrongAnswer();
            }
        }        
    }

    string ScrambleWord (string word) {
        int index = 0;
        char[] chars = new char[word.Length];        

        while (word.Length > 0) {
            int next = rnd.Next(0, word.Length - 1);
            chars[index] = word[next];
            word = word.Substring(0, next) + word.Substring(next + 1);
            ++index;
        }
        _scrambled = new string(chars);
        return new string(chars);
    }
}
