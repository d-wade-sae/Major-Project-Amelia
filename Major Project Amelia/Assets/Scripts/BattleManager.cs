using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    private GameManager GM;

    [Header("Battle Attributes")]
    public GameObject background;

    [Header("Player Objects")]
    public GameObject battlePlayer;
    public GameObject battleCompanion;

    [Header("Battle Canvas Master")]
    public GameObject battleCanvas;

    [Header("Prefabs")]
    // Button Prefabs
    public GameObject actionButton;
    public GameObject enemyButton;
    public GameObject skillButton;

    [Header("Panels")]
    public GameObject actionsPanel;
    public GameObject enemySelectPanel;
    public GameObject skillsPanel;
    public GameObject heroPanel;

    [Header("Spacers")]
    public Transform actionSpacer;
    public Transform enemySelectSpacer;
    public Transform skillSpacer;


	void Start ()
    {
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	
	void Update ()
    {
		
	}

    public void StartBattle() // Everything for Starting the Battle
    {
        // Enables player and Companion
        battlePlayer.SetActive(true);
        battleCompanion.SetActive(true);
        // Enables Battle Canvas
        battleCanvas.SetActive(true);
        // Disables any Selection Panel but the action panel

    }
}
