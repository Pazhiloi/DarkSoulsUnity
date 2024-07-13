using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

  public class PlayerManager : MonoBehaviour
  {

    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      anim = GetComponentInChildren<Animator>();
      playerLocomotion = GetComponent<PlayerLocomotion>();
      interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
      float delta = Time.deltaTime;
      isInteracting = anim.GetBool("isInteracting");
      canDoCombo = anim.GetBool("canDoCombo");

      inputHandler.TickInput(delta);
      playerLocomotion.HandleMovement(delta);
      playerLocomotion.HandleRollingAndSprinting(delta);
      playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
      CheckForInteractableObject();
    }
    private void FixedUpdate()
    {
      float delta = Time.deltaTime;

      if (cameraHandler != null)
      {
        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
      }
    }

    private void LateUpdate()
    {
      inputHandler.rollFlag = false;
      inputHandler.sprintFlag = false;
      inputHandler.rb_Input = false;
      inputHandler.rt_Input = false;
      inputHandler.d_Pad_Up = false;
      inputHandler.d_Pad_Down = false;
      inputHandler.d_Pad_Left = false;
      inputHandler.d_Pad_Right = false;
      inputHandler.a_Input = false;
      if (isInAir)
      {
        playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
      }
    }

    public void CheckForInteractableObject()
    {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
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
      else{
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
  }
}
