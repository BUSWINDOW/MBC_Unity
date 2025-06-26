using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwatCtrl : MonoBehaviour
{
    int hp;
    int max_Hp;
    private SwatAI ai;
    private SwatDamage damage;
    private SwatFire e_Fire;
    private MoveAgent moveAgent;
    private SwatAnimationCtrl animCtrl;

    private TPSPlayerCtrl player;

    private bool isDie;
    private WaitForSeconds ws = new WaitForSeconds(3);


    public Image hpBarImage;
    public Image hpBarDimImage;
    public GameObject hpBar;
    [Header("HpBar")]
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    
    void Awake()
    {


        this.max_Hp = 30;
        this.hp = this.max_Hp;
        this.ai = GetComponent<SwatAI>();
        this.damage = GetComponent<SwatDamage>();
        this.moveAgent = GetComponent<MoveAgent>();

        this.damage.hitAction = (dmg) =>
        {
            this.hp -= dmg;
            this.hpBarImage.fillAmount = (float)this.hp / (float)this.max_Hp;
            if (this.hp <= 0)
            {
                this.hp = 0;
                this.isDie = true;
                this.ai.state = EnemyAI.eState.Die;
                this.Die();
                
            }
        };

        

        this.e_Fire = GetComponent<SwatFire>();
        this.animCtrl = GetComponent<SwatAnimationCtrl>();
    }
    private void Start()
    {
        StartCoroutine(this.SetHPBar());
    }
    IEnumerator SetHPBar()
    {
        yield return null;


        hpBar = PoolingManager.Instance.GetHPBar();
        hpBar.gameObject.SetActive(true);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarDimImage = hpBar.GetComponentsInChildren<Image>()[0];


        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.transform;
        _hpBar.offset = this.hpBarOffset;
    }
    private void OnEnable()
    {
        TPSPlayerCtrl.onPlayerDie += this.PlayerDie;
        //BarrelCtrl.explodAction += this.Die;
    }
    private void OnDisable()
    {
        TPSPlayerCtrl.onPlayerDie -= this.PlayerDie;
        //BarrelCtrl.explodAction -= this.Die;

    }


    IEnumerator DieRoutine()
    {
        yield return ws;
        this.gameObject.SetActive(false);
        this.hp = this.max_Hp;
        this.isDie = false;
        this.animCtrl.SetPatroll(this.moveAgent.Speed);
        this.GetComponent<CapsuleCollider>().enabled = true;
        this.ai.state = EnemyAI.eState.Patrol;
        this.ai.isDie = false;

        this.hpBarImage.color = Color.red;
        this.hpBarDimImage.color = Color.black;

        this.hpBarImage.fillAmount = 1;
        this.hpBar.gameObject.SetActive(true);
    }
    private void Die()
    {
        //Debug.Log("Die");
        this.ai.isDie = true;
        this.e_Fire.isFire = false;
        this.e_Fire.isReload = false;
        this.moveAgent.agent.isStopped = true;
        this.animCtrl.SetDie();
        this.GetComponent<CapsuleCollider>().enabled = false;

        this.hpBarImage.color = Color.clear;
        this.hpBarDimImage.color = Color.clear;
        //hpBar.gameObject.SetActive(false);

        StartCoroutine(this.DieRoutine());
        GameManager.Instance.IncKillCnt();
    }

    public void PlayerDie()
    {
        this.e_Fire.isFire = false;
        this.moveAgent.Stop();
        this.ai.StopAllCoroutines();
        this.animCtrl.SetGameOver();
    }
}
