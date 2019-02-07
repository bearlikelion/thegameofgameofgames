using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameShow : MonoBehaviour {

	[SerializeField]
	private Text categoryText;
	
	[SerializeField]
	private Text questionText;

	// Use this for initialization
	void Start () {		
	}

	public void SetCategory(string text) {
		categoryText.text = text;
	}
	
	public void SetQuestion(string text) {
		questionText.text = text;
	}
}
