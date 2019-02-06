using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    bool timeUp = false;
    int localCategory;    
    float timeLeft = 10.0f;
    float timeBetweenQuestions = 1.5f;

    public int strikeCount = 0;
    public int correctAnswers = 0;    
    public AudioClip timerClip, wrongClip, correctClip, timeupClip;

    private AudioSource audioSource;
    private Questions questions = new Questions();

    private FillBlank currentFB;
    private TrueFalse currentTF;
    private WordScramble currentWS;
    private static List<FillBlank> unansweredFB;
    private static List<TrueFalse> unansweredTF;
    private static List<WordScramble> unansweredWS;

    private GameObject[] uiInputs;

    [SerializeField] private GameObject strikeGO;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text categoryText;
    [SerializeField] private Text questionText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text strikeText;
    [SerializeField] private Text correctText;
    [SerializeField] private Text timesupText;

    private void Start () {
        inputField.onEndEdit.AddListener(SubmitText);
        strikeText.text = "";

        if (unansweredTF == null || unansweredTF.Count == 0) unansweredTF = questions.TFQuestions.ToList<TrueFalse>(); // True False
        if (unansweredWS == null || unansweredWS.Count == 0) unansweredWS = questions.WSWords.ToList<WordScramble>(); // Word Scramble
        if (unansweredFB == null || unansweredFB.Count == 0) unansweredFB = questions.FBlank.ToList<FillBlank>(); // Fill in the blank

        uiInputs = GameObject.FindGameObjectsWithTag("UserInput");
        audioSource = GetComponent<AudioSource>();

        HideInputs();        
        SetCurrentQuestion();
    }

    private void Update () {
        if (timerText != null) {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0) {
                timerText.text = Mathf.Round(timeLeft).ToString();
            } else if (timeLeft < 0 && timeUp == false) {
                timerText.text = "0";
                TimesUp();
            }
        }

        if (localCategory == 1) {
            if (Input.GetKeyDown(KeyCode.F)) TFSelect(false);
            if (Input.GetKeyDown(KeyCode.T)) TFSelect(true);            
        }        
    }

    public void StopSound() {
        // STOP THIS DAMN TICKING NOISE
        audioSource.Stop();
    }

    void GameOver() {
        SceneManager.LoadScene("Credits");
    }

    void SetCurrentQuestion () {
        timeUp = false;
        timeLeft = 10.0f;
        List<int> categoryList = new List<int>();

        if (strikeCount >= 3) {
            audioSource.Stop();
            GameOver();
        }

        if (unansweredFB != null && unansweredFB.Count > 0) categoryList.Add(0);
        if (unansweredTF != null && unansweredTF.Count > 0) categoryList.Add(1);
        if (unansweredWS != null && unansweredWS.Count > 0) categoryList.Add(2);
                    
        if (categoryList.Count > 0) {
            categoryText.text = "";
            questionText.text = "";
            audioSource.PlayOneShot(timerClip, 0.25f);

            localCategory = categoryList[Random.Range(0, categoryList.Count)];
            if (localCategory == 0) FillInTheBlank();
            else if (localCategory == 1) TrueorFalse();
            else if (localCategory == 2) WordScramble();
        } else {
            audioSource.Stop();
            GameOver();
        }
    }

    // Times up
    private void TimesUp () {
        audioSource.Stop();
        audioSource.PlayOneShot(timeupClip, 0.25f);

        HideInputs();
        timeUp = true;        
        timesupText.transform.DOScale(1, 1);
        StartCoroutine(NextQuestion());
    }

    // Correct answer   
    private void CorrectAnswer () {   
        audioSource.Stop();
        audioSource.PlayOneShot(correctClip, 0.25f);

        HideInputs();
        correctAnswers++;        
        correctText.transform.DOScale(1, 1);
        StartCoroutine(NextQuestion());
    }

    // Wrong answer
    private void WrongAnswer () {       
        audioSource.Stop();
        audioSource.PlayOneShot(wrongClip, 0.25f);

        HideInputs();
        strikeCount++;            

        string strikes = "";
        for (int i = 0; i < strikeCount; i++) {
            strikes += " [X] ";
        }

        strikeText.text = strikes;        
        strikeGO.transform.localPosition = Vector3.zero;
        strikeGO.transform.DOScale(1, 1).OnComplete(moveStrikes);               
    }

    void moveStrikes() {
        if (strikeCount < 3) {
            strikeGO.transform.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 1);
            strikeGO.transform.DOLocalMove(new Vector3(290f, 215f, 0f), 1);
        }
        StartCoroutine(NextQuestion());
    }

    void TrueorFalse () {
        ShowInput("TrueorFalse");

        int randomTFIndex = Random.Range(0, unansweredTF.Count);
        currentTF = unansweredTF[randomTFIndex];
        unansweredTF.Remove(currentTF);

        categoryText.text = currentTF.Category;
        questionText.DOText(currentTF.question, 1);
    }

    void WordScramble () {
        ShowInput("TextInput");
        inputField.Select();
        inputField.ActivateInputField();

        int randomWSIndex = Random.Range(0, unansweredWS.Count);
        currentWS = unansweredWS[randomWSIndex];
        unansweredWS.Remove(currentWS);

        string scrambledText = ScrambleWord(currentWS.word);

        categoryText.text = currentWS.Category;
        questionText.DOText(scrambledText, 1);
    }

    void FillInTheBlank () {
        ShowInput("TextInput");
        inputField.Select();
        inputField.ActivateInputField();

        int randomFBIndex = Random.Range(0, unansweredFB.Count);
        currentFB = unansweredFB[randomFBIndex];
        unansweredFB.Remove(currentFB);

        categoryText.text = currentFB.Category;
        questionText.DOText(currentFB.question, 1);
    }

    public void TFSelect (bool answer) {
        if (currentTF.answer && answer) {
            CorrectAnswer();
        } else if (!currentTF.answer && !answer) {
            CorrectAnswer();
        } else {
            WrongAnswer();
        }
    }

    public void SubmitText (string guess) {        
        if (guess != "") {
            guess = guess.ToLower();

            if (localCategory == 2) {
                if (guess == currentWS.word) CorrectAnswer();
                else WrongAnswer();

                questionText.DOText(currentWS.word, 1);
            } else if (localCategory == 0) {
                if (guess == currentFB.answer) CorrectAnswer();
                else WrongAnswer();
                string replace = "____";                
                questionText.DOText(currentFB.question.Replace(replace, "<i>"+currentFB.answer+"</i>"), 1);
            }
            inputField.text = "";
        }
    }

    private void HideInputs () {
        foreach (var input in uiInputs) {
            input.SetActive(false);
        }

        correctText.transform.localScale = Vector3.zero;
        timesupText.transform.localScale = Vector3.zero;
    }

    private void ShowInput (string GameObjectName) {
        foreach (GameObject go in uiInputs) {
            if (go.name == GameObjectName) {
                go.SetActive(true);
                go.transform.DOScale(1, 1);                                
            }
        }
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

    IEnumerator NextQuestion () {
        yield return new WaitForSeconds(timeBetweenQuestions);
        HideInputs();
        SetCurrentQuestion();
    }
}
