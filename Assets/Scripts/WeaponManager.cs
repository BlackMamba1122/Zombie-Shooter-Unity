using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlot;
    public GameObject activeWeaponSlot;
    [Header("Ammo")]
    public int totalRifleAmmo=0;
    public int totalPistolAmmo=0;
    public int LethalCount=0;
    public ThrowAble.ThrowableType equippedLethal;
    public int tacticalCount=0;
    public ThrowAble.ThrowableType equippedtactical;
    public GameObject smokeGernadePrefab;


    public float ThrowForce=10f;
    public GameObject gernadePrefab;
    public GameObject ThrowAbleSpawn;
    public float forceMultiplier=0;
    public float MaxForce=2f;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        activeWeaponSlot = weaponSlot[0];
        equippedLethal = ThrowAble.ThrowableType.None;
        equippedtactical = ThrowAble.ThrowableType.None;
    }
    private void Update()
    {
        foreach (GameObject weaponSlo in weaponSlot)
        {
            if(weaponSlo == activeWeaponSlot)
            {
                weaponSlo.SetActive(true);
            }
            else
            {
                weaponSlo.SetActive(false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
        if(Input.GetKeyUp(KeyCode.G))
        {
            if(LethalCount > 0)
            {
                ThrowLethal();
            }
            forceMultiplier=0;
        }
        if(Input.GetKey(KeyCode.G))
        {
            forceMultiplier+=Time.deltaTime;
            if(forceMultiplier>MaxForce)
            {
                forceMultiplier=MaxForce;
            }
        }
        if(Input.GetKeyUp(KeyCode.T))
        {
            if(tacticalCount > 0)
            {
                ThrowTactical();
            }
            forceMultiplier=0;
        }
        if(Input.GetKey(KeyCode.T))
        {
            forceMultiplier+=Time.deltaTime;
            if(forceMultiplier>MaxForce)
            {
                forceMultiplier=MaxForce;
            }
        }
    }

    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedtactical);
        GameObject throwable = Instantiate(tacticalPrefab,ThrowAbleSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (ThrowForce * forceMultiplier),ForceMode.Impulse);
        throwable.GetComponent<ThrowAble>().hasBeenThrown = true;
        tacticalCount--;
        if(tacticalCount<=0)
        {
            equippedtactical = ThrowAble.ThrowableType.None;
        }
        HUDManager.Instance.updateThrowablesUI();
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethal);
        GameObject throwable = Instantiate(lethalPrefab,ThrowAbleSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (ThrowForce * forceMultiplier),ForceMode.Impulse);
        throwable.GetComponent<ThrowAble>().hasBeenThrown = true;
        LethalCount--;
        if(LethalCount<=0)
        {
            equippedLethal = ThrowAble.ThrowableType.None;
        }
        HUDManager.Instance.updateThrowablesUI();

    }

    private GameObject GetThrowablePrefab(ThrowAble.ThrowableType equippedType)
    {
        switch (equippedType)
        {
            case ThrowAble.ThrowableType.Gernade:
                return gernadePrefab;            
            case ThrowAble.ThrowableType.Smoke:
                return smokeGernadePrefab;
        }
        return new();
    }

    public void PickUpWeapon(GameObject PickUpweapon)
    {
        AddWEaponIntoActiveSlot(PickUpweapon);
    }

    private void AddWEaponIntoActiveSlot(GameObject pickUpweapon)
    {
        DropCurrentWeapon(pickUpweapon);
        pickUpweapon.transform.SetParent(activeWeaponSlot.transform, false);
        WeaponScript weapon = pickUpweapon.GetComponent<WeaponScript>();
        pickUpweapon.transform.localPosition = new Vector3(weapon.SpawnPosition.x, weapon.SpawnPosition.y ,weapon.SpawnPosition.z);
        pickUpweapon.transform.localRotation = Quaternion.Euler(weapon.SpawnRotation.x, weapon.SpawnRotation.y, weapon.SpawnRotation.z);
        weapon.IsActiveWeapon =true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickUpweapon)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<WeaponScript>().IsActiveWeapon = false;
            weaponToDrop.GetComponent<WeaponScript>().animator.enabled = false;
            weaponToDrop.transform.SetParent(pickUpweapon.transform.parent);
            weaponToDrop.transform.localPosition = pickUpweapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickUpweapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript currentweapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            currentweapon.IsActiveWeapon = false;
        }
        activeWeaponSlot = weaponSlot[slotNumber];
        if(activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            newWeapon.IsActiveWeapon = true;
        }
    }

    internal void PickUpAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.pistol_ammo:
            totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.riffle_ammo:
            totalRifleAmmo += ammo.ammoAmount;
                break;
        }
        
    }
    internal void AddAmmo(string name,int amount)
    {
        switch (name)
        {
            case "pistol":
            totalPistolAmmo += amount;
                break;
            case "ak":
            totalRifleAmmo += amount;
                break;
        }
        
    }

    internal void DecreseTotalAmmoLeft(int v, WeaponScript.WeaponModel thisWeapnModel)
    {
        switch (thisWeapnModel)
        {
            case WeaponScript.WeaponModel.Pistol:
                totalPistolAmmo -= v;
                break;
            case WeaponScript.WeaponModel.ak:
                totalRifleAmmo -= v;
                break;
        }        
    }
        public int checkAmmoLeft(WeaponScript.WeaponModel thisWeapnModel)
    {
        switch (thisWeapnModel)
        {
            case WeaponScript.WeaponModel.Pistol:
                return totalPistolAmmo;            
            case WeaponScript.WeaponModel.ak:
                return totalRifleAmmo;            
        }
        return 0;
    }

    public void PickUpThrowable(ThrowAble throwable)
    {
        switch (throwable.throwableType)
        {
            case ThrowAble.ThrowableType.Gernade:
                PickUpThrowableAsLethal(ThrowAble.ThrowableType.Gernade);
                break;
            case ThrowAble.ThrowableType.Smoke:
                PickUpThrowableAsTactical(ThrowAble.ThrowableType.Smoke);
                break;
        }
    }

    private void PickUpThrowableAsTactical(ThrowAble.ThrowableType tactical)
    {
        if(equippedtactical == tactical || equippedtactical == ThrowAble.ThrowableType.None)
        {
            equippedtactical = tactical;
            if(tacticalCount < 2)
            {
                tacticalCount +=1;
                Destroy(InterationManager.Instance.hoverThrowable.gameObject);
                HUDManager.Instance.updateThrowablesUI();
            }
            else
            {
                print("Tactical limit");
            }
        }
        else
        {

        }
    }

    private void PickUpThrowableAsLethal(ThrowAble.ThrowableType lethal)
    {
        if(equippedLethal == lethal || equippedLethal == ThrowAble.ThrowableType.None)
        {
            print("why");
            equippedLethal = lethal;
            if(LethalCount < 2)
            {
                LethalCount +=1;
                Destroy(InterationManager.Instance.hoverThrowable.gameObject);
                HUDManager.Instance.updateThrowablesUI();
            }
            else
            {
                print("lethal limit");
            }
        }
        else
        {

        }
    }
}
