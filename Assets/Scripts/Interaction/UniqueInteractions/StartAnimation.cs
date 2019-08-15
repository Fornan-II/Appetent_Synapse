using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public string AnimationVariableName = "InteractTrigger";
    public Animator TargetAnimator;

    public void TriggerAnimation(Pawn source)
    {
        if(TargetAnimator)
        {
            TargetAnimator.SetTrigger(AnimationVariableName);
        }
    }
}
