using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public float moveSpeed = 1f;
	public string areaTransitionName;
	public static PlayerController instance;
	public bool canMove = true;

	private Rigidbody2D myRB;
	private Animator myAnim;
	private Vector3 bottomLeftLimit, topRightLimit;

	void Awake () {
		// Check if player already exists in scene
		if (instance == null) {
			instance = this;
		}else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		// Set Private Variables
		myRB = GetComponent<Rigidbody2D>();
		myAnim = GetComponent<Animator>();
	}

	void Update () {
		
	}

	void FixedUpdate() {
		// Player Movement
		if (canMove) {
			myRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
		}else {
			myRB.velocity = Vector2.zero;
		}

		// Set the player animator variables
		myAnim.SetFloat("moveX", myRB.velocity.x);
		myAnim.SetFloat("moveY", myRB.velocity.y);

		// Determine last facing direction of player and 
		if (Input.GetAxisRaw("Horizontal") == 1|| Input.GetAxisRaw("Horizontal") == -1 || 
			Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) {

			if (canMove) {
				// set animator variable to proper direction when player stops moving
				myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
				myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
			}
		}

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
			Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
	}

	public void SetBounds(Vector3 botLeft, Vector3 topRight) {

		bottomLeftLimit = botLeft + new Vector3(.5f, .8f, 0);
		topRightLimit = topRight + new Vector3(-.5f, -.8f, 0);
	}
}
