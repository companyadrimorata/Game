using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

	public float maxSpeed = 10;
	public bool facingRight = true;

	public Rigidbody2D rigidbody2d;
	Animator animator;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	public float jumpForce = 700;

	void Start () 
	{
		animator = GetComponent<Animator> ();

	}
	
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		animator.SetBool ("Ground", grounded);

		animator.SetFloat ("vSpeed", rigidbody2d.velocity.y);

		float move = Input.GetAxis ("Horizontal");

		animator.SetFloat ("Speed", Mathf.Abs (move));

		rigidbody2d.velocity = new Vector2 (move * maxSpeed, rigidbody2d.velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}
	}

	void Update()
	{
		if (grounded && Input.GetKeyDown (KeyCode.UpArrow)) {
			animator.SetBool ("Ground", false);
			rigidbody2d.AddForce(new Vector2(0, jumpForce));
		}

	}

	public void Flip ()
	{
		//CANVIAR DE SENTIT

		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
