using UnityEngine;
namespace MR
{
  public class PlayerLocomotionManager : CharacterLocomotionManager
  {
    PlayerManager player;
   

    [Header("Movement Stats")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float walkingSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 7;
    [SerializeField] private float rotationSpeed = 10;

    [Header("Stamina Costs")]
    [SerializeField] private int rollStaminaCost = 15;
    [SerializeField] private int backstepStaminaCost = 12;
    [SerializeField] private int sprintStaminaCost = 12;

 

    protected override void Awake()
    {
      base.Awake();
      player = GetComponent<PlayerManager>();
    }
    protected override void Start()
    {
      base.Start();
      
    }


    public void HandleRotation()
    {
      if (player.canRotate)
      {
        if (player.isAiming)
        {
          Quaternion targetRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
          Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
          transform.rotation = playerRotation;
        }
        else
        {
          if (player.inputHandler.lockOnFlag)
          {

            if (player.inputHandler.sprintFlag || player.inputHandler.rollFlag)
            {
              Vector3 targetDirection = Vector3.zero;
              targetDirection = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
              targetDirection += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
              targetDirection.Normalize();
              targetDirection.y = 0;

              if (targetDirection == Vector3.zero)
              {
                targetDirection = transform.forward;
              }

              Quaternion tr = Quaternion.LookRotation(targetDirection);
              Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
              transform.rotation = targetRotation;
            }
            else
            {
              Vector3 rotationDirection = moveDirection;
              rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;

              rotationDirection.y = 0;
              rotationDirection.Normalize();
              Quaternion tr = Quaternion.LookRotation(rotationDirection);
              Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
              transform.rotation = targetRotation;
            }

          }
          else
          {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = player.inputHandler.moveAmount;

            targetDir = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
            targetDir += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
              targetDir = player.transform.forward;
            }
            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

            player.transform.rotation = targetRotation;
          }
        }

      }
    }

    public void HandleGroundedMovement()
    {

      if (player.inputHandler.rollFlag)
      {
        return;
      }
      if (player.isInteracting)
      {
        return;
      }
      if (!player.isGrounded)
      {
        return;
      }

      moveDirection = player.cameraHandler.transform.forward * player.inputHandler.vertical;
      moveDirection +=  player.cameraHandler.transform.right * player.inputHandler.horizontal;
      moveDirection.Normalize();
      moveDirection.y = 0;

      if (player.isSprinting)
      {
        player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
      }else
      {
        if (player.inputHandler.moveAmount > 0.5f)
        {
          player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
        else if (player.inputHandler.moveAmount <= 0.5f)
        {
          player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
      }

      if (player.inputHandler.lockOnFlag && !player.inputHandler.sprintFlag)
      {
        player.playerAnimatorManager.UpdateAnimatorValues(player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
      }
      else
      {
        player.playerAnimatorManager.UpdateAnimatorValues(player.inputHandler.moveAmount, 0, player.isSprinting);
      }


    }

    public void HandleRollingAndSprinting()
    {
      if (player.animator.GetBool("isInteracting")) return;
      if (player.playerStatsManager.currentStamina <= 0) return;

      if (player.inputHandler.rollFlag)
      {
        player.inputHandler.rollFlag = false;
        moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
        moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

        if (player.inputHandler.moveAmount > 0)
        {
          player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
          player.playerAnimatorManager.EraseHandIKForWeapon();
          moveDirection.y = 0;
          Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
          player.transform.rotation = rollRotation;
          player.playerStatsManager.DeductStamina(rollStaminaCost);
        }
        else
        {
          player.playerAnimatorManager.PlayTargetAnimation("Backstep", true);
          player.playerAnimatorManager.EraseHandIKForWeapon();
          player.playerStatsManager.DeductStamina(backstepStaminaCost);
        }
      }
    }

   
    public void HandleJumping()
    {
      if (player.isInteracting) return;
      if (player.playerStatsManager.currentStamina <= 0) return;

      if (player.inputHandler.jump_Input)
      {
        player.inputHandler.jump_Input = false;
        if (player.inputHandler.moveAmount > 0)
        {
          moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
          moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
          player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
          player.playerAnimatorManager.EraseHandIKForWeapon();
          moveDirection.y = 0;
          Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
          player.transform.rotation = jumpRotation;
        }
      }

    }



  }

}