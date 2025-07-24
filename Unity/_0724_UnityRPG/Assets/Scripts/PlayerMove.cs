using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    NavMeshAgent agent;
    int groundLayerMask = 1 << 6;
    int enemyLayerMask = 1 << 7;
    Ray ray;
    RaycastHit hit;


    Animator anim;
    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //우클릭 하면
        {
            
            this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(this.ray,out hit, Mathf.Infinity, this.groundLayerMask))
            {
                StopAllCoroutines();
                this.agent.SetDestination(hit.point);
                StartCoroutine(this.moveRoutine());
            }
            
        }

        if (Input.GetMouseButtonDown(0)) //좌클릭 하면
        {
            
            this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(this.ray, out hit, Mathf.Infinity, this.enemyLayerMask))
            {
                StopAllCoroutines();
                this.agent.SetDestination(hit.point);
                StartCoroutine(this.attackRoutine());
            }
            
        }

    }
    IEnumerator moveRoutine()
    {
        this.anim.SetFloat("forwardSpeed", this.agent.speed);
        while (Vector3.Distance(this.agent.destination, this.transform.position) > 0.1f)
        {
            yield return null;
        }
        this.anim.SetFloat("forwardSpeed", 0);
    }
    IEnumerator attackRoutine()
    {
        this.anim.SetFloat("forwardSpeed", this.agent.speed);
        while (Vector3.Distance(this.agent.destination, this.transform.position) > 1f)
        {
            yield return null;
        }
        this.agent.SetDestination(this.transform.position);
        this.anim.SetFloat("forwardSpeed", 0);
        this.anim.SetTrigger("Attack");
    }
}
