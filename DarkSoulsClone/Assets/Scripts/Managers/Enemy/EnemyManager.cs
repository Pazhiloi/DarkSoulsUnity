using UnityEngine;
using UnityEngine.AI;
namespace MR
{
  public class EnemyManager : CharacterManager
  {

    public EnemyBossManager enemyBossManager;
   public EnemyLocomotionManager enemyLocomotionManager;
   public EnemyAnimatorManager enemyAnimatorManager;
   public EnemyStatsManager enemyStatsManager;
   public EnemyEffectsManager enemyEffectsManager;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;


    public State currentState;
    public CharacterManager currentTarget;

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
    public AICombatStyle combatStyle;

    [Header("A.I Target Information")]
    public float distanceFromTarget;
    public Vector3 targetsDirection;
    public float viewableAngle;

    protected override void Awake()
    {
      base.Awake();
      enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
      enemyBossManager = GetComponent<EnemyBossManager>();
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

      isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
      isInteracting = animator.GetBool("isInteracting");
      isPhaseShifting = animator.GetBool("isPhaseShifting");
      isInvulnerable = animator.GetBool("isInvulnerable");
      canDoCombo = animator.GetBool("canDoCombo");
      canRotate = animator.GetBool("canRotate");
      animator.SetBool("isDead", isDead);

      if (currentTarget != null)
      {
         distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
          targetsDirection = currentTarget.transform.position - transform.position;
        viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
      }
      
    }

    protected override void FixedUpdate()
    {
      base.FixedUpdate();
      enemyEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
      navMeshAgent.transform.localPosition = Vector3.zero;
      navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {

      if (currentState != null)
      {
        State nextState = currentState.Tick(this);

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