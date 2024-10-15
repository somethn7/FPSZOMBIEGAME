using UnityEngine;

public class ZombieGirl : MonoBehaviour
{
    public ZombieHand zombieHand; // ZombieHand bileşeni atanacak

    public int zombieDamage; // Hasar miktarı

    private void Start()
    {
        if (zombieHand != null)
        {
            zombieHand.damage = zombieDamage;
        }
        else
        {
            Debug.LogWarning("ZombieHand bileşeni referans edilmemiş.");
        }
    }
}
