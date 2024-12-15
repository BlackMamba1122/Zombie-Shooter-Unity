using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRefrences : MonoBehaviour
{
    public static GlobalRefrences Instance { get; set; }
    public GameObject BulletImpact;
    public GameObject gernadeExplosionEffect;
    public GameObject smokeGernadeEffect;
    public GameObject BloodSpray;
    public int waveNumber;

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
}
