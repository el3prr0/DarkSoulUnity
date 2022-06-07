using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetisInteracting : StateMachineBehaviour
{
    public string[] ResetObjects = new string[4];

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(var obj in ResetObjects){
            animator.SetBool(obj,false);
        }

    }
}