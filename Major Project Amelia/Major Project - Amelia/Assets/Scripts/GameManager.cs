﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Player Variables")]
    public GameObject player;
    public GameObject companion;

    [Header("Cameras")]
    public GameObject worldCamera;
    public GameObject battleCamera;

    [Header("Canvas")]
    // Debug Canvas Variables
    public bool enableDebug;
    public GameObject debugCanvas;
    public GameObject battleCanvas;
    public Text debugDetected;
    public Text randomNumberDebug;
    public Text battleStartedDebug;

    [Header("World Variables")]
    public GameObject world;
    public GameObject battleArea;
    public static GameManager instance;
    public BattleManager BM;

    [Header("Battle Attributes")]
    public bool playerDetected; 
    public bool battleStarted;
    public int randomNumber;
    public int battleStartCount;
    private GameObject triggeredTile;
    


    void Awake()
    {
        if (instance == null) // Checking to see if there is a duplicate game manager and deleting if false
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!GameObject.Find("Player")) // Spawning in Player if none in scene
        {
            GameObject PlayerSpawn = Instantiate(player, Vector2.zero, Quaternion.identity) as GameObject;
            Vector2 playerSpawnLocation = new Vector2(0, 0);
            PlayerSpawn.transform.position = playerSpawnLocation;
            PlayerSpawn.name = "Player";
        }

        if (!GameObject.Find("HopeCompanion")) // Spawning in Companion if none in scene
        {
            GameObject companionSpawn = Instantiate(companion, Vector2.zero, Quaternion.identity) as GameObject;
            Vector2 companionSpawnPoint = new Vector2(3.5f, 3.5f);
            companionSpawn.transform.position = companionSpawnPoint;
            companionSpawn.name = "HopeCompanion";
        }
    }

    void Start ()
    {
        // Sets WorldMaster just in case of issue and disables
        world = GameObject.Find("WorldMaster");
        world.SetActive(true);
        // Sets BattleArea just in case of issue and disables
        battleArea = GameObject.Find("BattleMaster");
        BM = GameObject.Find("BattleMaster").GetComponent<BattleManager>();
        battleArea.SetActive(false);
        // Sets Main Camera to active and disables any others
        worldCamera.SetActive(true);
        battleCamera.SetActive(false);
        // Sets Player/Companion to Active and Disables Battle Player
        player.SetActive(true);
        companion.SetActive(true);
        // Checking to see if Debug Canvas should be enabled or disabled for testing purposes
        if (enableDebug == true)
        {
            debugCanvas.SetActive(true);
        }
        else
        {
            debugCanvas.SetActive(false);
        }
        StartDebugCanvas(); // All GetComponent<Text> for debug
        // Disables Battle Canvas
        battleCanvas.SetActive(false);
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
            InvokeRepeating("CallRandomNumber", 1, 1.25f);
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
        randomNumber = Random.Range(0, 100); // grabs a random number between 0 - 100
        print("Random Number: " + randomNumber);

        if (randomNumber < tile.tilePercentage && battleStartCount == 0) // checks random number against the tiles percentage chance and if battle can be started
        {
            battleStartCount++;
            LoadBattle();
        }
    }

    void LoadBattle() // Prep for Loading Battle
    {
        CancelInvoke("CallRandomNumber"); // stops calling the random number
        // Enabling Battle Area for Setup
        battleArea.SetActive(true);

        // Switches Camera
        worldCamera.SetActive(false);
        battleCamera.SetActive(true);
        // Stops the Player and companion, deactivates controllers along with colliders
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.GetComponent<CharacterController>().enabled = !enabled;
        player.GetComponent<BoxCollider2D>().enabled = !enabled;
        companion.GetComponent<CompanionController>().enabled = !enabled;
        // Sets bool to true
        battleStarted = true;

        // Last Step, Loads Up all Battle Varaibles
        BM.StartBattle();
        
    }
}
