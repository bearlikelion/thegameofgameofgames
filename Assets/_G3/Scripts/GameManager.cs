using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool isGameOver = false;
    public string playerName = "";
    public int correct = 0, strikes = 0, speed = 0;
    private string guid = "";

    public string Guid {
        get { return guid; }
    }
    

    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        if (guid == "") {
            GenerateGUID();
        }
    }

    public void ShowScores() {
        Debug.Log("Go to: Leaderboard"); // BRAVE CAPTAIN NUTCRACKER
        SceneManager.LoadScene("HighScore");
    }

    public void GameOver () {
        Debug.Log("Game Over!");
        isGameOver = true;
        SceneManager.LoadScene("Menu");
        // SceneManager.LoadScene("HighScore");
    }

    void GenerateGUID () {
        guid = System.Convert.ToBase64String(System.Guid.NewGuid().ToByteArray()); // Short GUID
        Debug.Log(guid);
    }
}
