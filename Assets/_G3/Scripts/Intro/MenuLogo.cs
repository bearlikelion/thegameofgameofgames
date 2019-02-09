using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogo : MonoBehaviour {

    public RectTransform logoRect;
    public RectTransform startRect;

    float speed = 200f;
    bool outlineCB = false;
    Outline outline;

    // Use this for initialization
    void Start () {
        startRect.DOLocalMoveY(-200, 2).From();
        logoRect.DOLocalMoveY(325, 2).From().OnComplete(SpreadOutline);
        outline = logoRect.gameObject.GetComponent<Outline>();
    }

	// Update is called once per frame
	void Update () {
        if (outlineCB == true && outline.effectDistance.x < 200) {
            outline.effectDistance = new Vector2(Mathf.MoveTowards(outline.effectDistance.x, 200, Time.deltaTime * speed), 0);
        }
    }

    void SpreadOutline() {
        outlineCB = true;
    }
}
