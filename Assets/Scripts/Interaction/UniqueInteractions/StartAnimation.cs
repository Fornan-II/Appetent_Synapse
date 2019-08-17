using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public enum AnimVarType
    {
        TRIGGER,
        BOOL
    }

    public string AnimationVariableName = "InteractTrigger";
    public AnimVarType AnimationVariableType = AnimVarType.TRIGGER;
    public Animator TargetAnimator;

    public void TriggerAnimation(Pawn source)
    {
        if(TargetAnimator)
        {
            if(AnimationVariableType == AnimVarType.TRIGGER)
            {
                TargetAnimator.SetTrigger(AnimationVariableName);
            }
            else if(AnimationVariableType == AnimVarType.BOOL)
            {
                bool value = TargetAnimator.GetBool(AnimationVariableName);
                TargetAnimator.SetBool(AnimationVariableName, !value);
            }
        }
    }
}
