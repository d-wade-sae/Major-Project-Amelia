﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    // Public Variables
    public float moveSpeed; // how fast the player moves
    public Text movingDebug; // ingame debug for moving
    public Text velocityDebug; // ingame debug for velocity

    // Private Variables
    private Rigidbody2D playerBody;
    private bool playerMoving; // if player is moving
    private Vector2 lastMove;
    private Animator characterAnim; // referencing the player animator

    private GameManager GM; // link to game manager


    void Start ()
    {
        playerBody = GetComponent<Rigidbody2D>();
        movingDebug.GetComponent<Text>();
        velocityDebug.GetComponent<Text>();
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
        characterAnim = GetComponent<Animator>();
    }
	
	void Update ()
    {
        playerMoving = false;
        movingDebug.text = "Player Moving: " + playerMoving.ToString();
        velocityDebug.text = "Player Velocity: " + playerBody.velocity.ToString();

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) //If player is moving left or right
        {
            playerBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerBody.velocity.y); // determines player velocity 
            playerMoving = true; // sets the player to moving
            movingDebug.text = "Player Moving: " + playerMoving.ToString();
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) //If player is moving up or down
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true; // sets the player to moving
            movingDebug.text = "Player Moving: " + playerMoving.ToString();
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f) // if no input
        {
            playerBody.velocity = new Vector2(0f, playerBody.velocity.y); // sets the players velocity to 0
        }

        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f) // if no input
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0f); // sets the players velocity to 0
        }

        characterAnim.SetFloat("Move X", Input.GetAxisRaw("Horizontal"));
        characterAnim.SetFloat("Move Y", Input.GetAxisRaw("Vertical"));
        //checks the Animator to instantiate the corresponding sprite/animation for the following axis
        characterAnim.SetBool("Player Moving", playerMoving);
        characterAnim.SetFloat("Last Move X", lastMove.x);
        //sets the conditions of the animators parameters to either true or false on the X axis
        characterAnim.SetFloat("Last Move Y", lastMove.y);
        //sets the conditions of the animators parameters to either true or false on the Y axis

        //TIP FOR ANIMATOR: We need to make sure to turn off 'Has Exit Time'and 'Fixed Durations'
    }
}
