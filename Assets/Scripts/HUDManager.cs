using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magzineAmmoUI;
    public TextMeshProUGUI TotalAmmoUI;
    [Header("Weapon")]
    public Image ammoTypeUI;
    public Image activeWeaponUI;
    public Image unactiveWeaponUI;
    [Header("Throwable")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmmoUI;
    public Image tacticalUI;
    public TextMeshProUGUI TacticalAmmoUI;
    public Sprite emptyslot;
    public Sprite greySlot;
    public GameObject middleDot;
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
    private void Update() 
    {
        WeaponScript activeweapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<WeaponScript>();
        WeaponScript unactiveweapon = GetUnActiveWeaponSlot().GetComponentInChildren<WeaponScript>();

        if(activeweapon)
        {
            magzineAmmoUI.text = $"{activeweapon.BulletsLeft}";
            TotalAmmoUI.text = $"{WeaponManager.Instance.checkAmmoLeft(activeweapon.thisWeapnModel)}";

            WeaponScript.WeaponModel model = activeweapon.thisWeapnModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);
            if(unactiveweapon)
            {
                unactiveWeaponUI.sprite = GetWeaponSprite(unactiveweapon.thisWeapnModel);

            }
        }
        else
        {
            magzineAmmoUI.text = "";
            TotalAmmoUI.text = "";
            ammoTypeUI.sprite = emptyslot;
            activeWeaponUI.sprite = emptyslot;
            unactiveWeaponUI.sprite = emptyslot;
            ammoTypeUI.sprite = emptyslot;
        }

        if(WeaponManager.Instance.LethalCount<=0)
        {
            lethalUI.sprite =greySlot;
        }
        if(WeaponManager.Instance.tacticalCount<=0)
        {
            tacticalUI.sprite =greySlot;
        }
    }

    private Sprite GetWeaponSprite(WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.Pistol:
                return Resources.Load<GameObject>("pistol").GetComponent<SpriteRenderer>().sprite;
            case WeaponScript.WeaponModel.ak:
                return Resources.Load<GameObject>("ak").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.Pistol:
                return Resources.Load<GameObject>("pistol_ammo").GetComponent<SpriteRenderer>().sprite;
            case WeaponScript.WeaponModel.ak:
                return Resources.Load<GameObject>("riffle_ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlot)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
            return null;
    }

    internal void updateThrowablesUI()
    {
        lethalAmmoUI.text = $"{WeaponManager.Instance.LethalCount}";
        TacticalAmmoUI.text = $"{WeaponManager.Instance.tacticalCount}";
        switch (WeaponManager.Instance.equippedLethal)
        {
            case ThrowAble.ThrowableType.Gernade:
                lethalUI.sprite = Resources.Load<GameObject>("Gernade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
        switch (WeaponManager.Instance.equippedtactical)
        {
            case ThrowAble.ThrowableType.Smoke:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
        
    }
}
