using UnityEngine;

namespace SG
{
  public class InputHandler : MonoBehaviour
  {
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool a_Input;
    public bool x_Input;
    public bool y_Input;

    public bool rb_Input, lb_Input;
    public bool hold_rb_Input;
    public bool rt_Input, lt_Input;

    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input, right_Stick_Left_Input;


    public bool d_Pad_Up, d_Pad_Down, d_Pad_Left, d_Pad_Right;

    public bool rollFlag;
    public bool twoHandFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool fireFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    public Transform criticalAttackRayCastStartPoint;


    PlayerControls inputActions;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerStatsManager playerStatsManager;
    BlockingCollider blockingCollider;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    CameraHandler cameraHandler;
    UIManager uiManager;


    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
      playerCombatManager = GetComponent<PlayerCombatManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerManager = GetComponent<PlayerManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      playerEffectsManager = GetComponent<PlayerEffectsManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      blockingCollider = GetComponentInChildren<BlockingCollider>();
      uiManager = FindObjectOfType<UIManager>();
      cameraHandler = FindObjectOfType<CameraHandler>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }



    public void OnEnable()
    {
      if (inputActions == null)
      {
        inputActions = new PlayerControls();
        inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
        inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
        inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
        inputActions.PlayerActions.HoldRB.canceled += i => fireFlag = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;
        inputActions.PlayerActions.LT.performed += i => lt_Input = true;
        inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
        inputActions.PlayerActions.LB.performed += i => lb_Input = true;
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        inputActions.PlayerActions.A.performed += i => a_Input = true;
        inputActions.PlayerActions.X.performed += i => x_Input = true;
        inputActions.PlayerActions.Roll.performed += i => b_Input = true;
        inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
        inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
        inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
        inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
        inputActions.PlayerActions.Y.performed += i => y_Input = true;
      }

      inputActions.Enable();
    }

    private void OnDisable()
    {
      inputActions.Disable();
    }

    public void TickInput(float delta)
    {
      if (playerStatsManager.isDead) return;
      HandleMoveInput(delta);
      HandleRollInput(delta);
      HandleLBInput();
      HandleCombatInput(delta);
      HandleQuickSlotsInput();
      HandleInventoryInput();
      HandleLockOnInput();
      HandleTwoHandInput();
      HandleUseConsumableInput();
      HandleHoldRBInput();
      HandleFireBowInput();
    }
    private void HandleMoveInput(float delta)
    {
      if (playerManager.isHoldingArrow)
      {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical) / 2);
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
      }
      else
      {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
      }

    }

    private void HandleRollInput(float delta)
    {
      if (b_Input)
      {
        rollInputTimer += delta;
        if (playerStatsManager.currentStamina <= 0)
        {
          b_Input = false;
          sprintFlag = false;
        }
        if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
        {
          sprintFlag = true;
        }
      }
      else
      {
        sprintFlag = false;

        if (rollInputTimer > 0 && rollInputTimer < 0.5f)
        {
          rollFlag = true;
        }
        rollInputTimer = 0;
      }
    }

    private void HandleCombatInput(float delta)
    {
      if (rb_Input)
      {
        playerCombatManager.HandleRBAction();
      }
      if (rt_Input)
      {
        playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
      }



      if (lt_Input)
      {
        if (twoHandFlag)
        {
          // handle two handing weapon art
        }
        else
        {
          // handle normal weapon art
          playerCombatManager.HandleLTAction();
        }
      }
    }

    private void HandleLBInput()
    {
      if (playerManager.isInAir || playerManager.isSprinting || playerManager.isFiringSpell)
      {
        lb_Input = false;
        return;
      }

      if (lb_Input)
      {
        playerCombatManager.HandleLBAction();
      }
      else if (lb_Input == false)
      {
        playerManager.isBlocking = false;

        if (blockingCollider.blockingCollider.enabled)
        {
          blockingCollider.DisableBlockingCollider();
        }

        if (playerManager.isHoldingArrow)
        {
          // playerAnimatorManager.animator.SetBool("isHoldingArrow", false);
        }
      }
    }

    private void HandleQuickSlotsInput()
    {

      if (d_Pad_Right)
      {
        playerInventoryManager.ChangeRightWeapon();
      }
      else if (d_Pad_Left)
      {
        playerInventoryManager.ChangeLeftWeapon();
      }
    }


    private void HandleInventoryInput()
    {

      if (inventory_Input)
      {
        inventoryFlag = !inventoryFlag;

        if (inventoryFlag)
        {
          uiManager.OpenSelectWindow();
          uiManager.UpdateUI();
          uiManager.hudWindow.SetActive(false);
        }
        else
        {
          uiManager.CloseSelectWindow();
          uiManager.CloseAllInventoryWindows();
          uiManager.hudWindow.SetActive(true);
        }
      }
    }


    private void HandleLockOnInput()
    {
      if (lockOnInput && lockOnFlag == false)
      {
        lockOnInput = false;
        cameraHandler.HandleLockOn();
        if (cameraHandler.nearestLockOnTarget != null)
        {
          cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
          lockOnFlag = true;
        }
      }
      else if (lockOnInput && lockOnFlag)
      {
        lockOnInput = false;
        lockOnFlag = false;
        cameraHandler.ClearLockOnTargets();
      }

      if (lockOnFlag && right_Stick_Left_Input)
      {
        right_Stick_Left_Input = false;
        cameraHandler.HandleLockOn();
        if (cameraHandler.leftLockTarget != null)
        {
          cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
        }
      }
      if (lockOnFlag && right_Stick_Right_Input)
      {
        right_Stick_Right_Input = false;
        cameraHandler.HandleLockOn();
        if (cameraHandler.rightLockTarget != null)
        {
          cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
        }
      }

      cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
      if (y_Input)
      {
        y_Input = false;
        twoHandFlag = !twoHandFlag;

        if (twoHandFlag)
        {
          playerManager.isTwoHandingWeapon = true;
          playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
          playerWeaponSlotManager.LoadTwoHandIKTargets(true);
        }
        else
        {
          playerManager.isTwoHandingWeapon = false;
          playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
          playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
          playerWeaponSlotManager.LoadTwoHandIKTargets(false);
        }
      }
    }


    private void HandleHoldRBInput()
    {
      if (hold_rb_Input)
      {
        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
        {
          playerCombatManager.HandleHoldRBAction();
        }
        else
        {
          hold_rb_Input = false;
          playerCombatManager.AttemptBackStabOrRiposte();
        }
      }
    }

    private void HandleFireBowInput()
    {
      if (fireFlag)
      {
        if (playerManager.isHoldingArrow)
        {
          fireFlag = false;
          playerCombatManager.FireArrowAction();
        }
      }
    }


    private void HandleUseConsumableInput()
    {
      if (x_Input)
      {
        x_Input = false;
        playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);
      }
    }


  }
}