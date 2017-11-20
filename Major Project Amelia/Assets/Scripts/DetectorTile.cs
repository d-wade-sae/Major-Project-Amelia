using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTile : MonoBehaviour {

    // Public Variables
    [Header("Tile Variables")]
    public string tileName; // name of tile
    public string region; // region for tile (enemies that spawn)
    public int tilePercentage; // 

    // Private Variables
    private GameObject player;
    private GameManager GM; // Game Manager
    private int randomNumber;


	void Start ()
    {
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	

	void Update ()
    {
		
	}
}
