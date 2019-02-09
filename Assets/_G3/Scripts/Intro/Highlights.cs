using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlights : MonoBehaviour {

	public GameObject redLight, blueLight, yellowLight, ellen;
	private IntroControl _intro;

	// Use this for initialization
	void Start () {
		_intro = GameObject.Find("--- IntroControl ---").GetComponent<IntroControl>();
	}

	// Update is called once per frame
	void Update () {
		if (_intro.showStarted) {
			redLight.transform.LookAt(ellen.transform);
			blueLight.transform.LookAt(ellen.transform);
			yellowLight.transform.LookAt(ellen.transform);
		}
	}
}
