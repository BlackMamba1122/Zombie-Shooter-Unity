using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterationManager : MonoBehaviour
{
    public static InterationManager Instance { get; set; }
    public WeaponScript hoverWeapon = null;
    public AmmoBox hoverAmmoBox=null;
    public ThrowAble hoverThrowable=null;
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if(objectHit.GetComponent<WeaponScript>() && objectHit.GetComponent<WeaponScript>().IsActiveWeapon == false)
            {
                if(hoverWeapon)
                {
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }
                hoverWeapon = objectHit.gameObject.GetComponent<WeaponScript>();
                hoverWeapon.GetComponent<Outline>().enabled = true;   
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHit.gameObject);
                }
            }
            else
            {   
                if (hoverWeapon)
                {
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            //ammo
            if(objectHit.GetComponent<AmmoBox>())
            {
                if(hoverAmmoBox)
                {
                    hoverAmmoBox.GetComponent<Outline>().enabled = false;
                }
                hoverAmmoBox = objectHit.gameObject.GetComponent<AmmoBox>();
                hoverAmmoBox.GetComponent<Outline>().enabled = true;   
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpAmmo(hoverAmmoBox);
                    Destroy(objectHit.gameObject);
                }
            }
            else
            {   
                if (hoverAmmoBox)
                {
                    hoverAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
                        //gernade
            if(objectHit.GetComponent<ThrowAble>())
            {
                if(hoverThrowable)
                {
                    hoverThrowable.GetComponent<Outline>().enabled = false;
                }
                hoverThrowable = objectHit.gameObject.GetComponent<ThrowAble>();
                hoverThrowable.GetComponent<Outline>().enabled = true;   
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpThrowable(hoverThrowable);
                }
            }
            else
            {   
                if (hoverThrowable)
                {
                    hoverThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
