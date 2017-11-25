using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyState : MonoBehaviour
{
    private BattleManager BM;
    public BaseEnemy enemy;

    public bool enableDebugLines = false;

    public bool enemyTurn = false;

    // Indicator for which enemy is active
    public GameObject selector;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;


    // Time for Action Variables
    private Vector3 startPosition; // Start Position for moving
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;

    private bool alive = true;

}