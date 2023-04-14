using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunScript : MonoBehaviour
{
    [Header("FIRE VARIABLES")]
    public float _rpm;
    public int _maxBullets;
    public int _currentBullets;

    [Header("GUN VARIABLES")]
    public float _reloadTime;
    public float _switchTime;
    public float _range;
     public float _recoilX;
    public float _recoilY;
    public float _recoilZ;
    
    public float _snappiness;
    public float _returnSpeed;
    public float _bulletSpeed;
   
   [Header("STATE VARIABLES")]
    public bool _canFire;
    public bool _reloadInput;
    public bool _isReloading;
    public bool _singleShot;
    public bool _isShooting;
    public bool _isAlternateShooting;
    public bool _isSwitching;
    [Header("REFERENCES")]
    public Transform _firePoint;
    public Transform _camRoot;
    

    public LayerMask _enemyLayer;
    public GunController gunController;



    public abstract void Shoot();
    public abstract void AlternateShoot();
    public abstract void Reload();
    public abstract void Recoil();
    public abstract IEnumerator ResetFire();
    public abstract  IEnumerator SwitchFromGun(float i);
    public abstract IEnumerator SwitchToGun();

    private void OnDisable() {
        _isShooting=false;
        _isReloading=false;
        _isAlternateShooting=false;
        _isSwitching=false;
        _canFire=true;
        _reloadInput=false;
    }
 
    

}
