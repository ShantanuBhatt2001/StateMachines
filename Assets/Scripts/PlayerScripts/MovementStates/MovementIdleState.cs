using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIdleState : MovementBaseState
{
    public MovementIdleState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory): base(currentContext,movementStateFactory) 
    {
        
    }

    public override void EnterState()
    {
       _ctx.MoveDirection= new Vector2(0f,0f);
       Debug.Log("Idle");
    }

    public override void UpdateState()
    {
        
        CheckSwitchStates();
    }

    public override void ExitState(){}
   
    public override void CheckSwitchStates()
    {
        if(_ctx.IsMoving() && !_ctx.IsSprinting)
        {
            SwitchState(_factory.Walk());
        }
        else if(_ctx.IsMoving() && _ctx.IsSprinting)
        {
            SwitchState(_factory.Run());
        }
    }

    public override void InitializeSubState(){}

}
