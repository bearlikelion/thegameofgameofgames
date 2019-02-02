using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    
    private Questions questions = new Questions();

    private TrueFalse currentTF;
    private static List<TrueFalse> unansweredTF;

    private WordScramble currentWS;
    private static List<WordScramble> unansweredWS;

    enum questionTypes {
        TrueFalse,
        WordScramble
    };

    private GameObject[] uiInputs;

    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text trueAnswerText;
    [SerializeField] private Text falseAnswerText;
    [SerializeField] private float timeBetweenQuestions = 1.0f;

    private void Start () {        
        // Load True False Questions
        if (unansweredTF == null || unansweredTF.Count == 0) {
            unansweredTF = questions.TFQuestions.ToList<TrueFalse>();
        }

        // Word Scramble        
        inputField.onEndEdit.AddListener(WSGuess);

        if (unansweredWS == null || unansweredWS.Count == 0) {
            unansweredWS = questions.WSWords.ToList<WordScramble>();
        }

        uiInputs = GameObject.FindGameObjectsWithTag("UserInput");

        HideInputs();
        SetCurrentQuestion();        
    }

    void SetCurrentQuestion() {
        var typeCount = questionTypes.GetNames(typeof(questionTypes)).Length;        
        var categoryIs = (questionTypes)Random.Range(0, typeCount);        

        if (categoryIs == questionTypes.TrueFalse) {
            TrueorFalse();
        } else if (categoryIs == questionTypes.WordScramble) {
            WordScramble();
        }
    }

    void TrueorFalse() {
        ShowInput("TrueorFalse");

        int randomTFIndex = Random.Range(0, unansweredTF.Count);
        currentTF = unansweredTF[randomTFIndex];
        unansweredTF.Remove(currentTF);

        categoryText.text = currentTF.Category;
        questionText.text = currentTF.question;
    }

    public void TFSelect (bool answer) {
        if (currentTF.answer && answer == true) { // if currentQuestion is True and answer is True            
            Debug.Log("Correct");
        } else if (!currentTF.answer && !answer) { // if currentQuestion is False and answer is False            
            Debug.Log("Correct");
        } else { // otherwise you're wrong            
            Debug.Log("Wrong!");
        }

        StartCoroutine(NextQuestion());
    }

    void WordScramble() {
        ShowInput("WordScramble");

        int randomWSIndex = Random.Range(0, unansweredWS.Count);
        currentWS = unansweredWS[randomWSIndex];
        unansweredWS.Remove(currentWS);

        string scrambledText = ScrambleWord(currentWS.word);

        categoryText.text = currentWS.Category;        
        questionText.text = scrambledText;
    }

    string ScrambleWord (string word) {
        char[] chars = new char[word.Length];
        System.Random rand = new System.Random(10000);        
        int index = 0;
        while (word.Length > 0) { // Get a random number between 0 and the length of the word. 
            int next = rand.Next(0, word.Length - 1); // Take the character from the random position 
                                                      //and add to our char array. 
            chars[index] = word[next];                // Remove the character from the word. 
            word = word.Substring(0, next) + word.Substring(next + 1);
            ++index;
        }        
        return new string(chars);
    }

    public void WSGuess(string guess) {            
        if (guess == currentWS.word) {
            Debug.Log("Correct!");

            inputField.text = "";
            StartCoroutine(NextQuestion());
        } else {
            Debug.Log("Wrong!");
        }
    }

    void ShowInput(string GameObjectName) {
        foreach (GameObject go in uiInputs) {            
            if (go.name == GameObjectName) {
                go.SetActive(true);
            }
        }
    }

    void HideInputs() {
        var Inputs = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (var input in Inputs) {
            input.SetActive(false);
        }
    }

    IEnumerator NextQuestion() {        
        yield return new WaitForSeconds(timeBetweenQuestions);

        HideInputs();        
        SetCurrentQuestion();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
