using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float cameraInputX;
    public float cameraInputY;

    public Vector3 mouseWorldPosition;

    public float sqrMagnitude;

    public bool b_input;
    public bool jump_input;

    public bool interactible_input;

    public bool rightLightAttack_input;
    public bool rightLeftHeavyAttack_input;

    public bool block_input;

    public bool d_pad_Up;
    public bool d_pad_Down;
    public bool d_pad_Right;
    public bool d_pad_Left;

    public bool aim_mode = false;

    public bool lockon = false;

    public bool loadWeaponAimMode = false;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;



    PlayerControl inputActions;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    PlayerAttacker playerAttacker;

    PlayerInventory playerInventory;
    CinemaCameraManager cameraManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CinemaCameraManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
    }


    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControl();
            inputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.Roll.performed += i => b_input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_input = false;
            inputActions.PlayerActions.Jump.performed += i => jump_input = true;
            inputActions.PlayerActions.LightAttack.performed += i => rightLightAttack_input = true;
            inputActions.PlayerActions.HeavyAttack.performed += i => rightLeftHeavyAttack_input = true;
            inputActions.PlayerActions.LeftAction.performed += i => block_input = true;
            inputActions.PlayerActions.LeftAction.canceled += i => block_input = false;
            inputActions.PlayerInventory.DPadUp.performed += i => d_pad_Up = true;
            inputActions.PlayerInventory.DPadDown.performed += i => d_pad_Down = true;
            inputActions.PlayerInventory.DPadRight.performed += i => d_pad_Right = true;
            inputActions.PlayerInventory.DPadLeft.performed += i => d_pad_Left = true;
            inputActions.PlayerActions.AimMode.performed += i => aim_mode = true;
            inputActions.PlayerActions.AimMode.canceled += i => aim_mode = false;
            inputActions.PlayerActions.lockon.performed += i => lockon = true;
            inputActions.PlayerActions.Interactable.performed += i => interactible_input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    public void HandleAllInputs()
    {
        HandleAimMode();
        HandleMoveInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleRightHeavyAttack();
        HandleRightLightAttack();
        //HandleBlockAttack();
        LeftAction();
        HandleQuickSlotsInput();
        HandleLockOn();
        HandleActionInput();
    }

    private void HandleMoveInput()
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        if (aim_mode || playerLocomotion.isLockOn)
        {
            animatorManager.UpdateAnimatorValues(vertical, horizontal, playerLocomotion.isSprinting);
        }
        else
        {
            animatorManager.UpdateAnimatorValues(moveAmount, 0, playerLocomotion.isSprinting);
        }



        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        sqrMagnitude = cameraInput.sqrMagnitude;
    }

    private void HandleSprintingInput()
    {
        if (aim_mode)
        {
            b_input = false;
            playerLocomotion.isSprinting = false;
            return;
        }

        if (b_input)
        {
            if (playerLocomotion.isLockOn && vertical <= 0)
            {
                playerLocomotion.HandleDash();
                b_input = false;
                return;
            }
            playerLocomotion.isSprinting = true;
            playerLocomotion.inRollingTime += Time.deltaTime;
        }
        else
        {
            playerLocomotion.isSprinting = false;
            if (playerLocomotion.inRollingTime > 0 && playerLocomotion.inRollingTime < 0.5f)
            {
                if (!playerLocomotion.isLockOn)
                {
                    playerLocomotion.HandleRolling();
                }
                b_input = false;
            }

            playerLocomotion.inRollingTime = 0;
        }

        if (b_input && moveAmount == 0)
        {
            b_input = false;
            playerLocomotion.HandleBackStep();
        }

    }

    private void HandleJumpingInput()
    {
        if (aim_mode)
        {
            jump_input = false;
            return;
        }
        if (jump_input)
        {
            jump_input = false;
            playerLocomotion.HandleJumping();

        }
    }
    private void HandleRightLightAttack()
    {
        if (rightLightAttack_input)
        {
            if (aim_mode)
            {

                playerAttacker.CastMagic(playerInventory.magic);
                rightLightAttack_input = false;
                playerInventory.LoadWeaponAfterMagic();
            }
            else
            {
                if (playerAttacker.CanCombo)
                {
                    playerAttacker.comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    playerAttacker.comboFlag = false;
                }
                else
                {

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
                if ((playerLocomotion.isJumping || playerLocomotion.inAirTimer > 0) && playerAttacker.canJumpAttack)
                {
                    if (playerLocomotion.isSprinting)
                    {
                        playerAttacker.HandleRunningJumpingAttack(playerInventory.rightWeapon);
                    }
                    else
                    {

                        playerAttacker.HandleJumpingAttack(playerInventory.rightWeapon);
                    }

                }
                rightLightAttack_input = false;

            }
        }
    }

    private void HandleRightHeavyAttack()
    {
        if (rightLeftHeavyAttack_input)
        {
            if (aim_mode)
            {

            }
            else
            {
                playerAttacker.HandleHeavyRightAttack(playerInventory.rightWeapon);
            }
            rightLeftHeavyAttack_input = false;

        }



    }

    private void HandleBlockAttack()
    {
        if (aim_mode)
        {
            block_input = false;
            return;
        }
        if (block_input)
        {

            playerAttacker.Parry += Time.deltaTime;

            if (playerAttacker.Parry > .2f)
            {
                if (playerInventory.leftWeapon.type == EnumWeaponTypes.unarmed && playerInventory.rightWeapon.type != EnumWeaponTypes.unarmed)
                {
                    playerAttacker.HandleBlockAction(playerInventory.rightWeapon);
                }
                else
                {
                    playerAttacker.LeftAttak(playerInventory.leftWeapon);
                }
            }
        }
        else
        {
            if (playerAttacker.Parry > 0f && playerAttacker.Parry < 0.5f)
            {
                playerAttacker.HandleParry();
            }
            playerAttacker.Parry = 0f;
        }
    }

    private void LeftAction()
    {
        if (aim_mode)
        {
            block_input = false;
            return;
        }
        if (block_input)
        {
            playerAttacker.Parry += Time.deltaTime;
            if (playerAttacker.Parry > .2f)
            {
                if (playerInventory.leftWeapon.type == EnumWeaponTypes.unarmed && playerInventory.rightWeapon.type != EnumWeaponTypes.unarmed)
                {
                    playerAttacker.HandleBlockAction(playerInventory.rightWeapon);
                }
                else
                {
                    playerAttacker.HandleBlockAction(playerInventory.leftWeapon);
                }
            }
        }
        else
        {
            if (playerAttacker.Parry > 0f && playerAttacker.Parry < 0.5f)
            {
                if (playerInventory.leftWeapon.type == EnumWeaponTypes.shield)
                {
                    playerAttacker.HandleParry();
                }
                else
                {
                    playerAttacker.LeftAttak(playerInventory.leftWeapon);
                }
            }
            playerAttacker.Parry = 0f;
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (aim_mode)
        {
            d_pad_Left = false;
            d_pad_Right = false;
            return;
        }
        if (d_pad_Right)
        {
            d_pad_Right = false;
            playerInventory.ChangeRightWeapon(1);
        }

        if (d_pad_Left)
        {
            d_pad_Left = false;
            playerInventory.ChangeLeftWeapon(1);
        }

        if (d_pad_Up)
        {
            d_pad_Up = false;
            playerInventory.ChangeMagic();
        }

    }

    private void HandleAimMode()
    {
        playerLocomotion.isAimMode = aim_mode;
        if (aim_mode)
        {
            b_input = false;
            lockon = false;
            playerLocomotion.isSprinting = false;
            cameraManager.AimMode(aim_mode);
            playerInventory.ChangeToAimMode();
            loadWeaponAimMode = true;
        }
        else
        {
            if (loadWeaponAimMode)
            {
                playerInventory.LoadWeaponAfterMagic();
            }
            cameraManager.AimMode(aim_mode);
            loadWeaponAimMode = false;
        }

    }

    private void HandleLockOn()
    {
        if (lockon)
        {
            if (playerLocomotion.isLockOn)
            {
                playerLocomotion.isLockOn = false;
            }
            else
            {
                playerLocomotion.isLockOn = true;
            }
            lockon = false;
        }
    }

    private void HandleActionInput()
    {

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out RaycastHit hit, 1f, cameraManager.aimColladerMask))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactaleText = interactableObject.interactableText;

                    if (interactible_input)
                    {
                        interactible_input = false;
                        interactableObject.Interact(GetComponent<PlayerManager>());
                    }
                }
            }
        }
    }
}



