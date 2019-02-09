using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OddOneOut : MonoBehaviour, Category {

	public GameObject answerPrefab, buttonPrefab;

	private GameShow _gameShow;
	private GameObject userInput, answerButton;
    private string _category = "Odd One Out";

    private int previousRandom;
	private OOQuestions _current;
	private List<OOQuestions> unanswered;
    private System.Random rnd = new System.Random();

    [SerializeField]
    private OOQuestions[] _questions;

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
			unanswered = _questions.ToList<OOQuestions>();
		}
	}

    public void SetQuestion () {
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = _category;
		_gameShow.Question = "Which of these is not like the other?";
        // _gameShow.Question = _current.question;
		MakeAnswerButtons();
	}

	void MakeAnswerButtons() {
        List<string> answers = new List<string>();
        List<Vector3> positions = new List<Vector3>();
        string[] choices = _current.choices.OrderBy(x => rnd.Next()).ToArray();

        answers.Add(_current.answer);

        for (int i = 0; i < _current.choices.Count(); i++) {
            answers.Add(choices[i]);
        }

        if (answers.Count() == 2) {
            positions.Add(new Vector3(-150, 0, 0));
            positions.Add(new Vector3(150, 0, 0));
        } else if (answers.Count() == 3) {
            positions.Add(new Vector3(-225, 0, 0));
            positions.Add(new Vector3(0, 0, 0));
            positions.Add(new Vector3(225, 0, 0));
        } else if (answers.Count() == 4) {
            positions.Add(new Vector3(-150, 30, 0));
            positions.Add(new Vector3(-150, -30, 0));
            positions.Add(new Vector3(150, 30, 0));
            positions.Add(new Vector3(150, -30, 0));
        } else if (answers.Count() == 5) {
            positions.Add(new Vector3(-225, 30, 0));
            positions.Add(new Vector3(-225, -30, 0));
            positions.Add(new Vector3(0, 0, 0));
            positions.Add(new Vector3(225, 30, 0));
            positions.Add(new Vector3(225, -30, 0));
        } else if (answers.Count() == 6) {
            positions.Add(new Vector3(-225, 30, 0));
            positions.Add(new Vector3(-225, -30, 0));
            positions.Add(new Vector3(0, 30, 0));
            positions.Add(new Vector3(0, -30, 0));
            positions.Add(new Vector3(225, 30, 0));
            positions.Add(new Vector3(225, -30, 0));
        }

        foreach (string answer in answers) {
            int r = Random.Range(0, positions.Count);
            GameObject button = Instantiate(buttonPrefab);
            button.tag = "UserInput";
            button.transform.SetParent(userInput.transform);
            button.transform.localPosition = positions[r];
            button.transform.localScale = Vector3.one;

            button.GetComponentInChildren<Text>().text = answer;
            button.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(button, answers.First() == answer));
            positions.Remove(positions[r]);
        }

        answerButton = Instantiate(answerPrefab, userInput.transform);
        answerButton.transform.localPosition = new Vector3(0, 65f, 0);
        answerButton.GetComponent<Text>().text = _current.question;
        answerButton.tag = "UserInput";
        answerButton.SetActive(false);
    }

	void SelectAnswer(GameObject button, bool isCorrect) {
        Image image = button.GetComponent<Image>();
        Text text = button.GetComponentInChildren<Text>();

        // Disable all buttons after answer is selected
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (GameObject _button in buttonObjects) {
            _button.GetComponent<Button>().interactable = false;
        }

        // Show question
        answerButton.SetActive(true);

        if (isCorrect) {
            image.color = Scheme.Green;
			_gameShow.CorrectAnswer();
		} else {
            image.color = Scheme.Red;
            _gameShow.WrongAnswer();
		}
        text.color = Color.white;
    }
}
