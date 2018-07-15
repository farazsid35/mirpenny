using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundControl : MonoBehaviour {

	// Use this for initialization
	public GameObject source;
	public GameObject soundOnButton;
	public GameObject soundOffButton;
	//public GameObject music;
	void Start () {
		if(SoundController.soundOn==false){
			source.SetActive(false);
			soundOffButton.SetActive(true);
			soundOnButton.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnSoundOnClick(){
		soundOnButton.SetActive(false);
		soundOffButton.SetActive(true);
		source.SetActive(false);
		SoundController.soundOn = false;
	}
	public void OnSoundOffClick(){
		soundOnButton.SetActive(true);
		soundOffButton.SetActive(false);
		source.SetActive(true);
		SoundController.soundOn = true;
	}
}
