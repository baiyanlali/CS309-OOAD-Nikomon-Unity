using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingSMB :StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.applyRootMotion = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.applyRootMotion = true;
    }
}
