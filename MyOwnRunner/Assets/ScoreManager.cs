using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ScoreManager : MonoBehaviour {
	public static int coins;
	public static int highScore;
	public static bool soundOn = true;
	Player player;
	// Use this for initialization
	void Start () {
		//soundOn = true;
		string js = ResourceHelper.ReadFromPersistentData("player.json");
		player = ResourceHelper.DecodeObject<Player>(js);
		highScore = player.highScore;
		coins = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Player{
	public int currentScore{get;set;}
	public int highScore{get;set;}
}
