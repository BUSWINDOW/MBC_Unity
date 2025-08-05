using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        NavMeshAgent agent;
        Animator anim;
        private readonly int hashMove = Animator.StringToHash("ForwardSpeed");
        public void Init()
        {
            this.agent = GetComponent<NavMeshAgent>();
            this.anim = GetComponent<Animator>();
        }
        public void MoveSet(Vector3 pos, float speed)
        {
            this.agent.isStopped = false;
            this.agent.speed = speed;
            this.anim.SetFloat(hashMove , speed);
            this.agent.SetDestination(pos);
        }
        public float GetDistance()
        {
            return this.agent.remainingDistance;
        }
        private void Update()
        {
            if(this.GetDistance() < 0.1f)
            {
                this.anim.SetFloat(hashMove, 0);

            }
        }
        public void Cancel()
        {
            //이동 취소시 현재 자신의 위치를 목적지로
            //this.agent.SetDestination(this.transform.position);
            this.anim.SetFloat(hashMove, 0);
            this.agent.isStopped = true;
        }
    }

}

  

