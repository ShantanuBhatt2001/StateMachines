public class MovementStateFactory 
{
    MovementStateMachine _context;

    public MovementStateFactory(MovementStateMachine currentContext)
    {
        _context=currentContext;
    }


    public MovementBaseState Idle()
    {
        return new MovementIdleState(_context,this);
    }

    public MovementBaseState Walk()
    {
        return new MovementWalkState(_context,this);
    }

    public MovementBaseState Run()
    {
        return new MovementRunState(_context,this);
    }

    public MovementBaseState Jump()
    {
        return new MovementJumpState(_context,this);
    }
    public MovementBaseState Grounded()
    {
        return new MovementGroundedState(_context,this);
    }
    public MovementBaseState Fall()
    {
        return new MovementFallState(_context,this);
    }

}