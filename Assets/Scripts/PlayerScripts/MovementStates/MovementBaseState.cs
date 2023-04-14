public abstract class MovementBaseState 
{
    protected bool _isRootState=false;
    protected MovementStateMachine _ctx;
    protected MovementStateFactory _factory;
    protected MovementBaseState _currentSubState;
    protected MovementBaseState _currentSuperState;

    public MovementBaseState(MovementStateMachine currentContext,MovementStateFactory movementStateFactory)
    {
        _ctx=currentContext;
        _factory=movementStateFactory;
    }

   public abstract void EnterState();

   public abstract void UpdateState();

   public abstract void ExitState();
   
   public abstract void CheckSwitchStates();

   public abstract void InitializeSubState();

   public void UpdateStates()
   {
        UpdateState();
        if(_currentSubState!=null)
        {
            _currentSubState.UpdateState();
        }
   }
   public void ExitStates()
   {
        ExitState();
        if(_currentSubState!=null)
        {
            _currentSubState.ExitState();
        }
   }

   protected void SwitchState(MovementBaseState newState)
   {
        ExitStates();
         newState.EnterState();
        if(_isRootState)
         _ctx.CurrentState=newState;
         else if(_currentSuperState!=null)
         {
            _currentSuperState.SetSubState(newState);
         }
   }
   protected void SetSuperState(MovementBaseState newSuperState)
   {
        _currentSuperState=newSuperState;
        _isRootState=false;
   }
   protected void SetSubState(MovementBaseState newSubState)
   {
        _currentSubState=newSubState;
        newSubState.SetSuperState(this);
   }

}
