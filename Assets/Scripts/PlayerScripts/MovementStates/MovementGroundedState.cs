using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGroundedState : MovementBaseState
{
    public MovementGroundedState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory): base(currentContext,movementStateFactory) 
    {
        InitializeSubState();
        _isRootState=true;
        
    }

    public override void EnterState()
    {
        _ctx.SetDrag(_ctx.GroundDrag);
         _ctx.CoyoteTimeCounter=_ctx.CoyoteTime;
         _ctx.CurrentGravity=0f;
    }

    public override void UpdateState()
    {
        _ctx.MoveMagnitude=_ctx.GroundSpeed;
        CheckSwitchStates();
        if(_ctx.OnSlope())
        {
            _ctx.MoveDirection=Vector3.ProjectOnPlane(_ctx.MoveDirection,_ctx.SlopeHit.normal);
        }
    }

    public override void ExitState(){}
   
    public override void CheckSwitchStates()
    {
        if(_ctx.JumpPressed)
        {
            SwitchState(_factory.Jump());
        }
        if(!_ctx.IsGrounded)
        {;
            SwitchState(_factory.Fall());
        }
    }

    public override void InitializeSubState()
    {
        if(!_ctx.IsMoving() )
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
