using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	// Use this for initialization
	public GameObject currentRoad;
	public GameObject nextRoad;
	void Start () {
		StartCoroutine(DestroyRoad());
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	IEnumerator DestroyRoad(){
		while(true){
		currentRoad = Instantiate(nextRoad,currentRoad.transform.GetChild(0).GetChild(0).position,Quaternion.identity);
		yield return new WaitForSeconds(3f);
		}
		
	}
}
