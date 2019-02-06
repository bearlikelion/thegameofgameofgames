using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlights : MonoBehaviour {

	public GameObject redLight, blueLight, yellowLight;

	// Use this for initialization
	void Start () {
		blueLight.transform.DOLocalRotate(new Vector3(0, 0, 15f), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();		
		redLight.transform.DOLocalRotate(new Vector3(0, 0, 30), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
		yellowLight.transform.DOLocalRotate(new Vector3(0, 0, -30f), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
	}
	
	// Update is called once per frame
	void Update () {
		// blueLight.transform.rotation = Quaternion.Euler(0f, 0f, 15f * Mathf.Sin(Time.time * 2f));
	}
}
