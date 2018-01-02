using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {

	
    [Header("State Machines")]
    public PerformAction battleState;
    // Enum for Battle State
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKSTATUS,
        WIN,
        LOSE
    }
    public HeroGUI HeroInput;
    // Enum for Hero Input;
    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    [Header("Lists")]
    public List<HandleTurn> PerformList = new List<HandleTurn>(); // List responsible for attacks and skill usage
    public List<GameObject> HerosInBattle = new List<GameObject>(); // List contains all heros in battle
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); // List contains all enemies in battle
    public List<GameObject> HerosToManage = new List<GameObject>(); // List controlling which heros need input by player
    // Target Button List
    private List<GameObject> targetButtons = new List<GameObject>();

    [Header("Battle Canvas and Panels")]
    public GameObject battleCanvas;
    public GameObject actionPanel;
    public GameObject targetPanel;

    [Header("Prefabs")]
    public GameObject enemyButton;

    [Header("Spacers")]
    public Transform targetSpacer;

    [Header("Audio")]
    public AudioSource battleAudioSource;
    public AudioClip battleWinSound;
    public AudioClip battleLoseSound;
    public AudioClip battleBackgroundMusic;

    // Private Variables
    private HandleTurn HeroChoice;
    private bool switchToWorld;

    void Start()
    {
        // Sets the Battle State to waiting
        battleState = PerformAction.WAIT;
        // Grabs all Heros and Enemies in Battle and adds to list
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));

        actionPanel.SetActive(false);
        targetPanel.SetActive(false);

        // Spawns Buttons
        EnemyButtons();

        battleAudioSource.GetComponent<AudioSource>();
        battleAudioSource.clip = battleBackgroundMusic;
        battleAudioSource.loop = true;
        battleAudioSource.Play();
    }

    void Update()
    {
        switch(battleState) // SWITCHING BATTLE STATE
        {
            case PerformAction.WAIT:
                if (PerformList.Count > 0)
                {
                    battleState = PerformAction.TAKEACTION;
                }

                break;

            case PerformAction.TAKEACTION:
                if (PerformList.Count == 0)
                {
                    battleState = PerformAction.CHECKSTATUS;
                }
                GameObject performer = (PerformList[0].AttackersObject); // grabs the performer at top of performlist 
                print("The Performer at space 0 is " + performer.gameObject); 
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyState ES = performer.GetComponent<EnemyState>();
                    ES.HeroToAttack = PerformList[0].AttackersTarget;
                    ES.currentState = EnemyState.TurnState.ACTION;
                }
                if (PerformList[0].Type == "Hero")
                {
                    HeroState HS = performer.GetComponent<HeroState>();
                    HS.EnemyToAttack = PerformList[0].AttackersTarget;
                    HS.currentState = HeroState.TurnState.ACTION;
                }

                battleState = PerformAction.PERFORMACTION;
                break;

            case PerformAction.PERFORMACTION:

                break;

            case PerformAction.CHECKSTATUS:
                if (HerosInBattle.Count == 0)
                {
                    battleState = PerformAction.LOSE; 
                }

                else if (EnemiesInBattle.Count == 0)
                {
                    battleState = PerformAction.WIN;
                }

                else
                {
                    battleState = PerformAction.WAIT;
                    HeroInput = HeroGUI.ACTIVATE;
                }
                break;

            case PerformAction.WIN:
                print("Battle Won!");
                PerformList.Clear();
                HeroInput = HeroGUI.WAITING;
                foreach (GameObject hero in HerosInBattle)
                {
                    HeroState HS = hero.GetComponent<HeroState>();
                    HS.currentState = HeroState.TurnState.WAITING;
                    HS.EnemyToAttack = null;
                    HS.selector.SetActive(false);
                }
                actionPanel.SetActive(false);
                targetPanel.SetActive(false);
                battleAudioSource.Stop();
                battleAudioSource.loop = false;
                battleAudioSource.clip = battleWinSound;
                battleAudioSource.Play();

                StartCoroutine(WinBattle());

                break;

            case PerformAction.LOSE:
                battleAudioSource.Stop(); 
                battleAudioSource.clip = battleLoseSound;
                battleAudioSource.loop = false;
                battleAudioSource.Play();
                StartCoroutine(LoseBattle());
                break;
        }

        switch (HeroInput)
        {
            case HeroGUI.ACTIVATE:
                if(HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();
                    ClearAttackPanel();
                    EnemyButtons();
                    actionPanel.SetActive(true);
                    HeroInput = HeroGUI.WAITING;
                }
                break;

            case HeroGUI.DONE:
                HeroInputDone();
                break;

            case HeroGUI.INPUT1:
                 
                break;

            case HeroGUI.INPUT2:

                break;

            case HeroGUI.WAITING:

                break;
        }

    }

    public void CollectActions(HandleTurn input) // Passes Input from Enemy and Hero into the Perform List
    {
        PerformList.Add(input);
    }

    public void EnemyButtons() // Spawns all Enemy Buttons for player to target enemys with
    {
        foreach (GameObject enemy in EnemiesInBattle) 
        {
            GameObject newButton = Instantiate(enemyButton);
            newButton.transform.SetParent(targetSpacer);
            newButton.transform.localScale = new Vector3(1f, 1f, 1f);
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyState currentEnemy = enemy.GetComponent<EnemyState>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = currentEnemy.enemy.theName;

            button.EnemyPrefab = enemy;

            targetButtons.Add(newButton);
        }
    }

    public void Input1()
    {
        HeroChoice.AttackersName = HerosToManage[0].name;
        HeroChoice.AttackersObject = HerosToManage[0];
        HeroChoice.Type = "Hero";

        // Disables Action Panel and Enables Enemy Target Panel
        actionPanel.SetActive(false);
        targetPanel.SetActive(true);
    }

    public void Input2(GameObject targetEnemy) // Enemy Target Selection
    {
        HeroChoice.AttackersTarget = targetEnemy;
        HeroInput = HeroGUI.DONE;
    }

    public void HeroInputDone() // Completed Hero Input and sending info to the perform list plus removing hero from HerosToManage
    {
        PerformList.Add(HeroChoice);
        targetPanel.SetActive(false);

        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    public void ClearAttackPanel()
    {
        targetPanel.SetActive(false);
        actionPanel.SetActive(false);
        foreach (GameObject targetButton in targetButtons)
        {
            Destroy(targetButton);
        }
        targetButtons.Clear();
    }

    public IEnumerator WinBattle()
    {

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("Overworld");

    }

    public IEnumerator LoseBattle()
    {
        yield return new WaitForSeconds(5);

        Application.Quit();
    }






}
