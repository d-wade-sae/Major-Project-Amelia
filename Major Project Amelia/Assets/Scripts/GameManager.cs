using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Player Variables")]
    public GameObject player;

    [Header("Cameras")]
    public GameObject worldCamera;

    [Header("Canvas")]
    public bool enableDebug;
    public GameObject debugCanvas;
    public Text debugDetected;

    [Header("World Variables")]
    public GameObject world;

    [Header("Battle Attributes")]
    public bool playerDetected;


    void Awake()
    {
        // Enabling Debug Canvas
        if (enableDebug == true)
        {
            debugCanvas.SetActive(true);
        }

        else
        {
            debugCanvas.SetActive(false);
        }
    }

    void Start ()
    {
        world.SetActive(true);
        player.SetActive(true);
        worldCamera.SetActive(true);

        StartDebugCanvas(); // All GetComponent<Text> for debug 
	}
	
	void Update ()
    {
        debugDetected.text = "Player Detected: " + playerDetected.ToString();
	}

    public void PlayerDetected (GameObject triggerTile)
    {
        DetectorTile tile = triggerTile.GetComponent<DetectorTile>();
        playerDetected = true;
    }

    void StartDebugCanvas () // All GetComponent<Text> for debug 
    {
        debugDetected.GetComponent<Text>();
    }
}
