using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SG
{
  public class EnemyManager : CharacterManager
  {

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;


    public State currentState;
    public CharacterStats currentTarget;

    public bool isPreformingAction;
    public bool isInteracting;

    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;

    [Header("Combat Flags")]
    public bool canDoCombo;


    [Header("A.I Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;

    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public float comboLikelyHood;

    private void Awake()
    {
      enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
      enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
      enemyStats = GetComponent<EnemyStats>();
      navMeshAgent = GetComponentInChildren<NavMeshAgent>();
      enemyRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
      navMeshAgent.enabled = false;
      enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
      HandleRecoveryTimer();
      HandleStateMachine();

      isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
      isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
      canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
      canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
      enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
    }

    private void LateUpdate() {
      navMeshAgent.transform.localPosition = Vector3.zero;
      navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {

      if (currentState != null)
      {
        State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

        if (nextState != null)
        {
          SwitchToNextState(nextState);
        }
      }
    }

    private void SwitchToNextState(State state)
    {
      currentState = state;
    }

    private void HandleRecoveryTimer()
    {
      if (currentRecoveryTime > 0)
      {
        currentRecoveryTime -= Time.deltaTime;
      }

      if (isPreformingAction)
      {
        if (currentRecoveryTime <= 0)
        {
          isPreformingAction = false;
        }
      }
    }

    #region  Attacks



    #endregion
  }
}
