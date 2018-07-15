using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour {

	// Use this for initialization
	public Swipe swipeControls;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(swipeControls.SwipeLeft){
			print("Left");
		}
		if(swipeControls.SwipeRight){
			print("Right");
		}
		if(swipeControls.SwipeUp){
			print("Up");
		}
	}
}
