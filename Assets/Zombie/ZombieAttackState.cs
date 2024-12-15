using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
        Transform player;
    NavMeshAgent agent;
    public float stopDistance=2.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               player=GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttack);
        }
        LookATPlayer();
               float distance=Vector3.Distance(player.position, animator.transform.position);
       if(distance > stopDistance)
       {
        animator.SetBool("IsAttacking",false);
       }
    }

    private void LookATPlayer()
    {
        Vector3 directiom =player.position-agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(directiom);
        var yRotaion = agent.transform.eulerAngles.y;
        agent.transform.rotation =Quaternion.Euler(0, yRotaion,0);
    }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.zombieChannel.Stop();

    }
}