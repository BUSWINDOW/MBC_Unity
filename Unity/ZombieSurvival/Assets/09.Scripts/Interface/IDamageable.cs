using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal); //hitPoint : ���� ��ġ , hitNormal : ���� ����
                                                                   //���� ��ġ�� ���� ����Ʈ�� ��Ÿ���� ���Ѱ�

}
