using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionController : MonoBehaviour {

    // Public Variables
    public float moveSpeed; // how fast the companion moves

    /*
    public Text movingDebug; // ingame debug for moving
    public Text velocityDebug; // ingame debug for velocity
    */

    // Private Variables
    private Rigidbody2D companionBody;
    private bool companionMoving; // if player is moving
    private GameObject hoverPoint;
    private Vector2 currentLocation;
    private Vector2 moveLocation;
    private Vector2 lastMove;

    private GameManager GM; // link to game manager

    void Start ()
    {
        hoverPoint = GameObject.Find("HopesPosition");
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	void Update ()
    {

        moveLocation = hoverPoint.transform.position;
        currentLocation = this.transform.position;

        if (GM.controllingCompanion == false)
        {
            if (currentLocation == moveLocation)
            {

            }

            else
            {
                transform.position = Vector2.MoveTowards(currentLocation, moveLocation, moveSpeed * Time.deltaTime);
            }
        }

        else
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) //If player is moving left or right
            {
                companionBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, companionBody.velocity.y); // determines player velocity 
                companionMoving = true; // sets the player to moving
                // movingDebug.text = "Player Moving: " + playerMoving.ToString();
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) //If player is moving up or down
            {
                companionBody.velocity = new Vector2(companionBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                companionMoving = true; // sets the player to moving
                // movingDebug.text = "Player Moving: " + companionMoving.ToString();
                lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }

            if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f) // if no input
            {
                companionBody.velocity = new Vector2(0f, companionBody.velocity.y); // sets the players velocity to 0
            }

            if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f) // if no input
            {
                companionBody.velocity = new Vector2(companionBody.velocity.x, 0f); // sets the players velocity to 0
            }
        }
    }
}
