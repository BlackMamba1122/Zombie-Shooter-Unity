using System.CodeDom.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ThrowAble : MonoBehaviour
{
    [SerializeField] float Delay= 10f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce =1200f;
    float countdown;
    bool hasexploaded=false;
    public bool hasBeenThrown= false;
    public enum ThrowableType
    {
        None,
        Gernade,
        Smoke
    }
    public ThrowableType throwableType;
    private void Start()
    {
        countdown = Delay;
    }
    private void Update()
    {
        if(hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0 && !hasexploaded)
            {
                Explode();
                hasexploaded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowAbleEffect();
        Destroy(gameObject);
    }

    private void GetThrowAbleEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Gernade:
                GernadeEffect();
                break;
            case ThrowableType.Smoke:
                SmokeEffect();
                break;
        }
    }

    private void SmokeEffect()
    {
        GameObject smokeEffect = GlobalRefrences.Instance.smokeGernadeEffect;
        Instantiate(smokeEffect,transform.position,transform.rotation);
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.gernadeSound);
        Collider[] collider = Physics.OverlapSphere(transform.position,damageRadius);
        foreach (Collider obj in collider)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
            }
        } 
    }

    private void GernadeEffect()
    {
        GameObject explosionEffect = GlobalRefrences.Instance.gernadeExplosionEffect;
        Instantiate(explosionEffect,transform.position,transform.rotation);
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.gernadeSound);
        Collider[] collider = Physics.OverlapSphere(transform.position,damageRadius);
        foreach (Collider obj in collider)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,damageRadius);
            }
            if(obj.gameObject.GetComponent<Zombie>())
            {
                obj.gameObject.GetComponent<Zombie>().TakeDAmage(65);
            }
        }        
    }
}
