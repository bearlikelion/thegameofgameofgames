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
        logoRect.DOLocalMoveY(48, 2).OnComplete(SpreadOutline);
        outline = logoRect.gameObject.GetComponent<Outline>();
        startRect.DOLocalMoveY(-98, 2);
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
