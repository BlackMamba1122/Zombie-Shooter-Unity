using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int bulletDamage = 25;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Target")) 
        {
            print("hit " + other.gameObject.name);
            CreateBulletImpactEffect(other);
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Bootle")) 
        {
            print("hit " + other.gameObject.name);
            other.gameObject.GetComponent<Bottlee>().shatter();
        }
        if(other.gameObject.CompareTag("Zombie")) 
        {
            print("hit " + other.gameObject.name);
            if(other.gameObject.GetComponent<Zombie>().IsDead==false)
            {
                other.gameObject.GetComponent<Zombie>().TakeDAmage(bulletDamage);
            }
            CreateBloodEffect(other);
            Destroy(gameObject);
        }

    }

    private void CreateBulletImpactEffect(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        GameObject hole = Instantiate(
            GlobalRefrences.Instance.BulletImpact,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(other.gameObject.transform);
    }

    void CreateBloodEffect(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        GameObject blood = Instantiate(
            GlobalRefrences.Instance.BloodSpray,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        blood.transform.SetParent(other.gameObject.transform);
    }

}
