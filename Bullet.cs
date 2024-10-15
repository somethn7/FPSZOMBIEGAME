using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    private void OnCollisionEnter(Collision objectWeHit)
    {
        // Çarpışma kontrolü
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit " + objectWeHit.gameObject.name);
            CreateBulletImpactEffect(objectWeHit);
            ReturnToPool();
        }
        else if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(objectWeHit);
        }
        else if (objectWeHit.gameObject.CompareTag("BeerBottle"))
        {
            print("hit a beer bottle");
            CreateBulletImpactEffect(objectWeHit);
            BeerBottle bottle = objectWeHit.gameObject.GetComponent<BeerBottle>();
            if (bottle != null)
            {
                bottle.Shatter();
            }
        }
        else if (objectWeHit.gameObject.CompareTag("Zombie"))
        {
            if (objectWeHit.gameObject.GetComponent<Zombie>().isDead==false)
            {
                objectWeHit.gameObject.GetComponent<Zombie>().TakeDamage(bulletDamage);
            }
            
      
                CreateBloodSprayEffect(objectWeHit);
                ReturnToPool();
            
        }
        else
        {
            CreateBulletImpactEffect(objectWeHit);
        }

        if (objectWeHit.gameObject.CompareTag("Target") || objectWeHit.gameObject.CompareTag("Zombie"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        BulletPoolManager.Instance.ReturnBullet(gameObject);
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        Debug.Log("ZAAKDJASDHXAXAZXAZXAXA");
        if (GlobalReferences.Instance.bloodSprayEffect != null)
        {
            ContactPoint contact = objectWeHit.contacts[0];
            GameObject bloodSprayPrefab = Instantiate(
                GlobalReferences.Instance.bloodSprayEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
        }
    }

    private void CreateBulletImpactEffect(Collision objectWeHit)
    {
        if (GlobalReferences.Instance.bulletImpactEffectPrefab != null)
        {
            ContactPoint contact = objectWeHit.contacts[0];
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactEffectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            hole.transform.SetParent(objectWeHit.gameObject.transform);
            Destroy(hole, 7.0f);
        }
        else
        {
            Debug.LogError("Bullet impact effect prefab is missing!");
        }
    }
}
