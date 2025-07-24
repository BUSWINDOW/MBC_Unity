using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    LineRenderer firePos; // �Ѿ� �߻��ϴ� ���� LineRenderer�޾Ұ�, �ٸ����� �޸����� ����
    PlayerInputCtrl inputCtrl;
    bool isFire = false;
    public float maxDistance = 100;
    public int damage = 10;
    void Start()
    {
        this.firePos = GetComponentInChildren<LineRenderer>();
        this.firePos.positionCount = 2;
        this.firePos.SetPosition(1, this.firePos.transform.position);
        this.inputCtrl = GetComponent<PlayerInputCtrl>();
    }

    void Update()
    {
        if (this.inputCtrl.Fire&&!isFire)
        {
            this.isFire = true;
            this.firePos.enabled = true;
            Ray ray = new Ray(this.firePos.transform.position, this.firePos.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,this.maxDistance,1<<6)) // 6�� ���̾� : Enemy, ���� ��Ҵٸ�
            {
                //�� �Ÿ������� LineRenderer�� �׸�
                //���� ���� onDamage ����
                this.firePos.SetPosition(0, this.firePos.transform.position);
                this.firePos.SetPosition(1, hit.transform.position);
                this.firePos.enabled = true;
                var enemy = hit.collider.gameObject.GetComponent<EnemyCtrl>();
                enemy.OnDamage(this.damage);
            }
            else // �ȴ�Ҵٸ�
            {
                //maxDistance ���� LineRenderer�� �׸�
                this.firePos.SetPosition(0, this.firePos.transform.position);
                this.firePos.SetPosition(1, this.firePos.transform.position + (this.firePos.transform.forward * this.maxDistance));
                this.firePos.enabled = true;
            }
            StartCoroutine(UtilCode.WaitForSec(() =>
            {
                this.isFire = false;
                this.inputCtrl.Fire = false;
                this.firePos.enabled =false;
            }, 0.2f));
        }
    }
}
