using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementJumpState :  MovementBaseState
{
    
    
    public MovementJumpState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory) : base(currentContext,movementStateFactory) 
    {
        
        InitializeSubState();
        _isRootState=true;
    }

    public override void EnterState()
    {
        _ctx.SetDrag(_ctx.AirDrag);
        _ctx.CurrentGravity=_ctx.Gravity;
        _ctx.IsGrounded=false;
        _ctx.CoyoteTimeCounter=0;
         _ctx.PlayerRb.velocity=new Vector3(_ctx.PlayerRb.velocity.x,0f,_ctx.PlayerRb.velocity.z);
        _ctx.PlayerRb.AddForce(_ctx.Orientation.up*_ctx.JumpForce,ForceMode.Impulse);
       _ctx.CallStopGroundCheck();
       
    }

   public override void UpdateState()
    {
        _ctx.MoveMagnitude=_ctx.AirSpeed;
        CheckSwitchStates();
    }

   public override void ExitState(){}
   
   public override void CheckSwitchStates()
    {
        if(_ctx.IsGrounded)
        {
            SwitchState(_factory.Grounded());
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

