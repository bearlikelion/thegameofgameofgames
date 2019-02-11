using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrueorFalse : MonoBehaviour, Category {

	public GameObject buttonPrefab;

	private GameShow _gameShow;
	private GameObject userInput;
    private string _category = "True of False";

	private TFQuestions _current;
	private List<TFQuestions> unanswered;
    private System.Random rnd = new System.Random();

    [SerializeField]
    private TFQuestions[] _questions;

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
			unanswered = _questions.ToList<TFQuestions>();
		}
	}

    public void SetQuestion () {
		int r = Random.Range(0, unanswered.Count);
		_current = unanswered[r];
		unanswered.Remove(unanswered[r]); // Remove question from list

		_gameShow.Category = _category;
		_gameShow.Question = _current.question;
		MakeButtons();
	}

	void MakeButtons() {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(new Vector3(0, 40, 0));
        positions.Add(new Vector3(0, -40, 0));
        positions = positions.OrderBy(x => rnd.Next()).ToList();

        GameObject trueButton = Instantiate(buttonPrefab);
        trueButton.tag = "UserInput";
        trueButton.transform.SetParent(userInput.transform);
        trueButton.transform.localPosition = positions[0];
        trueButton.transform.localScale = Vector3.one;

        trueButton.GetComponentInChildren<Text>().text = "True";
        trueButton.GetComponentInChildren<Text>().color = Color.white;
        trueButton.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(true));

        GameObject falseButton = Instantiate(buttonPrefab);
        falseButton.tag = "UserInput";
        falseButton.transform.SetParent(userInput.transform);
        falseButton.transform.localPosition = positions[1];
        falseButton.transform.localScale = Vector3.one;

        falseButton.GetComponentInChildren<Text>().text = "False";
        falseButton.GetComponentInChildren<Text>().color = Color.white;
        falseButton.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(false));

        // Random Swap Chance
        if (Random.Range(0f, 100f) < 10) {
            trueButton.GetComponent<Image>().color = Scheme.Red;
            falseButton.GetComponent<Image>().color = Scheme.Blue;
        } else {
            trueButton.GetComponent<Image>().color = Scheme.Blue;
            falseButton.GetComponent<Image>().color = Scheme.Red;
        }
    }

	void SelectAnswer(bool answer) {
        // Disable all buttons after answer is selected
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (GameObject _button in buttonObjects) {
            _button.GetComponent<Button>().interactable = false;
        }

        if (answer && _current.isTrue) {
            _gameShow.CorrectAnswer();
        } else if (!answer && !_current.isTrue) {
            _gameShow.CorrectAnswer();
        } else {
            _gameShow.WrongAnswer();
        }
    }

}
