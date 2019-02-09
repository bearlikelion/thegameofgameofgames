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

	// Use this for initialization
	void Start () {
		if (guid.ToString() == "") {
			guid = System.Guid.NewGuid();
			Debug.Log(guid.ToString());
		}
	}

	public void GameOver() {
        Debug.Log("Game Over!");
        if (strikes < 3) {
            Debug.Log("Ran out of questions");
        }

		SceneManager.LoadScene("HighScore");
    }

}
