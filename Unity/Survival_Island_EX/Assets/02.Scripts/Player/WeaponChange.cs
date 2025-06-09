using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//meshRenderer(바꿀 총들)이 필요
public class WeaponChange : MonoBehaviour
{
    public SkinnedMeshRenderer spas12;
    public MeshRenderer[] ak47;
    public MeshRenderer[] m4a1;
    public Animation anim;

    public bool isHaveM4a1 = false;

    private readonly string draw = "draw";
    void Start()
    {
        this.anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { ChangeWeapon2(); }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) { ChangeWeapon3(); }

    }

    private void ChangeWeapon1() // ak47은 연사되야함. 
    {
        ChangeAni();
        foreach (MeshRenderer mr in ak47)
        {
            mr.enabled = true;
        }
        spas12.enabled = false;
        foreach (MeshRenderer mr in m4a1)
        {
            mr.enabled = false;
        }
        this.isHaveM4a1 = false;
    }
    private void ChangeWeapon2()
    {
        ChangeAni();
        foreach (MeshRenderer mr in ak47)
        {
            mr.enabled = false;
        }
        spas12.enabled = true;
        foreach (MeshRenderer mr in m4a1)
        {
            mr.enabled = false;
        }
        this.isHaveM4a1 = false;
    }
    private void ChangeWeapon3()
    {
        ChangeAni();

        foreach (MeshRenderer mr in ak47)
        {
            mr.enabled = false;
        }
        spas12.enabled = false;
        foreach (MeshRenderer mr in m4a1)
        {
            mr.enabled = true;
        }
        this.isHaveM4a1 = true;

    }

    private void ChangeAni()
    {
        this.anim.Stop();
        this.anim.Play(draw);
    }
}
