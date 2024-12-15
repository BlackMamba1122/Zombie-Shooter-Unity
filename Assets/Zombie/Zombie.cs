using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP =100;
    private Animator animator;
    private NavMeshAgent navAgent;
    public bool IsDead;
    void Start()
    {
        animator=GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void TakeDAmage(int damage)
    {
        HP-=damage;

        if(HP<=0)
        {
            int random=UnityEngine.Random.Range(0,2);
            if(random==0)
            {
                animator.SetTrigger("Die1");
            }
            else
            {
                animator.SetTrigger("Die2");        
            }
            IsDead=true;
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("Damage");
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 70f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 71f);

    }
}
