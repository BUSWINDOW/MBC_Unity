using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _Test_PlayerCtrl : MonoBehaviour
{
    int groundLayerMask = 1 << 6;
    int enemyLayerMask = 1 << 7;
    Ray ray;
    RaycastHit hit;
    _Test_Mover mover;
    _Test_Fighter fighter;

    void Start()
    {
        this.mover = GetComponent<_Test_Mover>();
        this.fighter = GetComponent<_Test_Fighter>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //우클릭 하면
        {

            this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(this.ray, out hit, Mathf.Infinity, this.groundLayerMask))
            {
                this.fighter.Cancel();
                this.mover.MoveSet(hit.point);
            }

        }

        if (Input.GetMouseButtonDown(0)) //좌클릭 하면
        {

            this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(this.ray, out hit, Mathf.Infinity, this.enemyLayerMask))
            {
                this.fighter.AttackSet(hit.transform.gameObject);
            }

        }

    }
}
