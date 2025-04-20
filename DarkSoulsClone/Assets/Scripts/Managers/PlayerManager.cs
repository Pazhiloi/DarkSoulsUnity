using UnityEngine;

namespace MR
{

  public class PlayerManager : CharacterManager
  {

    [Header("Camera")]
    public CameraHandler cameraHandler;

    [Header("Input")]
    public InputHandler inputHandler;

    [Header("UI")]
    public UIManager uiManager;

    [Header("Player")]
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerEffectsManager playerEffectsManager;
    public PlayerAnimatorManager playerAnimatorManager;

    [Header("Colliders")]
    public BlockingCollider blockingCollider;
    
    [Header("Interactables")]
    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;



    protected override void Awake()
    {
      base.Awake();
      cameraHandler = FindObjectOfType<CameraHandler>();
      uiManager = FindObjectOfType<UIManager>();
      inputHandler = GetComponent<InputHandler>();
      animator = GetComponent<Animator>();

      backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
      blockingCollider = GetComponentInChildren<BlockingCollider>();
      
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      playerCombatManager = GetComponent<PlayerCombatManager>();
      playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      playerEffectsManager = GetComponent<PlayerEffectsManager>();
      interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
      float delta = Time.deltaTime;
      isInteracting = animator.GetBool("isInteracting");
      canDoCombo = animator.GetBool("canDoCombo");
      canRotate = animator.GetBool("canRotate");
      isInvulnerable = animator.GetBool("isInvulnerable");
      isFiringSpell = animator.GetBool("isFiringSpell");
      isHoldingArrow = animator.GetBool("isHoldingArrow");
      animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
      animator.SetBool("isBlocking", isBlocking);
      animator.SetBool("isInAir", isInAir);
      animator.SetBool("isDead", isDead);


      inputHandler.TickInput(delta);
      playerLocomotionManager.HandleRollingAndSprinting();
      playerLocomotionManager.HandleJumping();

      playerStatsManager.RegenerateStamina();

      CheckForInteractableObject();
    }
    protected override void FixedUpdate()
    {
      base.FixedUpdate();
      playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
      playerLocomotionManager.HandleMovement();
      playerLocomotionManager.HandleRotation();
      playerEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
      inputHandler.d_Pad_Up = false;
      inputHandler.d_Pad_Down = false;
      inputHandler.d_Pad_Left = false;
      inputHandler.d_Pad_Right = false;
      inputHandler.a_Input = false;
      inputHandler.inventory_Input = false;

      if (cameraHandler != null)
      {
        cameraHandler.FollowTarget();
        cameraHandler.HandleCameraRotation();
      }
      if (isInAir)
      {
        playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
      }
    }
    #region Player Interactions
    public void CheckForInteractableObject()
    {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, 0.06f, transform.forward, out hit) && hit.collider.tag == "Interactable")
      {
        if (hit.collider.tag == "Interactable")
        {
          Interactable interactableObject = hit.collider.GetComponent<Interactable>();
          if (interactableObject != null)
          {
            string interactableText = interactableObject.interactableText;
            interactableUI.interactableText.text = interactableText;
            interactableUIGameObject.SetActive(true);

            if (inputHandler.a_Input)
            {
              hit.collider.GetComponentInChildren<Interactable>().Interact(this);
            }
          }
        }
      }
      else
      {
        if (interactableUIGameObject != null)
        {
          interactableUIGameObject.SetActive(false);
        }

        if (itemInteractableGameObject != null && inputHandler.a_Input)
        {
          itemInteractableGameObject.SetActive(false);
        }

      }
    }

    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
      playerLocomotionManager.rigidbody.velocity = Vector3.zero;
      transform.position = playerStandsHereWhenOpeningChest.position;
      playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
      playerLocomotionManager.rigidbody.velocity = Vector3.zero;

      Vector3 rotationDirection = fogWallEntrance.transform.forward;
      Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
      transform.rotation = turnRotation;

      playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);

    }
    #endregion


  }
}