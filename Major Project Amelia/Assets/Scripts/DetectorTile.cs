using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTile : MonoBehaviour {

    // Public Variables
    [Header("Tile Variables")]
    public string tileName; // name of tile
    public string region; // region for tile (enemies that spawn)
    public int tilePercentage; // 
    public GameObject tile;

    // Private Variables
    private GameObject player;
    private GameManager GM; // Game Manager
    private int randomNumber;
    private bool detected;


	void Start ()
    {
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
        detected = false;
        tile = this.gameObject;
	}
	

	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player = col.gameObject;
            detected = true;
            GM.PlayerDetected(tile);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        player = null;
        detected = false;
        GM.playerDetected = false;
    }
}
