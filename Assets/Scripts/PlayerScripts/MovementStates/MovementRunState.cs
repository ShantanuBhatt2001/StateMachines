using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRunState : MovementBaseState
{
    public MovementRunState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory): base(currentContext,movementStateFactory) 
    {

    }

    public override void EnterState()
    {
        Debug.Log("Run");
    }

   public override void UpdateState()
   {
        _ctx.SpeedMultiplier=_ctx.SprintMultiplier;
        CheckSwitchStates();
   }

   public override void ExitState(){}
   
   public override void CheckSwitchStates()
   {
        if(!_ctx.IsMoving() && !_ctx.IsSprinting)
            {
                SwitchState(_factory.Idle());
            }
            else if(_ctx.IsMoving() && !_ctx.IsSprinting)
            {
                SwitchState(_factory.Walk());
            }
   }

   public override void InitializeSubState(){}
}
