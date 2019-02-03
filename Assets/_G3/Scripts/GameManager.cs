using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
            
    private int localCategory;
    private bool timeUp = false;
    private int strikeCount = 0;
    private int correctAnswers = 0;
    private float timeLeft = 10.0f;

    private Questions questions = new Questions();

    private TrueFalse currentTF;
    private static List<TrueFalse> unansweredTF;

    private WordScramble currentWS;
    private static List<WordScramble> unansweredWS;

    private FillBlank currentFB;
    private static List<FillBlank> unansweredFB;
    private GameObject[] uiInputs;

    [SerializeField] private float timeBetweenQuestions = 1.0f;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text categoryText;
    [SerializeField] private Text questionText;
    
    [SerializeField] private Text timerText;
    [SerializeField] private Text strikeText;   
    
    [SerializeField] private Text wrongText;    
    [SerializeField] private Text correctText;
    [SerializeField] private Text timesupText;

    private void Start () {
        inputField.onEndEdit.AddListener(SubmitText);
        strikeText.text = "";

        // Load True False Questions
        if (unansweredTF == null || unansweredTF.Count == 0) {
            unansweredTF = questions.TFQuestions.ToList<TrueFalse>();
        }

        // Word Scramble               
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

    private void Update() {
        timeLeft -= Time.deltaTime;        
        if ( timeLeft < 0 && timeUp == false ) {
            timeUp = true;
            TimesUp();
            timeLeft = 0;
        }
        timerText.text = "Time Left: " + Mathf.Round(timeLeft).ToString();
    }

    void SetCurrentQuestion() {
        timeUp = false;
        timeLeft = 10.0f;
        List<int> categoryList = new List<int>();

        // TODO: 3 strikes you're out

        if (unansweredFB.Count > 0) {
            categoryList.Add(0);            
        }

        if (unansweredTF.Count > 0) {
            categoryList.Add(1);
        }

        if (unansweredWS.Count > 0) {
            categoryList.Add(2);
        }

        localCategory = categoryList[Random.Range(0,categoryList.Count)];

        // TODO: Out of questions! -- Game Over

        if (localCategory == 1) {
            TrueorFalse();
        } else if (localCategory == 2) {
            WordScramble();
        } else if (localCategory == 0) {
            FillInTheBlank();
        }        
    }

    private void TimesUp() {
        Debug.Log("Times Up");

        HideInputs();
        timesupText.gameObject.SetActive(true);    
        StartCoroutine(NextQuestion());        
    }

    // TODO: Correct answer sound    
    private void CorrectAnswer () {
        Debug.Log("Correct");
        HideInputs();
        correctText.gameObject.SetActive(true);

        correctAnswers++;        
        StartCoroutine(NextQuestion());
    }

    // TODO: Wrong answer sound
    private void WrongAnswer () {
        Debug.Log("Wrong!");
        HideInputs();
        wrongText.gameObject.SetActive(true);
        strikeCount++;

        string strikes = "";
        for (int i = 0; i < strikeCount; i++) {
            strikes += " [X] ";
        }
        strikeText.text = strikes;
        StartCoroutine(NextQuestion());
    }

    public void TFSelect (bool answer) {
        if (currentTF.answer && answer) CorrectAnswer();
        else if (!currentTF.answer && !answer) CorrectAnswer();
        else WrongAnswer();
    }

    public void SubmitText (string guess) {
        if (localCategory == 2) {
            if (guess == currentWS.word) CorrectAnswer();
            else WrongAnswer();
        } else if (localCategory == 0) {
            if (guess == currentFB.answer) CorrectAnswer();
            else WrongAnswer();
        }

        if (inputField.text != "") inputField.text = "";
    }

    private void HideInputs () {
        foreach (var input in uiInputs) {
            input.SetActive(false);
        }

        wrongText.gameObject.SetActive(false);
        correctText.gameObject.SetActive(false);
        timesupText.gameObject.SetActive(false);
    }

    private void ShowInput (string GameObjectName) {
        foreach (GameObject go in uiInputs) {
            if (go.name == GameObjectName) {
                go.SetActive(true);
            }
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

    private string ScrambleWord (string word) {
        int index = 0;
        char[] chars = new char[word.Length];
        System.Random rand = new System.Random(10000);        
        
        while (word.Length > 0) {
            int next = rand.Next(0, word.Length - 1);
            chars[index] = word[next];
            word = word.Substring(0, next) + word.Substring(next + 1);
            ++index;
        }
        return new string(chars);
    }

    IEnumerator NextQuestion() {       
        yield return new WaitForSeconds(timeBetweenQuestions);
        HideInputs(); 
        SetCurrentQuestion();           
    }
}
