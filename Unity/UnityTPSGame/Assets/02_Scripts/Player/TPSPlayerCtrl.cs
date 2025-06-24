using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TPSPlayerCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private PlayerDamage damage;
    public Rigidbody rb;
    private const string enemy = "Enemy";

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler onPlayerDie;

    public float moveSpeed = 300;
    public float runSpeed = 800;
    public float rotateSpeed = 400;

    private int hp;
    private int max_Hp = 100000;

    [SerializeField] private Image hp_Bar;

    private readonly Color initColor = new Color(0, 1, 0);


    void Start()
    {
        this.input = GetComponent<TPSPlayerInput>();
        this.damage = GetComponent<PlayerDamage>();

        this.damage.hitAction = () =>
        {
            this.hp -= 20;
            this.hp = Mathf.Clamp(this.hp, 0, this.max_Hp);
            HP_Bar_Display();
            if (this.hp <= 0)
            {
                PlayerDie();
            }
        };

        this.rb = GetComponent<Rigidbody>();
        this.hp = this.max_Hp;
        this.hp_Bar.color = initColor;
    }

    private void HP_Bar_Display()
    {
        this.hp_Bar.fillAmount = (float)hp / (float)max_Hp;
        if (hp_Bar.fillAmount <= 0.3f)
            hp_Bar.color = Color.red;
        else if (hp_Bar.fillAmount <= 0.5f)
        {
            hp_Bar.color = Color.yellow;
        }
    }

    private void Update()
    {
        this.transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotateSpeed * this.input.MouseX);
    }

    void FixedUpdate()
    {
        Vector3 moveDir = (this.input.MoveX * this.transform.right + this.transform.forward* this.input.MoveZ).normalized;
        
        this.rb.velocity =  moveDir * (input.isRun?this.runSpeed:moveSpeed) * Time.fixedDeltaTime;
        

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

    void PlayerDie()
    {
        Debug.Log("Á×À½");
        GameManager.Instance.isGameOver = true;
        /*var enemys = GameObject.FindGameObjectsWithTag(enemy);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SendMessage("PlayerDie", SendMessageOptions.DontRequireReceiver);
        }*/

        onPlayerDie();
    }
}
