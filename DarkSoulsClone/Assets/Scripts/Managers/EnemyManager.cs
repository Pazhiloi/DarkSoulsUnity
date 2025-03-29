using UnityEngine;
using UnityEngine.AI;
namespace SG
{
  public class EnemyManager : CharacterManager
  {

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStatsManager enemyStatsManager;
    EnemyEffectsManager enemyEffectsManager;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;


    public State currentState;
    public CharacterStatsManager currentTarget;

    public bool isPreformingAction;

    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;


    [Header("A.I Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;

    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;

    private void Awake()
    {
      enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
      enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
      enemyStatsManager = GetComponent<EnemyStatsManager>();
      enemyEffectsManager = GetComponent<EnemyEffectsManager>();
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

      isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
      isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
      isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
      isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
      canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
      canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
      enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
    }

    private void FixedUpdate() {
      enemyEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate() {
      navMeshAgent.transform.localPosition = Vector3.zero;
      navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {

      if (currentState != null)
      {
        State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

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
