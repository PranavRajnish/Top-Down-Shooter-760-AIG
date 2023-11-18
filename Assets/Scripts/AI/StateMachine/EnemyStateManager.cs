using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    [SerializeField] private EnemyPathfinding pathfinding;
    [SerializeField] EnemyPerception enemyPerception;

    [SerializeField] private PolygonCollider2D baseCollider = null;

    [Header ("Idle State Properties")]
    public float patrolPointRadius = 10f;
    public float waitTimeBetweenPatrol = 5f;


    public EnemyPathfinding Pathfinding => pathfinding;
    public EnemyPerception Perception => enemyPerception;
    public PolygonCollider2D BaseCollider => baseCollider;

    public bool gotHit;


    private void Start()
    {
        baseCollider = GameObject.FindWithTag("Base").GetComponent<PolygonCollider2D>();

        currentState = new EnemyCapturingState(EnemyState.Capturing, this);
        states.Add(EnemyState.Capturing, currentState);
        states.Add(EnemyState.FindPlayer, new EnemyFindPlayerState(EnemyState.FindPlayer, this));
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        states.Add(EnemyState.Shooting, new EnemyShootingState(EnemyState.Shooting, this));
        states.Add(EnemyState.Reloading, new EnemyReloadingState(EnemyState.Reloading, this));

        currentState.EnterState();
    }

    private void OnEnable()
    {
        GetComponent<CharacterDefenseStats>().OnCharacterHit += OnCharacterHit;
    }

    private void OnDisable()
    {
        GetComponent<CharacterDefenseStats>().OnCharacterHit -= OnCharacterHit;
    }

    private void OnCharacterHit()
    {
        gotHit = true;
    }

    public enum EnemyState
    {
        Capturing,
        FindPlayer,
        Idle,
        Shooting,
        Reloading,
    };

    // Start is called before the first frame update
    private void Awake()
    {
    }
}