using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public string playerName = "";
	public int correct = 0, strikes = 0, speed = 0;
	private System.Guid guid;

	public string Guid {
		get { return guid.ToString(); }
	}

    void Awake () {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start () {
        GenerateGUID();		
	}

	public void GameOver() {
        Debug.Log("Game Over!");
        if (strikes < 3) {
            Debug.Log("Ran out of questions");
        }

		SceneManager.LoadScene("HighScore");
    }

    void GenerateGUID() {
        guid = System.Guid.NewGuid();
        Debug.Log(guid.ToString());
    }

}
