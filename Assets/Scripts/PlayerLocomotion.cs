using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    Transform cameraObject;
    InputManager inputManager;
    Vector3 moveeDirection;

    AnimatorManager animatorManager;
    PlayerAttacker playerAttacker;

    public Rigidbody playerRigiBody;

    [Header("Ground & Air Detection Stats")]

    [SerializeField]
    float GroundedOffSet = 0.5f;
    float GroundedRadius = 0.2f;
    public LayerMask groundLayer;

    public float inAirTimer;
    public float inRollingTime;
    public float fallingSpeed = 45;


    public float leapingVelocity = 3;

    [Header("Movement Stats")]

    [SerializeField]
    float walkingSpeed = 1.5f;
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float sprintSpeed = 17;

    [SerializeField]
    float rotationSpeed = 15;
    [SerializeField]

    [Header("Movements Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    public bool isAimMode;
    public bool isLockOn;

    [Header("Jump Stats")]
    public float jumpHeight = 1.2f;
    public float gravityIntensity = -15.0f;
    public float JumpTimeout = 0.50f;

    [Header("Dash Stats")]
    public float DashDuration = 0.25f;
    public float DashForce = 5f;

    public LayerMask dashLayers;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigiBody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        cameraObject = Camera.main.transform;


    }

    #region Movement

    public void HandleAllMovement()
    {

        HandleFalling();
        GroundCheck();

        if (playerManager.isInteracting || isJumping)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (isJumping || inAirTimer > 0)
        {
            return;
        }
        if (isAimMode || isLockOn)
        {
            Vector3 rotationDirection = moveeDirection;
            rotationDirection = cameraObject.transform.forward;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;

        }
        else
        {
            Vector3 targetDir = Vector3.zero;
            targetDir = cameraObject.forward * inputManager.vertical;
            targetDir += cameraObject.right * inputManager.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    private void HandleMovement()
    {
        if (isJumping || inAirTimer > 0 || !isGrounded)
        {
            return;
        }
        moveeDirection = cameraObject.forward * inputManager.vertical;
        moveeDirection += cameraObject.right * inputManager.horizontal;

        moveeDirection.Normalize();
        moveeDirection.y = 0;
            Vector3 originPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 dashPosition = originPosition + moveeDirection * DashForce;
            Debug.DrawRay(originPosition,dashPosition);

        if (isSprinting && inputManager.moveAmount > 0.5f)
        {
            isAimMode = false;
            moveeDirection = moveeDirection * sprintSpeed;
        }
        else
        {

            if (inputManager.moveAmount >= 0.5f)
            {
                moveeDirection = moveeDirection * movementSpeed;
            }
            else
            {
                moveeDirection = moveeDirection * walkingSpeed;
            }
        }

        if (isGrounded && !isJumping && inAirTimer == 0)
        {
            Vector3 projectedVelocity = moveeDirection;
            playerRigiBody.velocity = projectedVelocity;
        }

    }

    public void HandleRolling()
    {
        if (animatorManager.anim.GetBool("IsInteracting"))
        {
            return;
        }
        if (isJumping)
        {
            return;
        }

        if (inputManager.moveAmount > 0)
        {
            animatorManager.PlayTargetAnimation("Rolling", true);
        }



    }

    public void HandleBackStep()
    {
        if (animatorManager.anim.GetBool("IsInteracting"))
        {
            return;
        }
        if (isJumping)
        {
            return;
        }

        animatorManager.PlayTargetAnimation("backstep", true, true);
    }

    private void HandleFalling()
    {
        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true, true);
            }
            animatorManager.anim.SetBool("IsUsingRootMotion", false);
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigiBody.AddForce(transform.forward * leapingVelocity);
            playerRigiBody.AddForce(Vector3.down * fallingSpeed * inAirTimer);
        }
    }

    public void GroundCheck()
    {
        RaycastHit hit;
        Vector3 targetposition;
        targetposition = transform.position;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + GroundedOffSet, transform.position.z);
       
        if (Physics.SphereCast(spherePosition, GroundedRadius, Vector3.down, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);

            }
            Vector3 HitPoint = hit.point;
            targetposition.y = HitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
            playerAttacker.canJumpAttack = false;
        }
        else
        {
            isGrounded = false;
        }


        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetposition;
            }
        }
    }

    public void HandleJumping()
    {
        if (isGrounded && !playerManager.isInteracting && !isJumping)
        {
            isGrounded = false;
            animatorManager.anim.SetBool("IsJumping", true);
            float jumpingVelocity = Mathf.Sqrt(-2f * gravityIntensity * jumpHeight);
            //Vector3 playerVelocity = moveeDirection;
            //playerVelocity.y = jumpingVelocity;
            //playerRigiBody.velocity = playerVelocity;
            playerRigiBody.AddForce(Vector3.up * jumpingVelocity,ForceMode.Impulse);
            animatorManager.PlayTargetAnimation("Jump", true);
            playerAttacker.canJumpAttack = true;
        }

    }

    public void HandleDash()
    {
        if (isGrounded && !playerManager.isInteracting && !isJumping)
        {
            moveeDirection = cameraObject.forward * inputManager.vertical;
            moveeDirection += cameraObject.right * inputManager.horizontal;
            moveeDirection.Normalize();
            moveeDirection.y = 0;
            Vector3 originPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 dashPosition = originPosition + moveeDirection * DashForce;
            //if (Physics.Raycast(originPosition, dashPosition, out RaycastHit hit, dashLayers))
            //{
               // dashPosition = hit.point;
            //}
            if(Physics.SphereCast(originPosition,0.3f,dashPosition,out RaycastHit hit,1f,dashLayers)){
                dashPosition = hit.point;
            }

            //playerRigiBody.MovePosition(dashPosition);
            playerRigiBody.AddForce(dashPosition, ForceMode.Impulse);
            playerRigiBody.velocity = Vector3.zero;
        }
    }




    #endregion
}


