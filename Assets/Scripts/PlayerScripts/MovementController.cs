using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Miscellaneous References")]
    [SerializeField] Rigidbody _playerRb;
    [SerializeField] Transform _orientation;
    //SerializeField
    [Header("Movement Variables")]
    [SerializeField] float _groundWalkSpeed;
    [SerializeField] float _airWalkSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _groundDrag=6f;
    [SerializeField] float _airDrag=2f;
    [SerializeField] float _sprintMultiplier=1.2f;

    [Header("Mouse Variables")]
    [SerializeField] Transform _camRoot;
    [SerializeField] float _mouseSensitivity;
    [SerializeField] float _maxAngle=90f;

    [Header("Ground Variables")]
    [SerializeField] float _playerHeight;
    [SerializeField]LayerMask _ground;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _checkRadius=0.4f;
    [SerializeField] float _coyoteTime=0.1f;
    [SerializeField] float _gravity=20f;

    

    //Private Variables

    Vector2 _moveInput= Vector2.zero;
    Vector3 _moveDirection= Vector3.zero;
    float _xRot=0f;
    float _yRot=0f;
    bool _isGrounded=true;
    bool _jumpPressed=false;
    float _coyoteTimeCounter=0f;
    RaycastHit _slopeHit;
    bool _canCheckGround=true;
    bool _isSprinting=false;





    // GETSETS
    public Vector3 MoveInput{
        get{return _moveInput;}
        set{_moveInput=value;}
    }

    public Rigidbody PlayerRb{
        get{return _playerRb;}
    }

    public Vector3 MoveDirection{
        get{return _moveDirection;}
        set{_moveDirection=value;}
    }
    public Transform Orientation{
        get{return _orientation;}
    }
    public float GroundWalkSpeed{
        get{return _groundWalkSpeed;}
        set{_groundWalkSpeed=value;}
    }
    public float AirWalkSpeed{
        get{return _airWalkSpeed;}
        set{_airWalkSpeed=value;}
    }


    public float JumpForce{
        get{return _jumpForce;}
        set{_jumpForce=value;}
    }

    public float MouseSens{
        get{return _mouseSensitivity;}
        set{_mouseSensitivity=value;}
    }

    public float GroundDrag{
        get{return _groundDrag;}
        set{_groundDrag=value;}
    }
    public float AirDrag{
        get{return _airDrag;}
        set{_airDrag=value;}
    }
    public float XRot{
        get{return _xRot;}
        set{_xRot=value;}
    }
    public float YRot{
        get{return _yRot;}
        set{_yRot=value;}
    }
    public float MaxAngle{
        get{return _maxAngle;}
    }

    public Transform CamRoot{
        get{return _camRoot;}
    }
    public bool IsGrounded{
        get{return _isGrounded;}
        set{_isGrounded=value;}
    }

    public float PlayerHeight{
        get{return _playerHeight;}
    }

    public LayerMask GroundLayer{
        get{return _ground;}
    }
    public bool JumpPressed{
        get{return _jumpPressed;}
        set{_jumpPressed=value;}
    }

     public float CoyoteTime{
        get{return _coyoteTime;}
        set{_coyoteTime=value;}
    }
     public float CoyoteTimeCounter{
        get{return _coyoteTimeCounter;}
        set{_coyoteTimeCounter=value;}
    }
     public float Gravity{
        get{return _gravity;}
    }
    public float CheckRadius{
        get{return _checkRadius;}
    }

    public Transform GroundCheck{
        get{return _groundCheck;}
    }
    public bool IsSprinting{
        get{return _isSprinting;}
        set{_isSprinting=value;}
    }

    public float SprintMultiplier{
        get{return _sprintMultiplier;}
    }
    public bool OnSlope(){
        if(Physics.Raycast(transform.position,Vector3.down,out _slopeHit,PlayerHeight+0.3f))
        {
            if(_slopeHit.normal!=Vector3.up)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        else 
        return false;
    }
    
    


    private void Start() {
        PlayerRb.freezeRotation=true;
        
    }
    private void Update() {
        SetMoveDirection();
        if(_canCheckGround)
        CheckGrounded();
        Jump();
        
    }
    private void FixedUpdate() {
        MovePlayer();
        //Add Gravity
        
    }


    void SetMoveDirection()
    {
        MoveDirection=Orientation.forward*MoveInput.y+ Orientation.right*MoveInput.x;
        if(OnSlope() && IsGrounded)
        {
            MoveDirection=Vector3.ProjectOnPlane(MoveDirection,_slopeHit.normal);
        }
    }

    void MovePlayer()
    {
        if(IsSprinting)
        {
            if(IsGrounded)
            {
                PlayerRb.AddForce((MoveDirection.normalized*GroundWalkSpeed*SprintMultiplier)+Vector3.down*Gravity,ForceMode.Acceleration);
            }
            else
            {
                PlayerRb.AddForce((MoveDirection.normalized*AirWalkSpeed*SprintMultiplier)+Vector3.down*Gravity,ForceMode.Acceleration);
            }
        }
        else
        {
             if(IsGrounded)
            {
                PlayerRb.AddForce((MoveDirection.normalized*GroundWalkSpeed)+Vector3.down*Gravity,ForceMode.Acceleration);
            }
            else
            {
                PlayerRb.AddForce((MoveDirection.normalized*AirWalkSpeed)+Vector3.down*Gravity,ForceMode.Acceleration);
            }
        }
        
        
    }

    void SetDrag(float playerDrag)
    {
        PlayerRb.drag=playerDrag;
    }
    
    void CheckGrounded(){
        IsGrounded= Physics.CheckSphere(GroundCheck.position,CheckRadius,GroundLayer);
        if(IsGrounded)
        {
            CoyoteTimeCounter=CoyoteTime;
            SetDrag(GroundDrag);
        }
        else
        {
            CoyoteTimeCounter-=Time.deltaTime;
            SetDrag(AirDrag);
        }
    }

    void Jump()
    {
        if(CoyoteTimeCounter>0 && JumpPressed)
        {
            PlayerRb.velocity=new Vector3(PlayerRb.velocity.x,0f,PlayerRb.velocity.z);
            PlayerRb.AddForce(Orientation.up*JumpForce,ForceMode.Impulse);
            CoyoteTimeCounter=0f;
            SetDrag(AirDrag);
            IsGrounded=false;
            StartCoroutine(StopGroundCheck());

        }
    }

    IEnumerator StopGroundCheck()
    {
        _canCheckGround=false;
        yield return new WaitForSeconds(0.1f);
        _canCheckGround=true;
    }

    //LookControls
    public void Look(Vector2 mouseDir){
        float mouseX=mouseDir.x*MouseSens;
        float mouseY=mouseDir.y*MouseSens;

        Orientation.Rotate(Orientation.up*mouseX);

        YRot-=mouseY;
        XRot+=mouseX;
        YRot=Mathf.Clamp(YRot,-MaxAngle,MaxAngle);
        CamRoot.localRotation=Quaternion.Euler(YRot,XRot,0f);
    }

}
