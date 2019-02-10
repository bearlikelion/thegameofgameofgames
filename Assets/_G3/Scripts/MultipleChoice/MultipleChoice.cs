using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoice : MonoBehaviour, Category {

	public GameObject buttonPrefab;

	private GameShow _gameShow;
	private GameObject userInput;
    private string _category = "Multiple Choice";

	private MCQuestions _current;
	private List<MCQuestions> unanswered;
    private System.Random rnd = new System.Random();

    [SerializeField]
    private MCQuestions[] _questions;

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
			unanswered = _questions.ToList<MCQuestions>();
		}
	}

    public void SetQuestion () {
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = _category;
		_gameShow.Question = _current.question;
		MakeAnswerButtons();
	}

	void MakeAnswerButtons() {
        List<string> answers = new List<string>();
        List<Vector3> positions = new List<Vector3>();
        string[] choices = _current.choices.OrderBy(x => rnd.Next()).ToArray();

        answers.Add(_current.answer);
        for (int i = 0; i < 3; i++) {
            answers.Add(choices[i]);
        }

        positions.Add(new Vector3(-150, 40, 0));
        positions.Add(new Vector3(-150, -40, 0));
        positions.Add(new Vector3(150, 40, 0));
        positions.Add(new Vector3(150, -40, 0));

        foreach (string answer in answers) {
            int r = Random.Range(0, positions.Count);
            GameObject button = Instantiate(buttonPrefab, userInput.transform);
            button.transform.localPosition = positions[r];
            button.tag = "UserInput";

            button.GetComponentInChildren<Text>().text = answer;
            button.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(button, answers.First() == answer));
            positions.Remove(positions[r]);
        }
    }

	void SelectAnswer(GameObject button, bool isCorrect) {
        Image image = button.GetComponent<Image>();
        Text text = button.GetComponentInChildren<Text>();

        // Disable all buttons after answer is selected
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (GameObject _button in buttonObjects) {
            _button.GetComponent<Button>().interactable = false;
        }

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
