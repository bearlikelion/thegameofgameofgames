using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    
    private int strikeCount = 0;
    private int correctAnswers = 0;

    private Questions questions = new Questions();

    private TrueFalse currentTF;
    private static List<TrueFalse> unansweredTF;

    private WordScramble currentWS;
    private static List<WordScramble> unansweredWS;

    private FillBlank currentFB;
    private static List<FillBlank> unansweredFB;

    enum categories {
        TrueFalse,
        WordScramble,
        FillBlank
    };

    private categories categoryIs;

    private GameObject[] uiInputs;

    [SerializeField] private InputField inputField;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI strikeText;
    // [SerializeField] private Text trueAnswerText;
    // [SerializeField] private Text falseAnswerText;
    [SerializeField] private float timeBetweenQuestions = 1.0f;

    private void Start () {        
        // Load True False Questions
        if (unansweredTF == null || unansweredTF.Count == 0) {
            unansweredTF = questions.TFQuestions.ToList<TrueFalse>();
        }

        // Word Scramble        
        inputField.onEndEdit.AddListener(SubmitText);

        if (unansweredWS == null || unansweredWS.Count == 0) {
            unansweredWS = questions.WSWords.ToList<WordScramble>();
        }

        // Fill in the blank
        if (unansweredFB == null || unansweredFB.Count == 0) {
            unansweredFB = questions.FBlank.ToList<FillBlank>();
        }

        uiInputs = GameObject.FindGameObjectsWithTag("UserInput");

        HideInputs();
        SetCurrentQuestion();        
    }

    void SetCurrentQuestion() {
        var typeCount = categories.GetNames(typeof(categories)).Length;        
        categoryIs = (categories)Random.Range(0, typeCount);        

        if (categoryIs == categories.TrueFalse) {
            TrueorFalse();
        } else if (categoryIs == categories.WordScramble) {
            WordScramble();
        } else if (categoryIs == categories.FillBlank) {
            FillInTheBlank();
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

    void WordScramble() {
        ShowInput("TextInput");

        int randomWSIndex = Random.Range(0, unansweredWS.Count);
        currentWS = unansweredWS[randomWSIndex];
        unansweredWS.Remove(currentWS);

        string scrambledText = ScrambleWord(currentWS.word);

        categoryText.text = currentWS.Category;        
        questionText.text = scrambledText;
    }

    void FillInTheBlank() {
        ShowInput("TextInput");

        int randomFBIndex = Random.Range(0, unansweredFB.Count);
        currentFB = unansweredFB[randomFBIndex];
        unansweredFB.Remove(currentFB);

        categoryText.text = currentFB.Category;        
        questionText.text = currentFB.question;
    }

    public void TFSelect (bool answer) {
        if (currentTF.answer && answer == true) { // if currentQuestion is True and answer is True            
            CorrectAnswer();
        } else if (!currentTF.answer && !answer) { // if currentQuestion is False and answer is False            
            CorrectAnswer();
        } else { // otherwise you're wrong            
            WrongAnswer();
        }

        StartCoroutine(NextQuestion());
    }

    public void SubmitText(string guess) {
        if (categoryIs == categories.WordScramble) {
            if (guess == currentWS.word) CorrectAnswer();
            else WrongAnswer();            
        } else if (categoryIs == categories.FillBlank) {
            if (guess == currentFB.answer) CorrectAnswer();
            else WrongAnswer();                            
        }
    }

    private void CorrectAnswer() {
        Debug.Log("Correct");
        correctAnswers++;        
        correctText.text = "Correct: " + correctAnswers.ToString();
        StartCoroutine(NextQuestion());
    }

    private void WrongAnswer() {
        Debug.Log("Wrong!");
        strikeCount++;

        string strikes = "";

        for (int i = 0; i < strikeCount; i++) {
            strikes += " [X] ";
        }

        strikeText.text = strikes;

        if (inputField.text != "") {
            inputField.text = "";
        }
    }

    private void ShowInput(string GameObjectName) {
        foreach (GameObject go in uiInputs) {            
            if (go.name == GameObjectName) {
                go.SetActive(true);
            }
        }
    }

    private string ScrambleWord (string word) {
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
    private void HideInputs() {
        var Inputs = GameObject.FindGameObjectsWithTag("UserInput");
        foreach (var input in Inputs) {
            input.SetActive(false);
        }
    }

    IEnumerator NextQuestion() {
        yield return new WaitForSeconds(timeBetweenQuestions);

        HideInputs();        
        SetCurrentQuestion();        
    }
}
