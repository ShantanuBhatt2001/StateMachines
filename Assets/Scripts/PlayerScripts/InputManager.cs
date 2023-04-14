using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Scripts References")]
    [SerializeField] MovementStateMachine _movementController;
    [SerializeField] GunController _gunController;
    [Header("InputKeys")]
    [SerializeField] KeyCode _jump;
    [SerializeField] KeyCode _sprint;
    [SerializeField] KeyCode _reload;

    [Header("Miscellaneous")]
    [SerializeField] float _jumpSlack=0.1f;


    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetLookInput();
        GetGunsInput();
        //JumpInput

        if(Input.GetKeyDown(_jump))
        {
            StartCoroutine(JumpPlayer());
        }
    }


    void GetMovementInput()
    {
        float inputX=Input.GetAxisRaw("Horizontal");
        float inputY=Input.GetAxisRaw("Vertical");
        if(Input.GetKey(_sprint))
        {
            _movementController.IsSprinting=true;
        }
        else 
        _movementController.IsSprinting=false;
        _movementController.MoveInput= new Vector2(inputX,inputY);

    }
   void GetLookInput()
    {
        float mouseX=Input.GetAxis("Mouse X");
        float mouseY=Input.GetAxis("Mouse Y");
        Vector2 _mouseInput= new Vector2(mouseX,mouseY);
        _movementController.Look(_mouseInput);
        _gunController.Sway(_mouseInput);
    }

    void GetGunsInput()
    {
        //ShootInput
        if(_gunController.CurrentGun._singleShot)_gunController.CurrentGun._isShooting=Input.GetKeyDown(KeyCode.Mouse0);
        else _gunController.CurrentGun._isShooting=Input.GetKey(KeyCode.Mouse0);

        
        _gunController.CurrentGun._isAlternateShooting=Input.GetKey(KeyCode.Mouse1);
        _gunController.CurrentGun._reloadInput=Input.GetKeyDown(_reload);
        
        

        if(Input.mouseScrollDelta.y!=0)
        {
            Debug.Log("called Switch");
            _gunController.SwitchGuns(Input.mouseScrollDelta.y);
        }
        
    }

    IEnumerator JumpPlayer()
    {
            _movementController.JumpPressed=true;
            yield return new WaitForSeconds(_jumpSlack);
            _movementController.JumpPressed=false;
    }
}
