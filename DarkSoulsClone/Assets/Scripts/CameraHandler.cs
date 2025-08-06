using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  public class CameraHandler : MonoBehaviour
  {
    InputHandler inputHandler;
    PlayerManager playerManager;
    public Transform targetTransform;
    public Transform targetTransformWhileAiming;
    public Transform cameraTransform;
    public Camera cameraObject;
    public Transform cameraPivotTransform;
    private Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers, environmentLayer;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler singleton;

    public float leftAndRightLookSpeed = 250f;
    public float leftAndRightAimingLookSpeed = 25f;
    public float groundedFollowSpeed = 30f;
    public float aerialFollowSpeed = 10f;
    public float upAndDownLookSpeed = 250f;
    public float upAndDownAimingLookSpeed = 25f;

    private float targetPosition;
    private float defaultPosition;
    private float leftAndRightAngle;
    private float upAndDownAngle;
    public float minimumLookUpAngle = -35;
    public float maximumLookUpAngle = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;


    public CharacterManager currentLockOnTarget;

    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget, rightLockTarget;

    public float maximumLockOnDistance = 30;


    private void Awake()
    {
      singleton = this;
      defaultPosition = cameraTransform.localPosition.z;
      ignoreLayers = ~(1 << 9 | 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13);
      targetTransform = FindObjectOfType<PlayerManager>().transform;
      inputHandler = FindObjectOfType<InputHandler>();
      playerManager = FindObjectOfType<PlayerManager>();
      cameraObject = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
      environmentLayer = LayerMask.NameToLayer("Environment");
    }

    public void FollowTarget()
    {

      if (playerManager.isAiming)
      {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity,   groundedFollowSpeed * Time.deltaTime);
        transform.position = targetPosition;
      }
      else
      {
        if (playerManager.isGrounded)
        {
          Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, groundedFollowSpeed * Time.deltaTime);
          transform.position = targetPosition;
        }
        else
        {
          Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, aerialFollowSpeed * Time.deltaTime);
          transform.position = targetPosition;
        }
       
      }
      HandleCameraCollisions();

    }
    public void HandleCameraRotation()
    {
      if (inputHandler.lockOnFlag && currentLockOnTarget != null)
      {
        HandleLockedCameraRotation();
      }
      else if (playerManager.isAiming)
      {
        HandleAimedCameraRotation();
      }
      else
      {
        HandleStandartCameraRotation();
      }
    }

    public void HandleStandartCameraRotation()
    {
      leftAndRightAngle += inputHandler.mouseX * leftAndRightLookSpeed * Time.deltaTime;
      upAndDownAngle -= inputHandler.mouseY * upAndDownLookSpeed * Time.deltaTime;
      upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookUpAngle, maximumLookUpAngle);

      Vector3 rotation = Vector3.zero;
      rotation.y = leftAndRightAngle;
      Quaternion targetRotation = Quaternion.Euler(rotation);
      transform.rotation = targetRotation;

      rotation = Vector3.zero;
      rotation.x = upAndDownAngle;

      targetRotation = Quaternion.Euler(rotation);
      cameraPivotTransform.localRotation = targetRotation;

    }

    private void HandleLockedCameraRotation()
    {
      Vector3 dir = currentLockOnTarget.transform.position - transform.position;
      dir.Normalize();
      dir.y = 0;

      Quaternion targetRotation = Quaternion.LookRotation(dir);
      transform.rotation = targetRotation;

      dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
      dir.Normalize();

      targetRotation = Quaternion.LookRotation(dir);
      Vector3 eulerAngle = targetRotation.eulerAngles;
      eulerAngle.y = 0;
      cameraPivotTransform.localEulerAngles = eulerAngle;
    }

    private void HandleAimedCameraRotation()
    {
      transform.rotation = Quaternion.Euler(0, 0, 0);
      cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

      Quaternion targetRotationX;
      Quaternion targetRotationY;

      Vector3 cameraRotationX = Vector3.zero;
      Vector3 cameraRotationY = Vector3.zero;

      leftAndRightAngle += (inputHandler.mouseX * leftAndRightAimingLookSpeed) * Time.deltaTime;
      upAndDownAngle -= (inputHandler.mouseY * upAndDownAimingLookSpeed) * Time.deltaTime;

      cameraRotationY.y = leftAndRightAngle;
      targetRotationY = Quaternion.Euler(cameraRotationY);
      targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
      transform.localRotation = targetRotationY;

      cameraRotationX.x = upAndDownAngle;
      targetRotationX = Quaternion.Euler(cameraRotationX);
      targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
      cameraTransform.localRotation = targetRotationX;
    }


    public void ResetAimCameraRotations()
    {
      cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void HandleCameraCollisions()
    {
      targetPosition = defaultPosition;
      RaycastHit hit;
      Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
      direction.Normalize();

      if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
      {
        float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
        targetPosition = -(dis - cameraCollisionOffSet);
      }

      if (Mathf.Abs(targetPosition) < minimumCollisionOffSet)
      {
        targetPosition = -minimumCollisionOffSet;
      }

      cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime / 0.2f);
      cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
      float shortestDistance = Mathf.Infinity;
      float shortestDistanceOfLeftTarget = -Mathf.Infinity;
      float shortestDistanceOfRightTarget = Mathf.Infinity;

      Collider[] colliders = Physics.OverlapSphere(cameraTransform.position, 26);

      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterManager character = colliders[i].GetComponent<CharacterManager>();

        if (character != null)
        {
          Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
          float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
          float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
          RaycastHit hit;

          if (character.transform.root != targetTransform.transform.root
          && viewableAngle > -50 && viewableAngle < 50 &&
          distanceFromTarget <= maximumLockOnDistance)
          {
            if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
            {
              Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

              if (hit.transform.gameObject.layer == environmentLayer)
              {
                // Cannot lock the target
              }
              else
              {
                availableTargets.Add(character);

              }
            }
          }
        }
      }
      for (int k = 0; k < availableTargets.Count; k++)
      {
        float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

        if (distanceFromTarget < shortestDistance)
        {
          shortestDistance = distanceFromTarget;
          nearestLockOnTarget = availableTargets[k];
        }

        if (inputHandler.lockOnFlag)
        {
          Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
          var distanceFromLeftTarget = relativeEnemyPosition.x;
          var distanceFromRightTarget = relativeEnemyPosition.x;

          if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[k] != currentLockOnTarget)
          {
            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
            leftLockTarget = availableTargets[k];
          }
          else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget && availableTargets[k] != currentLockOnTarget)
          {
            shortestDistanceOfRightTarget = distanceFromRightTarget;
            rightLockTarget = availableTargets[k];
          }
        }
      }
    }


    public void ClearLockOnTargets()
    {
      availableTargets.Clear();
      nearestLockOnTarget = null;
      currentLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
      Vector3 velocity = Vector3.zero;
      Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
      Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

      if (currentLockOnTarget != null)
      {
        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
      }
      else
      {
        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
      }
    }


  }
}