using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Ammo")]
    public TMP_Text magazineAmmoUI;
    public TMP_Text totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TMP_Text lethalAmountUI;

    public Image tacticalUI;
    public TMP_Text tacticalAmountUI;

    public Sprite emptySlot;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot()?.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / Mathf.Max(activeWeapon.bulletPerBurst, 1)}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeft()}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
          //  ammoTypeUI.sprite = GetAmmoSprite(model);
            //activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
            else
            {
                unActiveWeaponUI.sprite = emptySlot;
            }
        }
        else
        {
            magazineAmmoUI.text = " ";
            totalAmmoUI.text = " ";

           // activeWeaponUI.sprite = emptySlot;
           // unActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<Sprite>("Pistol_Weapon");

            case Weapon.WeaponModel.Rifle:
                return Resources.Load<Sprite>("Rifle_Weapon");

            default:
                return emptySlot;
        }
    }

   private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
            return Resources.Load<Sprite>("Pistol_Ammo");
          
            case Weapon.WeaponModel.Rifle:
               return Resources.Load<Sprite>("Rifle_Ammo");
              
            default:
               return emptySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}
