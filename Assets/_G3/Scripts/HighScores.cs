using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour {

    public GameObject user, score, highscores, textPrefab;    
    

    void Start () {              
        LoadScores();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SubmitScore() {
        int playerScore = 0;
        string playerName = user.GetComponent<InputField>().text;
        int.TryParse(score.GetComponent<InputField>().text, out playerScore);

        
        LoadScores();
    }

    public void LoadScores() {        
        foreach (Transform child in highscores.transform) {
            Destroy(child.gameObject);
        }

        // TODO: Show Scores                
    }
}
