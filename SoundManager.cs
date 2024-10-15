using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;


    public AudioSource reloadingSoundPistol;
    public AudioSource reloadingSoundShotGun;
    public AudioSource reloadingSoundRifle;
    public AudioSource emptyMagazineSound;

    public AudioClip PistolShot;
    public AudioClip ShotGunShot;
    public AudioClip RiffleShot;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
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
        switch(weapon)
        {
            case WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(PistolShot);
                break;

            case WeaponModel.ShotGun:
                ShootingChannel.PlayOneShot(ShotGunShot);
                break;
             
            case WeaponModel.Rifle:
                ShootingChannel.PlayOneShot(RiffleShot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol:
                reloadingSoundPistol.Play();
                break;

            case WeaponModel.ShotGun:
                reloadingSoundShotGun.Play();
                break;

            case WeaponModel.Rifle:
                reloadingSoundRifle.Play();
                break;

        }
    }
}

