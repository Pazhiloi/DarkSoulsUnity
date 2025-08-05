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


      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      playerCombatManager = GetComponent<PlayerCombatManager>();
      playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      playerEffectsManager = GetComponent<PlayerEffectsManager>();
      interactableUI = FindObjectOfType<InteractableUI>();

      WorldSaveGameManager.instance.player = this;
    }

    protected override void Start()
    {
      base.Start();
    }




    protected override void Update()
    {
      base.Update();
      float delta = Time.deltaTime;
      isInteracting = animator.GetBool("isInteracting");
      canDoCombo = animator.GetBool("canDoCombo");
      canRotate = animator.GetBool("canRotate");
      isInvulnerable = animator.GetBool("isInvulnerable");
      isFiringSpell = animator.GetBool("isFiringSpell");
      isHoldingArrow = animator.GetBool("isHoldingArrow");
      isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
      animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
      animator.SetBool("isBlocking", isBlocking);
      animator.SetBool("isInAir", isInAir);
      animator.SetBool("isDead", isDead);


      inputHandler.TickInput();
      playerLocomotionManager.HandleRollingAndSprinting();
      playerLocomotionManager.HandleJumping();

      playerStatsManager.RegenerateStamina();
      playerLocomotionManager.HandleGroundedMovement();
      playerLocomotionManager.HandleRotation();

      CheckForInteractableObject();
    }
    protected override void FixedUpdate()
    {
      base.FixedUpdate();
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
      playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
      transform.position = playerStandsHereWhenOpeningChest.position;
      playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
      playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;

      Vector3 rotationDirection = fogWallEntrance.transform.forward;
      Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
      transform.rotation = turnRotation;

      playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);

    }
    #endregion


    public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
      currentCharacterSaveData.characterName = playerStatsManager.characterName;
      currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;
      currentCharacterSaveData.xPosition = transform.position.x;
      currentCharacterSaveData.yPosition = transform.position.y;
      currentCharacterSaveData.zPosition = transform.position.z;


      // EQUIPMENT
      currentCharacterSaveData.currentRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
      currentCharacterSaveData.currentLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;

      if (playerInventoryManager.currentHelmetEquipment != null)
      {
        currentCharacterSaveData.currentHeadGearItemID = playerInventoryManager.currentHelmetEquipment.itemID;
      }
      else
      {
        currentCharacterSaveData.currentHeadGearItemID = -1;
      }

      if (playerInventoryManager.currentBodyEquipment != null)
      {
        currentCharacterSaveData.currentChestGearItemID = playerInventoryManager.currentBodyEquipment.itemID;
      }
      else
      {
        currentCharacterSaveData.currentChestGearItemID = -1;
      }

      if (playerInventoryManager.currentLegEquipment != null)
      {
        currentCharacterSaveData.currentLegGearItemID = playerInventoryManager.currentLegEquipment.itemID;
      }
      else
      {
        currentCharacterSaveData.currentLegGearItemID = -1;
      }

      if (playerInventoryManager.currentHandEquipment != null)
      {
        currentCharacterSaveData.currentHandGearItemID = playerInventoryManager.currentHandEquipment.itemID;
      }
      else
      {
        currentCharacterSaveData.currentHandGearItemID = -1;
      }

    }

    public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
      playerStatsManager.characterName = currentCharacterSaveData.characterName;
      playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;
      transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

      // EQUIPMENT
      playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandWeaponID);
      playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandWeaponID);
      playerWeaponSlotManager.LoadBothWeaponsOnSlots();


      EquipmentItem headEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHeadGearItemID);
      // If this item exists in the data base, we apply it
      if (headEquipment != null)
      {
        playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
      }

      EquipmentItem bodyEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentChestGearItemID);
      if (bodyEquipment != null)
      {
        playerInventoryManager.currentBodyEquipment = bodyEquipment as BodyEquipment;
      }

      EquipmentItem legEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentLegGearItemID);
      if (legEquipment != null)
      {
        playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
      }

      EquipmentItem handEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHandGearItemID);
      if (handEquipment != null)
      {
        playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
      }

      playerEquipmentManager.EquipAllEquipmentModelsOnStart();
    }


  }
}