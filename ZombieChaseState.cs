using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }





        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer= Vector3.Distance(player.position,animator.transform.position);

        if(distanceFromPlayer>stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        if(distanceFromPlayer<attackingDistance)
        {
            animator.SetBool("isAttacking",true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        agent.SetDestination(agent.transform.position);

        SoundManager.Instance.zombieChannel.Stop();
    }
}
