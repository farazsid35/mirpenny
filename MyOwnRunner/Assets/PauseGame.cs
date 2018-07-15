using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseGame : MonoBehaviour {

	// Use this for initialization
	public GameObject PausePanel;
	bool isPaused;
	void Start () {
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPauseClick(){
		if(isPaused==false){
			Time.timeScale = 0;
			PausePanel.SetActive(true);
		}
		
	}
	public void OnResumeClick(){
		Time.timeScale = 1;
		PausePanel.SetActive(false);
	}
}
