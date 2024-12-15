using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponScript;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource ShootingSoundPistol;
    public AudioSource ReloadingSoundPistol;
    public AudioSource EmptySoundak;

    public AudioClip AkShot;
    public AudioClip PistolShot;
    public AudioSource ShootingSoundAk;
    public AudioSource ReloadingSoundAk;
    public AudioSource throwablesChannel;
    public AudioClip gernadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;
    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;
    public AudioSource playerChannel;
    public AudioClip playerhurt;
    public AudioClip palyerdie;
    public AudioClip gameover;

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
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ShootingSoundPistol.PlayOneShot(PistolShot);
                break;
            case WeaponModel.ak:
                ShootingSoundAk.PlayOneShot(AkShot);
                break;
        }
    }
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ReloadingSoundPistol.Play();
                break;
            case WeaponModel.ak:
                ReloadingSoundAk.Play();
                break;
        }
    }
}
