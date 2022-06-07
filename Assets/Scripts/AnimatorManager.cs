using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;

    PlayerManager playerManager;
    int vertical;
    int horizontal;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }

        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }
       
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);

    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool useRootMotion = false)
    {
        anim.SetBool("IsInteracting", isInteracting);
        anim.SetBool("IsUsingRootMotion", useRootMotion);
        anim.CrossFade(targetAnim, 0.2f);
    }

    private void OnAnimatorMove() {
        
        if(playerManager.isUsingRootMotion){
            playerLocomotion.playerRigiBody.drag  = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            playerLocomotion.playerRigiBody.velocity = velocity;
        }
    }

    public void EnableCombo(){
        anim.SetBool("CanCombo",true);
    }

    public void DisableCombo(){
        anim.SetBool("CanCombo",false);
    }

}

