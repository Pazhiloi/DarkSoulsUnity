using UnityEngine;
using UnityEngine.AI;
namespace MR
{
  public class AICharacterManager : CharacterManager
  {

    public EnemyBossManager enemyBossManager;
   public AICharacterLocomotionManager enemyLocomotionManager;
   public AICharacterAnimatorManager enemyAnimatorManager;
   public AICharacterStatsManager enemyStatsManager;
   public AICharacterEffectsManager enemyEffectsManager;
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
    public float stoppingDistance = 1.2f;

    [Header("Advanced A.I Settings")]
    public bool allowAIToPerformBlock;
    public int blockLikelyHood = 50;
    public bool allowAIToPerformDodge;
    public int dodgeLikelyHood = 50;

    public bool allowAIToPerformParry;
    public int parryLikelyHood = 50;


    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;
    public AICombatStyle combatStyle;

    [Header("A.I Archery Settings")]
    public bool isStationaryArcher;
    public float minimumTimeToAimAtTarget = 3;
    public float maximumTimeToAimAtTarget = 6;
    [Header("A.I Companion Settings")]
    public float maxDistanceFromCompanion;
    public float minimumDistanceFromCompanion;
    public float returnDistanceFromCompanion;
    public float distanceFromCompanion;
    public CharacterManager companion; 

    [Header("A.I Target Information")]
    public float distanceFromTarget;
    public Vector3 targetsDirection;
    public float viewableAngle;

    protected override void Awake()
    {
      base.Awake();
      enemyLocomotionManager = GetComponent<AICharacterLocomotionManager>();
      enemyBossManager = GetComponent<EnemyBossManager>();
      enemyAnimatorManager = GetComponent<AICharacterAnimatorManager>();
      enemyStatsManager = GetComponent<AICharacterStatsManager>();
      enemyEffectsManager = GetComponent<AICharacterEffectsManager>();
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
      isHoldingArrow = animator.GetBool("isHoldingArrow");
      canDoCombo = animator.GetBool("canDoCombo");
      canRotate = animator.GetBool("canRotate");
      animator.SetBool("isDead", isDead);
      animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
      animator.SetBool("isBlocking", isBlocking);

      if (currentTarget != null)
      {
         distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
          targetsDirection = currentTarget.transform.position - transform.position;
        viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
      }

      if (companion != null)
      {
        distanceFromCompanion = Vector3.Distance(companion.transform.position, transform.position);
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