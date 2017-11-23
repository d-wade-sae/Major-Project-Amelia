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
    private bool playerMoving; // if player is moving
    private GameObject hoverPoint;
    private Vector2 currentLocation;
    private Vector2 moveLocation;

    void Start ()
    {
        hoverPoint = GameObject.Find("HopesPosition");
	}
	
	void Update ()
    {

        moveLocation = hoverPoint.transform.position;
        currentLocation = this.transform.position;

        if (currentLocation == moveLocation)
        {
            
        }

        else
        {
            transform.position = Vector2.MoveTowards(currentLocation, moveLocation, moveSpeed * Time.deltaTime);
        }
	}
}
