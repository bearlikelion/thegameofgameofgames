using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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

    public void GameOver () {
        Debug.Log("Game Over!");
        if (strikes < 3) {
            Debug.Log("Ran out of questions");
        }

        SceneManager.LoadScene("HighScore");
    }

    void GenerateGUID () {
        guid = System.Convert.ToBase64String(System.Guid.NewGuid().ToByteArray()); // Short GUID        
        Debug.Log(guid);
    }
}
