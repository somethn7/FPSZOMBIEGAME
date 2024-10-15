using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public int HP = 100;

    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead;

    private void Start()
    {
        animator=GetComponent<Animator>();
        navAgent=GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        Debug.Log(HP);

        if(HP<=0)
        {

            int randomValue=Random.Range(0, 2);

            if(randomValue == 0 )
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            isDead = true;

            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position, 100f);

        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position, 101f);
    }
}

