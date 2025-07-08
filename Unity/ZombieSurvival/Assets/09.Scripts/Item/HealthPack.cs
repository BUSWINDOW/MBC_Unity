using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public int health = 50;
    public void Use(GameObject target)
    {
        var player = target.GetComponent<WomanHealth>();
        if (player != null)
        {
            player.RestoreHealth(health); // �÷��̾��� ü�� ȸ��
        }
        Destroy(gameObject); // ������ ��� �� ����
    }
}
