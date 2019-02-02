using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    enum colors {
        black,
        blue,
        cyan,
        gray,
        green,
        grey,
        magenta,
        red,        
        yellow
    }    

    private categories categoryIs;
    private GameObject[] uiInputs;

    [SerializeField] private float timeBetweenQuestions = 0f;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text categoryText;
    [SerializeField] private Text questionText;
    [SerializeField] private Text correctText;
    [SerializeField] private Text strikeText;   

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

    void SetCurrentQuestion() {
        var typeCount = categories.GetNames(typeof(categories)).Length;
        var backgrounds = colors.GetNames(typeof(colors)).Length;

        categoryIs = (categories)Random.Range(0, typeCount);
        var bgColor = (colors)Random.Range(0, backgrounds);

        // TODO: Fix background colors to shades of purple, add background textures
        switch (bgColor) {
            case colors.black:
                Camera.main.backgroundColor = Color.black;
                break;
            case colors.blue:
                Camera.main.backgroundColor = Color.blue;
                break;
            case colors.cyan:
                Camera.main.backgroundColor = Color.cyan;
                break;
            case colors.gray:
                Camera.main.backgroundColor = Color.gray;
                break;
            case colors.green:
                Camera.main.backgroundColor = Color.green;
                break;
            case colors.grey:
                Camera.main.backgroundColor = Color.grey;
                break;
            case colors.magenta:
                Camera.main.backgroundColor = Color.magenta;
                break;
            case colors.red:
                Camera.main.backgroundColor = Color.red;
                break;
            case colors.yellow:
                Camera.main.backgroundColor = Color.yellow;
                break;
        }

        if (categoryIs == categories.TrueFalse) {
            TrueorFalse();
        } else if (categoryIs == categories.WordScramble) {
            WordScramble();
        } else if (categoryIs == categories.FillBlank) {
            FillInTheBlank();
        }        
    }

    // TODO: Correct answer sound
    // TODO: Correct answer text
    private void CorrectAnswer () {
        Debug.Log("Correct");
        correctAnswers++;
        correctText.text = "Correct: " + correctAnswers.ToString();

        StartCoroutine(NextQuestion());
    }

    // TODO: Wrong answer sound
    // TODO: Wrong answer text
    private void WrongAnswer () {
        Debug.Log("Wrong!");
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
        if (categoryIs == categories.WordScramble) {
            if (guess == currentWS.word) CorrectAnswer();
            else WrongAnswer();
        } else if (categoryIs == categories.FillBlank) {
            if (guess == currentFB.answer) CorrectAnswer();
            else WrongAnswer();
        }

        if (inputField.text != "") inputField.text = "";
    }

    private void HideInputs () {
        foreach (var input in uiInputs) {
            input.SetActive(false);
        }
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
        HideInputs();        
        SetCurrentQuestion();

        // yield return new WaitForSeconds(timeBetweenQuestions);
        yield break;
    }
}
