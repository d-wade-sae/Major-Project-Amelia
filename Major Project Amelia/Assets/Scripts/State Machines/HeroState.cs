using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour {

    private BattleManager BM;
    public MainHero hero;
    public bool battlePlayer;
    private bool inBattle;
    public GameObject characterWorld;

    public bool playerTurn;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    public GameObject selector;

    // Attack Movements
    public GameObject enemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;
    public bool alive = true;

    void Start ()
    {
        BM = GameObject.Find("BattleMaster").GetComponent<BattleManager>();

        currentState = TurnState.PROCESSING;
        selector.SetActive(false);
        startPosition = transform.position;
	}
	
	void Update ()
    {
		
	}
}
