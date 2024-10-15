using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    public bool isActiveWeapon;
    public float bulletVelocity = 500f;
    public float bulletPrefabLifeTime = 3f;

    public bool allowReset = true;
    public bool isShooting;
    public bool readyToShoot;
    public float shootingDelay = 0.2f;
    public int bulletPerBurst;
    public int currentBurst;
    public float spreadIntensity;
    public Transform bulletSpawn;

    public GameObject muzzleEffect;
    [SerializeField] private ParticleSystem muzzleEffectPs;
    public Animator animator;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponModel
    {
        Pistol,
        ShotGun,
        Rifle
    }

    public WeaponModel thisWeaponModel;
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletPerBurst;
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSound.Play();
            }

            // Determine if the weapon is shooting based on mode
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            // Handle reload input
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            // Handle firing
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                currentBurst = bulletPerBurst;
                FireWeapon();
            }
        }
    }

    void FireWeapon()
    {
        bulletsLeft--;
        muzzleEffectPs.Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        if (!readyToShoot) return;

        Debug.Log("Firing weapon");

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = BulletPoolManager.Instance.GetBullet();
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.position = bulletSpawn.position;
        bullet.transform.forward = shootingDirection;

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootingDirection * bulletVelocity;
        }
        else
        {
            Debug.LogError("Bullet prefabında Rigidbody bulunamadı!");
        }

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Debug.Log("Starting reset timer");
            StartCoroutine(ResetShotAfterDelay());
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        int ammoToReload = Mathf.Min(magazineSize, WeaponManager.Instance.CheckAmmoLeft());
        bulletsLeft = ammoToReload;
        WeaponManager.Instance.DecreaseTotalAmmo(ammoToReload);

        isReloading = false;
    }

    private IEnumerator ResetShotAfterDelay()
    {
        yield return new WaitForSeconds(shootingDelay);
        ResetShot();
    }

    private void ResetShot()
    {
        Debug.Log("Resetting shot");
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(100);
        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float z = Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, 0, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifeTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifeTime);
        BulletPoolManager.Instance.ReturnBullet(bullet);
    }
}
