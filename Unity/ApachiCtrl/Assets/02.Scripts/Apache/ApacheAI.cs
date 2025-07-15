using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ApacheAI : MonoBehaviourPun
{
    private readonly string tankTag = "Player";
    public enum AppacheState { PATROL, ATTACK, DESTROY }
    public AppacheState state = AppacheState.PATROL;

    public List<Transform> patrolList;
    float rotSpeed = 15f, moveSpeed = 10f;
    Transform myTr;

    int currentPatorlIdx = 0;
    float wayCheck = 7f;
    public bool isSearch = true;
    public float attackTime = 0f;
    public float attackRemiming = 3f;

    private ApacheAI_Attack attak;

    [SerializeField] private GameObject[] targets = null;

    

    //private Transform targetTr;

    void Start()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;
        //��� ������ UDP�������
        //photonView.ObservedComponents[0] = this;
        var pObj = GameObject.Find("Points");
        if (pObj != null)
            pObj.GetComponentsInChildren<Transform>(patrolList);

        patrolList.RemoveAt(0);

        myTr = transform;
        attak = GetComponent<ApacheAI_Attack>();

    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isSearch)
                WayPatrol();
            else
                Attack();
        }
        else
        {
            Attack();
        }


    }
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            CheckP();
    }

    void WayPatrol()
    {
        state = AppacheState.PATROL;
        Vector3 movePos = patrolList[currentPatorlIdx].position - myTr.position;

        myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(movePos), Time.fixedDeltaTime * rotSpeed);
        myTr.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
        Search();

    }
    void Search()
    {
        //float tankFindDist = (GameObject.FindWithTag(tankTag).transform.position - myTr.transform.position).sqrMagnitude;
        //// Distance�� �����ϴ°� ����, �ڿ��� ���� �Ա� ������
        //if (tankFindDist <= 80f * 80f)

        #region ��ũ�� 1���϶��� ����
        /*if (Vector3.Distance(GameObject.FindWithTag(tankTag).transform.position, myTr.transform.position) < 80f)
            isSearch = false;*/
        #endregion


        TankSearch();
    }

    private Transform TankSearch()
    {
        #region ��ũ�� 2�� �̻��϶��� ����
        this.targets = GameObject.FindGameObjectsWithTag(tankTag);
        Transform targetTr = null;
        if (targets.Length > 0)
        {
            float minDist = Mathf.Infinity;
            
            foreach (GameObject target in targets)
            {
                float dist = Vector3.Distance(target.transform.position, myTr.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    targetTr = target.transform;
                }
            }
            if (minDist <= 80f)
            {
                isSearch = false;
                //���� ����� ��ũ�� Ÿ������ ��´�
                Vector3 targetDist = (targetTr.position - myTr.transform.position);
                myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(targetDist.normalized), Time.fixedDeltaTime * rotSpeed);
            }
        }
        else
        {
            isSearch = true;
        }

        return targetTr;
        #endregion
    }

    void CheckP()
    {
        if (Vector3.Distance(transform.position, patrolList[currentPatorlIdx].position) <= 5f)
        {
            if (currentPatorlIdx == patrolList.Count - 1)
                currentPatorlIdx = 0;
            else
                currentPatorlIdx++;
        }
    }

    void Attack()
    {
        //�÷��̾� ��ũ�� ������ �� ���, ���� ���� �����ָ� Ÿ������ ��´�
        //�������� ������ ������, �׶��� �׳� return �Ѵ�.
        state = AppacheState.ATTACK;
        //Vector3 targetDist = (GameObject.FindWithTag(tankTag).transform.position - myTr.transform.position);
        if (TankSearch() == null)
        {
            isSearch = true;
            return;
        }
        var targetDist = TankSearch().position - myTr.transform.position;
        myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(targetDist.normalized), Time.fixedDeltaTime * rotSpeed);
        if (Time.time - attackTime >= attackRemiming)
        {
            attak.Fire(attak.firePosL, attak.leaserBeamL);
            attak.Fire(attak.firePosR, attak.leaserBeamR);
            attackTime = Time.time;
        }
        if (targetDist.magnitude > 80f)
            isSearch = true;
    }


    
}