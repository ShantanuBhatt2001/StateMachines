using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWalkState : MovementBaseState
{
    public MovementWalkState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory): base(currentContext,movementStateFactory) 
    {

    }

    public override void EnterState()
    {
        Debug.Log("Walk");
    }

  public override void UpdateState()
   {
       _ctx.SpeedMultiplier=_ctx.WalkMultiplier;
        CheckSwitchStates();
   }

   public override void ExitState(){}
   
   public override void CheckSwitchStates()
   {
        if(!_ctx.IsMoving() && !_ctx.IsSprinting)
        {
            SwitchState(_factory.Idle());
        }
        else if(_ctx.IsMoving() && _ctx.IsSprinting)
        {
            SwitchState(_factory.Run());
        }
   }

   public override void InitializeSubState(){}
}
