using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFallState : MovementBaseState
{
    public MovementFallState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory) : base(currentContext,movementStateFactory) 
    {
        
        InitializeSubState();
        _isRootState=true;
    }

    public override void EnterState()
    {
        
        _ctx.SetDrag(_ctx.AirDrag);
        _ctx.CurrentGravity=_ctx.Gravity;
        
    }

   public override void UpdateState()
   {
     //GravitySet
        _ctx.MoveMagnitude=_ctx.AirSpeed;
        _ctx.CoyoteTimeCounter-=Time.deltaTime;
        CheckSwitchStates();

   }

   public override void ExitState(){}
   
   public override void CheckSwitchStates()
    {
        if(_ctx.IsGrounded)
        {
            SwitchState(_factory.Grounded());
        }
        if(_ctx.JumpPressed && _ctx.CoyoteTimeCounter>0f)
        {
            SwitchState(_factory.Jump());
        }
    }

   public override void InitializeSubState()
   {
        if(!_ctx.IsMoving() && !_ctx.IsSprinting)
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMoving() && !_ctx.IsSprinting)
        {
            SetSubState(_factory.Walk());
        }
        else if(_ctx.IsMoving() && _ctx.IsSprinting)
        {
            SetSubState(_factory.Run());
        }
   } 
}
