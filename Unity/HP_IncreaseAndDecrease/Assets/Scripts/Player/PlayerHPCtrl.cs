using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPCtrl : MonoBehaviour
{

    public Slider hpSlider;
    public TMP_Text hpText;
    private Animator anim;
    private int hp = 100;
    Vector3 moveDir;

    void Start()
    {
        this.anim = GetComponent<Animator>();
        this.hpSlider.onValueChanged.AddListener((value) =>
        {
            this.hp = (int)(100 * value);
            this.hpText.text = $"HP : {this.hp}";
            this.anim.SetLayerWeight(1, 1 - value);
        });
    }
    private void Update()
    {
        if(this.moveDir.magnitude > 0.1f)
        {
            this.transform.rotation = Quaternion.LookRotation(moveDir);
            this.anim.SetBool("IsMove", true);
            this.anim.SetFloat("Move", this.moveDir.magnitude);
            this.transform.Translate(Vector3.forward * Time.deltaTime);
        }
        else
        {
            this.anim.SetBool("IsMove", false);
        }
    }
    public void MoveCtrl(Vector2 dir)
    {
        this.moveDir = new Vector3(dir.x, 0, dir.y);
        
    }
    public void GranadeThrow()
    {
        this.anim.SetTrigger("Throw");
    }
}
