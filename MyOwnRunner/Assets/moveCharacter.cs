using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCharacter : MonoBehaviour {

	// Use this for initialization
	public KeyCode moveL;
	public KeyCode moveR;
	public KeyCode jump;
	public float horizontalVel = 0;
	public int laneNum = 2;
	public string controlLocked = "n";
	Rigidbody rb;
	CharacterController pennyController;
	float playerYPos;
	bool isGrounded;
	public GameObject cam;
	Animator animamtor;
	Vector3 pos;
	void Start () {
		isGrounded = true;
		animamtor = GetComponent<Animator>();
		pos.y = transform.position.y;
		print(pos);
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().velocity = new 	Vector3(horizontalVel,Gm.vertVel,4);
		if(Input.GetKeyDown(moveL) && (laneNum>1)&&(controlLocked=="n")){
			print("left");
			horizontalVel = -2;
			StartCoroutine(stopSlide());
			laneNum -=1;
			controlLocked = "y";
		}
		if(Input.GetKeyDown(moveR)&&(laneNum<3)&&(controlLocked=="n")){
			print("right");
			horizontalVel = 2;
			StartCoroutine(stopSlide());
			laneNum+=1;
			controlLocked = "y";
		}
		if(Input.GetKeyDown(jump)&&(isGrounded==true)&&(controlLocked=="n")){
			playerYPos = transform.position.y;
			animamtor.Play("Jump");
			//Gm.vertVel = 3;
			rb.AddForce(transform.up*10,ForceMode.Impulse);
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
			//Destroy(gameObject);
			//GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			animamtor.Play("Death");
			Destroy(GetComponent<Rigidbody>());
			cam.GetComponent<CarFollow>().enabled = false;
			GetComponent<moveCharacter>().enabled = false;
			
			
		}
		
	}
	void OnCollisionStay(){
		print("colliders working");
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Collectible"){
			Destroy(other.gameObject);
		}
	}
}
