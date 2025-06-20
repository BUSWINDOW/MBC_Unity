using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSAnimationCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private Animator anim;
    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveZ = Animator.StringToHash("MoveZ");
    private readonly int hashSprint = Animator.StringToHash("Sprint");
    private readonly int hashReload = Animator.StringToHash("Reload");

    void Start()
    {
        this.input = GetComponent<TPSPlayerInput>();
        this.anim = GetComponent<Animator>();
    }
    void Update()
    {
        this.anim.SetFloat(this.hashMoveX, this.input.MoveX, 0.01f, Time.fixedDeltaTime);
        this.anim.SetFloat(this.hashMoveZ, this.input.MoveZ, 0.01f, Time.fixedDeltaTime);
        this.anim.SetBool(this.hashSprint, (input.isRun && this.input.MoveZ > 0.1f));
        this.anim.SetBool(this.hashReload, input.Reload);
    }
}
