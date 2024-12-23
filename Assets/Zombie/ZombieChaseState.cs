using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public float chaseSpeed=6f;
    public float stopChasingDistance =21;
    public float attackingDistance=2.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player=GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }
       agent.SetDestination(player.position);
       animator.transform.LookAt(player);
       float distance=Vector3.Distance(player.position, animator.transform.position);
       if(distance > stopChasingDistance)
       {
        animator.SetBool("IsChasing",false);
       }
       if(distance<attackingDistance)
       {
        animator.SetBool("IsAttacking",true);
       }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(animator.transform.position);
       SoundManager.Instance.zombieChannel.Stop();
    }
}
