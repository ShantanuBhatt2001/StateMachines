using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BasicWeapons : GunScript{
    
    [SerializeField]GameObject _bulletPrefab;
    [SerializeField] GameObject _hitMarker;
    [SerializeField] Image _bulletCounter;
    [SerializeField] TextMeshProUGUI _bulletNumber;
    GameObject tempTrail;
    
    private void Start() {
            _bulletCounter.fillAmount=((float)_currentBullets/(float)_maxBullets);
            _bulletNumber.text=_currentBullets.ToString();
    }

    public override void Shoot()
    {
        if(_isShooting && _canFire && !_isReloading && _currentBullets>0 && !_isSwitching)
        {
            
            _currentBullets--;
            _bulletCounter.fillAmount=((float)_currentBullets/(float)_maxBullets);
            _bulletNumber.text=_currentBullets.ToString();
            _canFire=false;
            StartCoroutine(ResetFire());
            if(Physics.Raycast(_camRoot.position,_camRoot.forward,out RaycastHit hitInfo,_range))
            {
                StartCoroutine(ShootTrail(hitInfo.point));
                GameObject hitMarker=Instantiate(_hitMarker,hitInfo.point,Quaternion.identity);
                hitMarker.transform.forward=hitInfo.normal;
                Debug.Log(hitInfo.collider.name);
            }
            else
            StartCoroutine(ShootTrail(_firePoint.position+(_firePoint.forward*_range)));
            Recoil();
        }
    }
    public override void AlternateShoot()
    {
        
    }
    public override void Recoil()
    {
        gunController._targetRotation+=new Vector3(_recoilX,Random.Range(-_recoilY,_recoilY),Random.Range(-_recoilZ,_recoilZ));
    }

    public override void Reload()
    {
        if(_reloadInput && !_isReloading && _currentBullets<_maxBullets && !_isSwitching)
        {
            StartCoroutine(StartReload());
        }
    }

    public override IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(60/_rpm);
        _canFire=true;

    }

    public IEnumerator StartReload()
    {
        _isReloading=true;
        float Timer=0;
        _bulletNumber.text="--";
        while(Timer<_reloadTime)
        {
            Timer+=Time.deltaTime;
            _bulletCounter.fillAmount=Timer/_reloadTime;
            yield return null;
        }
        
        _currentBullets=_maxBullets;
        _bulletNumber.text=_currentBullets.ToString();
        _isReloading=false;
    }


    IEnumerator ShootTrail(Vector3 hitPoint)
    {
        GameObject tempTrail=GameObject.Instantiate(_bulletPrefab,_firePoint.position,Quaternion.identity);
        Vector3 startPos=_firePoint.position;
        float travelTime=Vector3.Magnitude(hitPoint-startPos)/_bulletSpeed;
        float time=0;
        while(time<1)
        {
            
            tempTrail.transform.position=Vector3.Lerp(startPos,hitPoint,time);
            
            time+=Time.deltaTime/travelTime;
            yield return null;
        }
        tempTrail.transform.position=hitPoint;
        
        Destroy(tempTrail,tempTrail.GetComponent<TrailRenderer>().time);
    }


    public override IEnumerator SwitchFromGun(float i)
    {
        Debug.Log("Recieved  SwitchFromGun"+i);
        if(_isSwitching)
        yield break;
        _isSwitching=true;
        yield return new WaitForSeconds(_switchTime);
        _isSwitching=false;
        gunController.ChangeWeapon(i);
        gameObject.SetActive(false);
        
    }
    public override IEnumerator SwitchToGun()
    {
        gameObject.SetActive(true);
        gunController.CurrentGun=this;
        _isSwitching=true;
        yield return new WaitForSeconds(_switchTime);
        _isSwitching=false;
    }
}
