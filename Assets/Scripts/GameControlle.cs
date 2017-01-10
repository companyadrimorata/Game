using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlle : MonoBehaviour {

	private bool dead;

	public Image greenKey_canvas;
	public Image redKey_canvas;
	public Image yellowKey_canvas;

	public int gems = 0;
	public Text gemsText;

	public Image lives1;
	public Image lives2;
	public Image lives3;
	public Image lives4;
	public Image lives5;
	public bool liveTaken = false;
	public int lives = 5;

	public Sprite enterDoor;
	public bool inRedTrigger;
	public bool inYellowTrigger;
	public bool inGreenTrigger;
	private GameObject door;

	public bool inJumpBlock;
	public bool done;
	public bool inSpeedBlock;
	public bool hasSpeed;

	RobotController robotController;

	public GameObject enemy;

	void Start () 
	{
		greenKey_canvas.enabled = false;
		redKey_canvas.enabled = false;
		yellowKey_canvas.enabled = false;
		done = false;

		dead = false;

		GameObject player = GameObject.Find ("Character");
		robotController = player.GetComponent<RobotController> ();
	}
	
	void Update () 
	{
		DeathChecker ();
		DoorChecker ();
		StartCoroutine(SpeedChecker());
		PowerUpChecker ();
		Attack ();

		if (dead) {
			SceneManager.LoadScene ("Dead");

		}
	}

	void FixedUpdate()
	{
		StartCoroutine(LiveChecker ());
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "GreenKey") {
			print ("Green Key taken");
			Destroy (other.gameObject);
			greenKey_canvas.enabled = true;
		} else if (other.gameObject.tag == "RedKey") {
			print ("Red Key taken");
			Destroy (other.gameObject);
			redKey_canvas.enabled = true;
		} else if (other.gameObject.tag == "YellowKey") {
			print ("Yellow Key taken");
			Destroy (other.gameObject);
			yellowKey_canvas.enabled = true;
		} else if (other.gameObject.tag == "Gem") {
			print ("Gem taken");
			gems = gems + 5;
			gemsText.text = gems.ToString ();
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "RedGem") {
			print ("Gem taken");
			gems++;
			gemsText.text = gems.ToString ();
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "LiveTaker") {
			lives--;
		} else if (other.gameObject.tag == "RedDoor") {
			inRedTrigger = true;
			door = other.gameObject;
		} else if (other.gameObject.tag == "YellowDoor") {
			inYellowTrigger = true;
			door = other.gameObject;
		} else if (other.gameObject.tag == "GreenDoor") {
			inGreenTrigger = true;
			door = other.gameObject;
		} else if (other.gameObject.tag == "Finish") {
			//FINISH LEVEL
			print ("finish level");
		} else if (other.gameObject.tag == "Enemy") {
			if (!liveTaken) {
				lives--;
				liveTaken = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "RedDoor") {
			inRedTrigger = false;
		} else if (other.gameObject.tag == "YellowDoor") {
			inYellowTrigger = false;
		} else if (other.gameObject.tag == "GreenDoor") {
			inGreenTrigger = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "JumpBlock") {
			inJumpBlock = true;
		} else if (other.gameObject.tag == "SpeedBlock") {
			inSpeedBlock = true;
		} else if (other.gameObject.tag == "LiveBlock") {
			lives++;
			Destroy (other.gameObject);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "JumpBlock") {
			inJumpBlock = false;
			done = false;
		} else if (other.gameObject.tag == "SpeedBlock") {
			inSpeedBlock = false;
			done = false;
		}
	}

	IEnumerator LiveChecker()
	{
		if (lives == 5) {
			lives1.enabled = true;
			lives2.enabled = true;
			lives3.enabled = true;
			lives4.enabled = true;
			lives5.enabled = true;
		} else if (lives == 4) {
			lives1.enabled = true;
			lives2.enabled = true;
			lives3.enabled = true;
			lives4.enabled = true;
			lives5.enabled = false;
		} else if (lives == 3) {
			lives1.enabled = true;
			lives2.enabled = true;
			lives3.enabled = true;
			lives4.enabled = false;
			lives5.enabled = false;
		} else if (lives == 2) {
			lives1.enabled = true;
			lives2.enabled = true;
			lives3.enabled = false;
			lives4.enabled = false;
			lives5.enabled = false;
		} else if (lives == 1) {
			lives1.enabled = true;
			lives2.enabled = false;
			lives3.enabled = false;
			lives4.enabled = false;
			lives5.enabled = false;
		} else if (lives <= 0) {
			lives1.enabled = false;
			lives2.enabled = false;
			lives3.enabled = false;
			lives4.enabled = false;
			lives5.enabled = false;
		}

		if (liveTaken) {
			yield return new WaitForSeconds (1);
			liveTaken = false;
		}

	}

	void DeathChecker()
	{
		if (lives <= 0) {
			dead = true;
		}

		if (transform.position.y <= -30) {
			dead = true;
		}
	}

	void DoorChecker()
	{
		if (inRedTrigger && redKey_canvas.enabled) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				door.GetComponent<SpriteRenderer> ().sprite = enterDoor;
				redKey_canvas.enabled = false;
				inRedTrigger = false;
			}
		}
		if (inYellowTrigger && yellowKey_canvas.enabled) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				door.GetComponent<SpriteRenderer> ().sprite = enterDoor;
				yellowKey_canvas.enabled = false;
				inYellowTrigger = false;
			}
		}
		if (inGreenTrigger && greenKey_canvas.enabled) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				door.GetComponent<SpriteRenderer> ().sprite = enterDoor;
				greenKey_canvas.enabled = false;
				inGreenTrigger = false;
			}
		}
	}

	void PowerUpChecker()
	{
		if (inJumpBlock) {
			
			if (!done) {
				robotController.jumpForce = robotController.jumpForce * 2;	
				done = true;
			}
		} else {
			robotController.jumpForce = 700;
		}

		if (inSpeedBlock) {
			if (!done) {
				robotController.maxSpeed = robotController.maxSpeed * 2;
				done = true;
				hasSpeed = true;
			}
		}
	}

	IEnumerator SpeedChecker()
	{
		if (hasSpeed) {
			yield return new WaitForSeconds (5);
			robotController.maxSpeed = 10;
			hasSpeed = false;
		}
	}

	void Attack()
	{
		if (transform.position.x >= enemy.transform.position.x && transform.position.x < enemy.transform.position.x + 2 ||
			transform.position.x <= enemy.transform.position.x && transform.position.x > enemy.transform.position.x - 2) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Destroy (enemy);
			}
		}

	}

}
