using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour
{
    public float speed = 100f; //코인 속도
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(Vector2.right * speed,ForceMode2D.Impulse);
    }

   
 
}
