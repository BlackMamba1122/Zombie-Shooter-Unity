using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public bool IsActiveWeapon;
    public int WeaponDamage;
    public bool isShooting, readyToShoot;
    bool allowReser = true;
    public float shootingDelay = 2f;
    public int bulletPerBurst = 3;
    public int BurstBulletLeft;

    private float spreadSensitivity;
    public float hipSpreadSensitivity;
    public float adsSpreadSensitivity;
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;
    public enum WeaponModel
    {
        Pistol,
        ak
    }
    public WeaponModel thisWeapnModel;
    public enum ShootingMode
    {
        SIngle,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        BurstBulletLeft = bulletPerBurst;
        animator = GetComponent<Animator>();
        BulletsLeft=MagSize;
        spreadSensitivity = hipSpreadSensitivity;
    }
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity=30f;
    public float bulletPrefabLifeTime = 3f;
    public GameObject MuzzleEffect;
    internal Animator animator;

    public float reloadTime;
    public int MagSize,BulletsLeft;
    public bool isReloading;
    bool isAds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(IsActiveWeapon)
        {

            GetComponent<Outline>().enabled = false;
            if(Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("enterAds");
                isAds=true;
                HUDManager.Instance.middleDot.SetActive(false);
                spreadSensitivity = adsSpreadSensitivity;
            }
            if(Input.GetMouseButtonUp(1))
            {
                animator.SetTrigger("exitAds");
                isAds=false;
                HUDManager.Instance.middleDot.SetActive(true);
                spreadSensitivity= hipSpreadSensitivity;
            }
        if(BulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.EmptySoundak.Play();
        }
        if(currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == ShootingMode.SIngle || currentShootingMode == ShootingMode.Burst)
        {
            isShooting= Input.GetKeyDown(KeyCode.Mouse0);
        }
        if(Input.GetKeyDown(KeyCode.R) && BulletsLeft<MagSize && isReloading==false && WeaponManager.Instance.checkAmmoLeft(thisWeapnModel) >0)
        {
            Reload();
        }
        // if(readyToShoot && isShooting==false && BulletsLeft<=0 && isReloading==false && checkAmmoLeft(thisWeapnModel) >0)
        // {
        //     Reload();
        // }
        if(readyToShoot && isShooting && BulletsLeft>0 && isReloading==false )
        {
            BurstBulletLeft= bulletPerBurst;
            FireWeapn();
        }
        }
    }

    private void FireWeapn()
    {
        BulletsLeft--;
        MuzzleEffect.GetComponent<ParticleSystem>().Play();
        if(isAds)
        {
            animator.SetTrigger("RecoilAds");
        }
        else
        {
            animator.SetTrigger("Recoil");
        }
        SoundManager.Instance.PlayShootingSound(thisWeapnModel);
        readyToShoot= false;
        Vector3 shootingDirection = calculateDiretionandSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        BulletScript bul=bullet.GetComponent<BulletScript>();
        bul.bulletDamage = WeaponDamage;
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));
        if(allowReser)
        {
            Invoke("ResetShot",shootingDelay);
            allowReser=false;
        }
        if(currentShootingMode==ShootingMode.Burst && BurstBulletLeft>1)
        {
            BurstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }
    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeapnModel);
        animator.SetTrigger("Reload");
        isReloading=true;
        Invoke("ReloadCompleted", reloadTime);
    }
    private void ReloadCompleted()
    {
        if((WeaponManager.Instance.checkAmmoLeft(thisWeapnModel)+BulletsLeft)>= MagSize)
        {
            int totalBulletsNeeded=MagSize-BulletsLeft;
            BulletsLeft=MagSize;
            WeaponManager.Instance.DecreseTotalAmmoLeft(totalBulletsNeeded,thisWeapnModel);
        }
        else
        {
            BulletsLeft=WeaponManager.Instance.checkAmmoLeft(thisWeapnModel);
            WeaponManager.Instance.DecreseTotalAmmoLeft(WeaponManager.Instance.checkAmmoLeft(thisWeapnModel),thisWeapnModel);
        }
        isReloading=false;
    }
    private void ResetShot()
    {
        readyToShoot =true;
        allowReser=true;
    }
    private Vector3 calculateDiretionandSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadSensitivity, spreadSensitivity);
        float y = UnityEngine.Random.Range(-spreadSensitivity, spreadSensitivity);

        return direction + new Vector3(x, y,0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
