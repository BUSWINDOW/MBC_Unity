
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class SwatAnimationCtrl : MonoBehaviour
{
    public Animator animator;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly static int hashDie = Animator.StringToHash("Die");
    private readonly static int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly static int hashFire = Animator.StringToHash("Fire");
    private readonly static int hashReload = Animator.StringToHash("Reload");
    private readonly static int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly static int hashOffSet = Animator.StringToHash("OffSet");
    private readonly static int hashPlayerDie = Animator.StringToHash("PlayerDie");


    // Start is called before the first frame update
    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetFloat(hashOffSet, Random.Range(0, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1, 1.2f));
    }

    public void SetPatroll(float speed)
    {
        this.animator.SetBool(hashMove, true);
        this.animator.SetFloat(hashSpeed, speed);
    }
    public void SetTrace(float speed)
    {
        this.animator.SetBool(hashMove, true);
        this.animator.SetFloat(hashSpeed, speed);
    }
    public void SetAttack()
    {
        this.animator.SetBool(hashMove, false);
        this.animator.SetTrigger(hashFire);
    }
    public void SetDie()
    {
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        animator.SetTrigger(hashDie);

    }
    public void SetReload()
    {
        this.animator.SetTrigger(hashReload);
    }
    public void SetGameOver()
    {
        this.animator.SetTrigger(hashPlayerDie);
    }
}
