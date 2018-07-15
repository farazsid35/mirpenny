using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPlayClick(){
		SceneManager.LoadScene(1);
	}
	public void OnRestartClick(){
		Scene loadLevel = SceneManager.GetActiveScene();
		SceneManager.LoadScene(loadLevel.buildIndex);
	}

	public void OnHomeClick(){
		SceneManager.LoadScene(0);
	}
}
