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
    PlayerAnimCtrl animCtrl;
    
    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.animCtrl = GetComponent<PlayerAnimCtrl>();
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
        this.animCtrl.RunAnimSet(this.agent.speed);
        while (Vector3.Distance(this.agent.destination, this.transform.position) > 0.1f)
        {
            yield return null;
        }
        this.animCtrl.RunAnimSet(0);
    }
    IEnumerator attackRoutine()
    {
        this.animCtrl.RunAnimSet(this.agent.speed);
        while (Vector3.Distance(this.agent.destination, this.transform.position) > 1)
        {
            //Debug.Log(this.transform.rotation != Quaternion.LookRotation(hit.point));
            
            yield return null;
        }
        //this.transform.LookAt(hit.point);


        //Debug.Log(this.transform.rotation != Quaternion.LookRotation(hit.point));

        this.animCtrl.RunAnimSet(0);
        this.agent.SetDestination(this.transform.position);
        var targetDir = hit.point - this.transform.position;
        targetDir.y = 0;
        
        while (Quaternion.Angle(
            this.transform.rotation, 
            Quaternion.LookRotation(targetDir)) > 1)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 20);

            yield return null;
        }
        this.animCtrl.AttackAnimSet();
    }


}
