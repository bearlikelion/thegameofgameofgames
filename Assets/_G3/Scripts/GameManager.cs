using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public System.Guid guid;
	public string playerName;
	public int correct, strikes, speed;

	// Use this for initialization
	void Start () {
		guid = System.Guid.NewGuid();
		Debug.Log(guid.ToString());
	}

	public void GameOver() {
        Debug.Log("Game Over!");
        if (strikes < 3) {
            Debug.Log("Ran out of questions");
        }
        Debug.Log("Player Speed: " + speed);
    }

}
