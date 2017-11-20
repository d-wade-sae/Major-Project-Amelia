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

    [Header("World Variables")]
    public GameObject world;


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
	}
	
	void Update ()
    {
		
	}
}
