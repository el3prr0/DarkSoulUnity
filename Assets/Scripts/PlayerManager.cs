using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;

    Animator anim;
    CameraManager cameraManager;
    CinemaCameraManager cinemaCameraManager;
    PlayerLocomotion playerLocomotion;
    PlayerAttacker playerAttacker;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isUsingRootMotion;

    public bool isAimMode;

    private void Awake()
    {
        //cameraManager = FindObjectOfType<CameraManager>();
        cinemaCameraManager = FindObjectOfType<CinemaCameraManager>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
        anim = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {

        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        //cameraManager.HandleAllCameraMovement();

        cinemaCameraManager.CameraRotation();
        isInteracting = anim.GetBool("IsInteracting");
        isUsingRootMotion = anim.GetBool("IsUsingRootMotion");
        playerLocomotion.isJumping = anim.GetBool("IsJumping");
        playerAttacker.CanCombo = anim.GetBool("CanCombo");
        anim.SetBool("IsGrounded",playerLocomotion.isGrounded);
    }
}
