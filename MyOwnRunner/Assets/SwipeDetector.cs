using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
public class SwipeDetector : MonoBehaviour {

    private const int mMessageWidth  = 200;
    private const int mMessageHeight = 64;
    public Text highScore;
    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);
	
    
	
    
    
    
    
    private readonly string [] mMessage = {
        "",
        "Swipe Left",
        "Swipe Right",
        "Swipe Top",
        "Swipe Bottom"
    };

    private int mMessageIndex = 0;
    public int tempScore;

    // The angle range for detecting swipe
    private const float mAngleRange = 30;

    // To recognize as swipe user should at lease swipe for this many pixels
    private const float mMinSwipeDist = 50.0f;

    // To recognize as a swipe the velocity of the swipe
    // should be at least mMinVelocity
    // Reduce or increase to control the swipe speed
    private const float mMinVelocity  = 2000.0f;

    private Vector2 mStartPosition;
    private float mSwipeStartTime;
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

    // Use this for initialization
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
        // Mouse button down, possible chance for a swipe
        if (Input.GetMouseButtonDown(0)) {
            // Record start time and position
            mStartPosition = new Vector2(Input.mousePosition.x,
                                         Input.mousePosition.y);
            mSwipeStartTime = Time.time;
        }

        // Mouse button up, possible chance for a swipe
        if (Input.GetMouseButtonUp(0)) {
            float deltaTime = Time.time - mSwipeStartTime;

            Vector2 endPosition  = new Vector2(Input.mousePosition.x,
                                               Input.mousePosition.y);
            Vector2 swipeVector = endPosition - mStartPosition;

            float velocity = swipeVector.magnitude/deltaTime;

            if (velocity > mMinVelocity &&
                swipeVector.magnitude > mMinSwipeDist) {
                // if the swipe has enough velocity and enough distance

                swipeVector.Normalize();

                float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                // Detect left and right swipe
                if ((angleOfSwipe < mAngleRange)&&(laneNum<3)&&(controlLocked=="n")) {
                    print("left");
			        horizontalVel = 2;
                    //horizontalVel = 4;
			        StartCoroutine(stopSlide());
			        laneNum+=1;
			        controlLocked = "y";
                    OnSwipeRight();
                } else if (((180.0f - angleOfSwipe) < mAngleRange)&& (laneNum>1)&&(controlLocked=="n")) {
                    print("left");
			        horizontalVel = -2;
			        StartCoroutine(stopSlide());
			        laneNum -=1;
			        controlLocked = "y";
                    OnSwipeLeft();
                } else {
                    // Detect top and bottom swipe
                    angleOfSwipe = Vector2.Dot(swipeVector, mYAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                    if ((angleOfSwipe < mAngleRange)&&(isGrounded==true)&&(controlLocked=="n")) {
                        playerYPos = transform.position.y;
			            animamtor.Play("Jump");
			            //Gm.vertVel = 3;
			            rb.AddForce(transform.up*30,ForceMode.Impulse);
			            isGrounded = false;
			            controlLocked = "y";
                        OnSwipeTop();
                    } else if ((180.0f - angleOfSwipe) < mAngleRange) {
                        OnSwipeBottom();
                    } else {
                        mMessageIndex = 0;
                    }
                }
            }
        }
        if(isGrounded == false){
			StartCoroutine(StopJump());
		}
        // if(transform.position.x>=1.0f){
        //     transform.position = new Vector3(1.0f,transform.position.y,transform.position.z);
        // }
        // if(transform.position.x<=-1.0f){
        //     transform.position = new Vector3(-1.0f,transform.position.y,transform.position.z);
        // }
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
    // void OnGUI() {
    //     // Display the appropriate message
    //     GUI.Label(new Rect((Screen.width-mMessageWidth)/2,
    //                        (Screen.height-mMessageHeight)/2,
    //                         mMessageWidth, mMessageHeight),
    //               mMessage[mMessageIndex]);
    // }

    private void OnSwipeLeft() {
        mMessageIndex = 1;
		print("Left");
		
    }

    private void OnSwipeRight() {
        mMessageIndex = 2;
		print("Right");
        
    }

    private void OnSwipeTop() {
        mMessageIndex = 3;
        print("Top");
        // if(isGrounded == true){
            
        // }
        
            
            //print(isGrounded);
        //pennyBharwa.transform.Translate(pennyBharwa.transform.up*pennyBharwa.GetComponent<PennyMovement>().pennyChutyeKiSpeed*5f);

    }

    private void OnSwipeBottom() {
        mMessageIndex = 4;
    }
}