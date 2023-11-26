using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>
{
    public enum EnemyState
    {
        Capturing,
        FindPlayer,
        Idle,
        Shooting,
        Reloading,
        Hiding
    };

    [SerializeField] private EnemyPathfinding pathfinding;
    [SerializeField] EnemyPerception enemyPerception;

    [SerializeField] private PolygonCollider2D baseCollider = null;

    [Header("Idle State Properties")]
    public float patrolPointRadius = 10f;
    public float waitTimeBetweenPatrol = 5f;

    [Header("Hiding State Properties")]
    public float sniperWaitingTimeMin = 2f;
    public float sniperWaitingTimeMax = 10f;

    public EnemyPathfinding Pathfinding => pathfinding;
    public EnemyPerception Perception => enemyPerception;
    public PolygonCollider2D BaseCollider => baseCollider;
    public Gun currentGun;

    public bool gotHit;

    
    private float distanceFromPlayer;

    private void Start()
    {
        baseCollider = GameObject.FindWithTag("Base").GetComponent<PolygonCollider2D>();
        OnGunChange();

        currentState = new EnemyCapturingState(EnemyState.Capturing, this);
        states.Add(EnemyState.Capturing, currentState);
        states.Add(EnemyState.FindPlayer, new EnemyFindPlayerState(EnemyState.FindPlayer, this));
        states.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        states.Add(EnemyState.Shooting, new EnemyShootingState(EnemyState.Shooting, this));
        states.Add(EnemyState.Reloading, new EnemyReloadingState(EnemyState.Reloading, this));
        states.Add(EnemyState.Hiding, new EnemyHidingState(EnemyState.Hiding, this));

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


    // Start is called before the first frame update
    private void Awake()
    {
    }

    private void OnGunChange()
    {
        currentGun = transform.GetComponent<Enemy>().currentGun;

        if (currentGun is ARGun)
        {

        }
        else if (currentGun is Pistol)
        {

        }
        else if (currentGun is Sniper)
        {

        }

        distanceFromPlayer = Perception.GetSightDistance() - 1;
    }
}