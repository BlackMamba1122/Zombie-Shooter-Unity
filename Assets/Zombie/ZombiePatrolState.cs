using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;

public class ZombiePatrolState : StateMachineBehaviour
{
    float timer;
    public float patroltime=0;
    Transform player;
    NavMeshAgent agent;
    public float detectionArea=18f;
    public float patrolSpeed=2f;
    List<Transform>wayPointsList=new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player=GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        timer=0;
        GameObject wayPointCluster=GameObject.FindGameObjectWithTag("WayPointCluster");
        foreach (Transform t in wayPointCluster.transform)
        {
            wayPointsList.Add(t);
        }
        Vector3 nextPosition=wayPointsList[UnityEngine.Random.Range(0,wayPointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip =SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }
       if(agent.remainingDistance <= agent.stoppingDistance)
       {
            agent.SetDestination(wayPointsList[UnityEngine.Random.Range(0,wayPointsList.Count)].position);
       }
       timer+=Time.deltaTime;
       timer+=Time.deltaTime;
       if(timer>patroltime)
       {
        animator.SetBool("IsPatroling",false);
       }
       float distance=Vector3.Distance(player.position, animator.transform.position);
       if(distance<detectionArea)
       {
            animator.SetBool("IsChasing",true);
       }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(agent.transform.position);
        SoundManager.Instance.zombieChannel.Stop();

    }
}
