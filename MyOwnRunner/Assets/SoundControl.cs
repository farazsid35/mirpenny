using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundControl : MonoBehaviour {

	// Use this for initialization
	public GameObject soundOn;
	public GameObject soundOff;
	public GameObject music;
	bool isSound;
	void Awake(){
		
	}
	void Start () {
		// isSound = true;
		print(ScoreManager.soundOn);
		if(SoundController.soundOn == false){
			music.SetActive(false);
			soundOn.SetActive(false);
			soundOff.SetActive(true);
		}
		//ScoreManager.soundOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnSoundOnClick(){
		soundOn.SetActive(false);
		soundOff.SetActive(true);
		music.SetActive(false);
		SoundController.soundOn = false;
	}
	public void OnSoundOffClick(){
		soundOn.SetActive(true);
		soundOff.SetActive(false);
		music.SetActive(true);
		SoundController.soundOn = true;
	}
}
