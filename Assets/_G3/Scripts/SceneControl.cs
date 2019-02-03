using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

	private void Start() {
		
	}

	public void BeginShow() {
		Debug.Log("Lets start the show!");
	}

	// TODO: Show Credits
	public void ShowCredits() {
		Debug.Log("Show credits");
	}

	public void StartQuestions() {
		SceneManager.LoadScene("Questions");
	}
}
