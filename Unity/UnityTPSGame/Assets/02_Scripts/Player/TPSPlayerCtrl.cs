using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TPSPlayerCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    public Rigidbody rb;

    public float moveSpeed = 300;
    public float runSpeed = 800;
    public float rotateSpeed = 400;

    public GameObject bulletPrefab;
    public GameObject[] bulletPool;
    private int bulletCnt;
    
    
    void Start()
    {
        this.input = GetComponent<TPSPlayerInput>();

        this.rb = GetComponent<Rigidbody>();
        this.bulletPool = new GameObject[150];
        for (int i = 0; i < bulletPool.Length; i++)
        {
            this.bulletPool[i] = Instantiate(bulletPrefab);
        }
        this.bulletCnt = 0;
    }

    
    void Update()
    {
        Vector3 moveDir = (this.input.MoveX * this.transform.right + this.transform.forward* this.input.MoveZ).normalized;
        
        this.rb.velocity =  moveDir * (input.isRun?this.runSpeed:moveSpeed) * Time.fixedDeltaTime;
        this.transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotateSpeed * this.input.MouseX);

        if (!this.input.isRun && this.input.Fire)
        {
            //ÃÑ½ô
            
        }
        #region ·¹°Å½Ã ¾Ö´Ï¸ÞÀÌ¼Ç
        /*if(this.input.MoveZ > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.anim.Play("SprintF");
            }
            else
                this.anim.Play("RunF");
        }
        else if (this.input.MoveZ < -0.1f)
        {
            this.anim.Play("RunB");
        }
        else if (this.input.MoveX > 0.1f)
        {
            this.anim.Play("RunR");
        }
        else if (this.input.MoveX < -0.1f)
        {
            this.anim.Play("RunL");
        }
        else
        {
            this.anim.Play("Idle");
        }*/
        #endregion




    }
}
