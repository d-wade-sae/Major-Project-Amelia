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
    // Debug Canvas Variables
    public bool enableDebug;
    public GameObject debugCanvas;
    public Text debugDetected;
    public Text randomNumberDebug;
    public Text battleStartedDebug;

    [Header("World Variables")]
    public GameObject world;
    public static GameManager instance;

    [Header("Battle Attributes")]
    public bool playerDetected;
    public bool battleStarted;
    public int randomNumber;
    public int battleStartCount;
    private GameObject triggeredTile;
    


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!GameObject.Find("Player"))
        {
            GameObject PlayerSpawn = Instantiate(player, Vector2.zero, Quaternion.identity) as GameObject;
            Vector2 playerSpawnLocation = new Vector2(0, 0);
            PlayerSpawn.transform.position = playerSpawnLocation;
            PlayerSpawn.name = "Player";
        }
    }

    void Start ()
    {
        world.SetActive(true);
        player.SetActive(true);
        worldCamera.SetActive(true);

        // Enabling Debug Canvas
        if (enableDebug == true)
        {
            debugCanvas.SetActive(true);
        }

        else
        {
            debugCanvas.SetActive(false);
        }

        StartDebugCanvas(); // All GetComponent<Text> for debug 
	}
	
	void Update ()
    {
        debugDetected.text = "Player Detected: " + playerDetected.ToString();
        randomNumberDebug.text = "Random Number: " + randomNumber.ToString();
        battleStartedDebug.text = "Battle Started: " + battleStarted.ToString();
	}

    public void PlayerDetected (GameObject tile)
    {
        playerDetected = true;
        triggeredTile = tile;
        if (battleStartCount == 0)
        {
            InvokeRepeating("CallRandomNumber", 3f, 1.25f);
            print("Invoke Repeating");
        }

    }

    void StartDebugCanvas () // All GetComponent<Text> for debug 
    {
        debugDetected.GetComponent<Text>();
        randomNumberDebug.GetComponent<Text>();
        battleStartedDebug.GetComponent<Text>();
    }

    void CallRandomNumber () // Calls Random Number and runs it against the 
    {
        DetectorTile tile = triggeredTile.GetComponent<DetectorTile>();
        randomNumber = Random.Range(0, 100);
        print("Random Number: " + randomNumber);

        if (randomNumber < tile.tilePercentage && battleStartCount == 0) // checks random number against the tiles percentage chance and if battle can be started
        {
            battleStartCount++;
            StartBattle();
            randomNumber = 0;
        }
    }

    void StartBattle()
    {
        CancelInvoke("CallRandomNumber");
        battleStarted = true;
    }
}
