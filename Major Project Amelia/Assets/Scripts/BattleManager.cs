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

    [Header("States")]
    public PerformAction battleStates;
    public HeroGUI HeroInput;

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE,
        RESET
    }

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE,
        RESET
    }

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
        // Enables all panels for spawning of buttons
        actionsPanel.SetActive(true);
        enemySelectPanel.SetActive(true);
        skillsPanel.SetActive(true);
        
    }

    void SpawnEnemies()
    {
        
    }
}
