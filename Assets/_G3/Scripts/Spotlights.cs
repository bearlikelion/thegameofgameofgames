using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlights : MonoBehaviour {

    public GameObject redLight, blueLight, yellowLight, ellen;
    private IntroControl _intro;    

    // Use this for initialization
    void Start () {
        _intro = GameObject.Find("--- IntroControl ---").GetComponent<IntroControl>();


        // blueLight.transform.DOLocalRotate(new Vector3(0, 0, 15f), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();		
        // redLight.transform.DOLocalRotate(new Vector3(0, 0, 30), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
        // yellowLight.transform.DOLocalRotate(new Vector3(0, 0, -30f), 25f).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
    }

    // Update is called once per frame
    void Update () {
        if (_intro.showStarted) {
            blueLight.transform.Find("BlueSpot").LookAt(ellen.transform);
        } else {
            float blueAngle = Mathf.PingPong(Time.time * 7.5f, 30) - 15;
            blueLight.transform.rotation = Quaternion.Euler(0, 0, blueAngle);

            float yellowAngle = Mathf.PingPong(Time.time * 15f, 30);
            yellowLight.transform.rotation = Quaternion.Euler(0, 0, -yellowAngle);

            float redAngle = Mathf.PingPong(Time.time * 15f, 30);
            redLight.transform.rotation = Quaternion.Euler(0, 0, redAngle);
        }        
    }
}
