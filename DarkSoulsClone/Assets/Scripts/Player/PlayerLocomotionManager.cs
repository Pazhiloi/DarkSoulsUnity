using UnityEngine;
namespace MR
{
  public class PlayerLocomotionManager : MonoBehaviour
  {
    PlayerManager player;
    public Vector3 moveDirection;
    public new Rigidbody rigidbody;

    [Header("Ground & Air Detection Stats")]
    [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = -0.2f;
    public LayerMask groundLayer;
    public float inAirTimer;

    [Header("Movement Stats")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float walkingSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 7;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float fallingSpeed = 45;

    [Header("Stamina Costs")]
    [SerializeField] private int rollStaminaCost = 15;
    [SerializeField] private int backstepStaminaCost = 12;
    [SerializeField] private int sprintStaminaCost = 12;

    Vector3 normalVector;
    Vector3 targetPosition;
    public CapsuleCollider characterCollider, characterCollisionBlockerCollider;

    private void Awake()
    {
      player = GetComponent<PlayerManager>();
      rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
      player.isGrounded = true;
      groundLayer = ~(1 << 8 | 1 << 11);
      Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
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

    public void HandleMovement()
    {

      if (player.inputHandler.rollFlag)
      {
        return;
      }
      if (player.isInteracting)
      {
        return;
      }

      moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
      moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
      moveDirection.Normalize();
      moveDirection.y = 0;

      float speed = movementSpeed;

      if (player.inputHandler.sprintFlag && player.inputHandler.moveAmount > 0.5)
      {
        speed = sprintSpeed;
        player.isSprinting = true;
        moveDirection *= speed;
        player.playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
      }
      else
      {
        if (player.inputHandler.moveAmount <= 0.5)
        {
          moveDirection *= walkingSpeed;
          player.isSprinting = false;
        }
        else
        {
          moveDirection *= speed;
          player.isSprinting = false;
        }
      }

      Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
      rigidbody.velocity = projectedVelocity;
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
          player.playerStatsManager.TakeStaminaDamage(rollStaminaCost);
        }
        else
        {
          player.playerAnimatorManager.PlayTargetAnimation("Backstep", true);
          player.playerAnimatorManager.EraseHandIKForWeapon();
          player.playerStatsManager.TakeStaminaDamage(backstepStaminaCost);
        }
      }
    }

    public void HandleFalling( Vector3 moveDirection)
    {
      player.isGrounded = false;
      RaycastHit hit;
      Vector3 origin = player.transform.position;
      origin.y += groundDetectionRayStartPoint;

      if (Physics.Raycast(origin, player.transform.forward, out hit, 0.4f))
      {
        moveDirection = Vector3.zero;
      }

      if (player.isInAir)
      {
        rigidbody.AddForce(-Vector3.up * fallingSpeed);
        rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
      }

      Vector3 dir = moveDirection;
      dir.Normalize();
      origin = origin + dir * groundDirectionRayDistance;

      targetPosition = player.transform.position;

      Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
      if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
      {
        normalVector = hit.normal;
        Vector3 tp = hit.point;
        player.isGrounded = true;
        targetPosition.y = tp.y;

        if (player.isInAir)
        {
          if (inAirTimer > 0.5f)
          {
            Debug.Log("You were in the air for " + inAirTimer);
            player.playerAnimatorManager.PlayTargetAnimation("Landing", true);
            inAirTimer = 0;
          }
          else
          {
            player.playerAnimatorManager.PlayTargetAnimation("Empty", false);
            inAirTimer = 0;
          }
          player.isInAir = false;
        }
      }
      else
      {
        if (player.isGrounded)
        {
          player.isGrounded = false;
        }
        if (player.isInAir == false)
        {

          if (player.isInteracting == false)
          {
            player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
          }

          Vector3 vel = rigidbody.velocity;
          vel.Normalize();
          rigidbody.velocity = vel * (movementSpeed / 2);
          player.isInAir = true;
        }
      }

      if (player.isGrounded)
      {
        if (player.isInteracting || player.inputHandler.moveAmount > 0)
        {
          player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
          player.transform.position = targetPosition;
        }
      }
    }

    public void HandleJumping()
    {
      if (player.isInteracting) return;
      if (player.playerStatsManager.currentStamina <= 0) return;

      if (player.inputHandler.jump_Input)
      {

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