using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal); //hitPoint : 맞은 위치 , hitNormal : 맞은 방향
                                                                   //맞은 위치에 따라서 이펙트를 나타내기 위한것

}
