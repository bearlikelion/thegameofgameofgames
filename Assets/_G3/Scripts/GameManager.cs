using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool isGameOver = false;
    public string playerName = "";
    public int correct = 0, strikes = 0, speed = 0;
    private string guid;

    public string Guid {
        get { return guid; }
    }

    void Awake () {
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        GenerateGUID();
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
