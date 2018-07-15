using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLight : MonoBehaviour {

	// Use this for initialization
	public GameObject directionalLight;
	Light light;
	void Start () {
		light = directionalLight.GetComponent<Light>();
		light.color = Color.white;
		directionalLight.GetComponent<Transform>().rotation = (Quaternion.Euler(43,12,61));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
