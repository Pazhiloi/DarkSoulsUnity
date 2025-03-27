using UnityEngine;

namespace SG
{

  public class PlayerManager : CharacterManager
  {

    InputHandler inputHandler;
    Animator animator;
    CameraHandler cameraHandler;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerStatsManager playerStatsManager;
    PlayerAnimatorManager playerAnimatorManager;
    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand, isUsingLeftHand;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
      inputHandler = GetComponent<InputHandler>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      animator = GetComponent<Animator>();
      playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
      float delta = Time.deltaTime;
      isInteracting = animator.GetBool("isInteracting");
      canDoCombo = animator.GetBool("canDoCombo");
      isUsingRightHand = animator.GetBool("isUsingRightHand");
      isUsingLeftHand = animator.GetBool("isUsingLeftHand");
      isInvulnerable = animator.GetBool("isInvulnerable");
      isFiringSpell = animator.GetBool("isFiringSpell");

      animator.SetBool("isBlocking", isBlocking);
      animator.SetBool("isInAir", isInAir);
      animator.SetBool("isDead", playerStatsManager.isDead);

      
      inputHandler.TickInput(delta);
      playerAnimatorManager.canRotate = animator.GetBool("canRotate");
      playerLocomotionManager.HandleRollingAndSprinting(delta);
      playerLocomotionManager.HandleJumping();

      playerStatsManager.RegenerateStamina();

      CheckForInteractableObject();
    }
    private void FixedUpdate()
    {
      float delta = Time.fixedDeltaTime;
      playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
      playerLocomotionManager.HandleMovement(delta);
      playerLocomotionManager.HandleRotation(delta);
    }

    private void LateUpdate()
    {
      inputHandler.rollFlag = false;
      inputHandler.rb_Input = false;
      inputHandler.rt_Input = false;
      inputHandler.lt_Input = false;
      inputHandler.d_Pad_Up = false;
      inputHandler.d_Pad_Down = false;
      inputHandler.d_Pad_Left = false;
      inputHandler.d_Pad_Right = false;
      inputHandler.a_Input = false;
      inputHandler.jump_Input = false;
      inputHandler.inventory_Input = false;

      float delta = Time.deltaTime;
      if (cameraHandler != null)
      {
        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
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

    public void PassThroughFogWallInteraction(Transform fogWallEntrance){
      playerLocomotionManager.rigidbody.velocity = Vector3.zero;

      Vector3 rotationDirection  = fogWallEntrance.transform.forward;
      Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
      transform.rotation = turnRotation;

      playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
      
    }
    #endregion


  }
}
