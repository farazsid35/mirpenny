using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
public class PennyMoveFinal : MonoBehaviour {

	// Use this for initialization
	 public Text highScore;
	 public int tempScore;
	 public float horizontalVel = 0;
	public int laneNum = 2;
    public Text currentScore;
	public string controlLocked = "n";
    public Text scorePanel;
	Rigidbody rb;
	CharacterController pennyController;
	float playerYPos;
	bool isGrounded;
	public GameObject cam;
    public GameObject gameOverPanel;
    public AudioSource coinSound;
	Animator animamtor;
	Vector3 pos;

	public Swipe swipeControls;
	void Start () {
		isGrounded = true;
		animamtor = GetComponent<Animator>();
		pos.y = transform.position.y;
		print(pos);
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().velocity = new 	Vector3(horizontalVel,Gm.vertVel,6);
		if((swipeControls.SwipeLeft)&& (laneNum>1)&&(controlLocked=="n")){
			print("Left");
			horizontalVel = -2;
			StartCoroutine(stopSlide());
			laneNum -=1;
			controlLocked = "y";
		}
		if((swipeControls.SwipeRight)&&(laneNum<3)&&(controlLocked=="n")){
			print("Right");
			horizontalVel = 2;
            //horizontalVel = 4;
			StartCoroutine(stopSlide());
			laneNum+=1;
			controlLocked = "y";
		}
		if((swipeControls.SwipeUp)&&(isGrounded==true)&&(controlLocked=="n")){
			print("Up");
			playerYPos = transform.position.y;
			animamtor.Play("Jump");
			//Gm.vertVel = 3;
			rb.AddForce(transform.up*30,ForceMode.Impulse);
			isGrounded = false;
			controlLocked = "y";
		}
		if(isGrounded == false){
			StartCoroutine(StopJump());
		}
	}
	IEnumerator stopSlide(){
		yield return new WaitForSeconds(0.5f);
		horizontalVel = 0;
		//Gm.vertVel = 0;
		controlLocked = "n";
	}
	IEnumerator StopJump(){
		yield return new WaitForSeconds(0.7f);
		Gm.vertVel = 0;
		transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x,0.51f,transform.position.z),Time.smoothDeltaTime);
		controlLocked = "n";
		isGrounded = true;
		print(Gm.vertVel);
		print("Jump Stopped");
	}
    void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Obstacle"){
            gameOverPanel.SetActive(true);
            currentScore.text = ScoreManager.coins.ToString();
            if(ScoreManager.coins>ScoreManager.highScore){
                tempScore = ScoreManager.coins;
            }
            else{
                tempScore = ScoreManager.highScore;
            }
            Player player = new Player{
                currentScore = ScoreManager.coins,
                highScore = tempScore
            };
            ResourceHelper.SaveToPersistentData("player.json",player);
            highScore.text = player.highScore.ToString();
			//Destroy(gameObject);
			//GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			animamtor.Play("Death");
			Destroy(GetComponent<Rigidbody>());
			cam.GetComponent<CarFollow>().enabled = false;
			GetComponent<moveCharacter>().enabled = false;
			gameOverPanel.SetActive(true);
			
		}
        if(other.gameObject.tag == "Pit"){
            // Destroy(GetComponent<Rigidbody>());
            gameOverPanel.SetActive(true);
            currentScore.text = ScoreManager.coins.ToString();
            if(ScoreManager.coins>ScoreManager.highScore){
                tempScore = ScoreManager.coins;
            }
            else{
                tempScore = ScoreManager.highScore;
            }
            Player player = new Player{
                currentScore = ScoreManager.coins,
                highScore = tempScore
            };
            ResourceHelper.SaveToPersistentData("player.json",player);
            highScore.text = player.highScore.ToString();
            Destroy(gameObject);
			cam.GetComponent<CarFollow>().enabled = false;
            
			//GetComponent<moveCharacter>().enabled = false;
           
        }
		
	}
	void OnCollisionStay(){
		print("colliders working");
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Collectible"){
            
            scorePanel.text = ScoreManager.coins.ToString();
            ScoreManager.coins +=1;
            coinSound.Play();
			Destroy(other.gameObject);
		}
        if(other.gameObject.tag=="RoadDestroyer"){
            StartCoroutine(DestroyRoad(other.gameObject.transform.parent.gameObject));
        }
	}
    IEnumerator DestroyRoad(GameObject game){
        yield return new WaitForSeconds(1.0f);
        Destroy(game);
    }
}
