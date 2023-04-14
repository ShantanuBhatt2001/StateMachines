using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
   [Header("References")]
   [SerializeField] GunScript[] _guns;
   [SerializeField] Transform _camRoot;
   [SerializeField] Transform _recoilRoot;
   [SerializeField] Transform _weaponRoot;
   [Header("Gun Sway")]
   [SerializeField] float _smoothness;
   [SerializeField] float _swayMultiplier;
   [HideInInspector]
   public Vector3 _currentRotation;
   [HideInInspector]
   public Vector3 _targetRotation;
   


   
   
   GunScript _currentGun;
   int _currentGunIndex=0;
   
   

    public GunScript CurrentGun{get{return _currentGun;}set{_currentGun=value;}}

   private void Start() 
{
        _currentGun=_guns[0];
        foreach(GunScript gun in _guns)
        {
            gun._camRoot=_recoilRoot;
            gun.gunController=this;
            
        }
   }


   void Update()
   {
        _currentGun.Shoot();
        _currentGun.Reload();
        _currentGun.AlternateShoot();
        _targetRotation=Vector3.Lerp(_targetRotation,Vector3.zero,CurrentGun._returnSpeed*Time.deltaTime);
        _currentRotation=Vector3.Slerp(_currentRotation,_targetRotation,CurrentGun._snappiness*Time.fixedDeltaTime);
        _recoilRoot.localRotation=Quaternion.Euler(_currentRotation*10);
        
   }

   public void SwitchGuns(float i)
   {
      Debug.Log("Recieved  Switch call");
      StartCoroutine(CurrentGun.SwitchFromGun(i));
   }

   public void ChangeWeapon(float i)
   {
      Debug.Log("Recieved  ChangeWeapon call");
      if(i<0)
      {
         if(_currentGunIndex==0)
         {
            
            _currentGunIndex=_guns.Length-1;
         }
         else
         _currentGunIndex--;
      }
      else if(i>0)
      {
         if(_currentGunIndex==_guns.Length-1)
         {
            _currentGunIndex=0;

         }
         else
         {
            _currentGunIndex++;
         }
      }
      Debug.Log(_currentGunIndex);
      CurrentGun=_guns[_currentGunIndex];
      StartCoroutine(CurrentGun.SwitchToGun());
      
   }

   public void Sway(Vector2 input)
   {
      input=input*_swayMultiplier;
      Quaternion rx=Quaternion.AngleAxis(-input.y,Vector3.right);
      Quaternion ry=Quaternion.AngleAxis(input.x,Vector3.up);
      Quaternion targetRotation=rx*ry;
      _weaponRoot.localRotation=Quaternion.Slerp(_weaponRoot.localRotation,targetRotation,_smoothness*Time.deltaTime);

   }
}
