using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public int score = 1000;
    public void Use(GameObject target)
    {
        var player = target.GetComponent<WomanHealth>();
        if (player != null)
        {
            GameManager.Instance.AddScore(score);
        }
        Destroy(gameObject); // ������ ��� �� ����

    }
}
