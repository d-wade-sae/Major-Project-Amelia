using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    private GameManager GM;

    [Header("Battle Attributes")]
    public static BattleManager instance;
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

    public enum PerformAction // Manages the Battle States and wether anything is performing an action
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE,
        RESET
    }

    public enum HeroGUI // Responsible for Hero Turns
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE,
        RESET
    }

    [Header("Lists")]
    public List<TurnBasedHandler> PerformList = new List<TurnBasedHandler>(); // List that handles whos turn it is to perform an action
    public List<GameObject> HerosInGame = new List<GameObject>(); // List that handles which heros are in game
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); // List that handles which enemies are in game
    public List<GameObject> HerosToManage = new List<GameObject>(); //  List that is responsible for which hero turn it is
    public List<GameObject> EnemiesToManage = new List<GameObject>(); // list that is responsible for which enemies need turns
    public List<GameObject> DeadEnemies = new List<GameObject>(); // list of dead enemies (only used for 2 or more enemies)
    
    // Private Lists
    // List for Attack Buttons
    private List<GameObject> attackButtons = new List<GameObject>();
    // List for Enemy Buttons
    private List<GameObject> enemyButtons = new List<GameObject>();

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
