using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TankDamage : MonoBehaviourPun
{
    //탱크가 3대쯤 맞으면 사망
    //메쉬를 전부 껐다가 , 5초뒤 다시 켜서 부활
    public Image hpBar; // 탱크의 체력바 UI

    [SerializeField]
    private MeshRenderer[] meshRenderers; // 탱크의 메쉬 렌더러들
    private readonly string tankTag = "Player"; // 탱크 태그
    private readonly string apacheTag = "Apache"; // 아파치 태그
    private GameObject expEffect; // 폭발 이펙트

    private readonly int maxHp = 100; // 최대 체력
    private int curHp; // 현재 체력

    void Start()
    {
        this.expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        this.curHp = this.maxHp; // 현재 체력을 최대 체력으로 초기화
        this.meshRenderers = GetComponentsInChildren<MeshRenderer>(); // 탱크의 모든 메쉬 렌더러를 가져옴
    }
    public void OnDamage(string tag)
    {

        photonView.RPC("OnDamageRPC", RpcTarget.All, tag); // 모든 클라이언트에 데미지 전달
        Debug.Log("OnDamage called with tag: " + tag);

    }
    [PunRPC]
    void OnDamageRPC(string tag) 
    {
        if (this.curHp > 0 && (tag == this.tankTag||tag == this.apacheTag)) 
        {
            //데미지 전달
            HpBarInit(tag); // 체력바 UI 업데이트
            if(this.curHp <= 0)
            {
                //파괴 연출(이펙트)
                StartCoroutine(Explosion());
            }
        }
    }
    WaitForSeconds ws = new WaitForSeconds(5f);
    IEnumerator Explosion()
    {
        var eff = Instantiate(this.expEffect, this.transform.position, Quaternion.identity);
        Destroy(eff, 2f); // 폭발 이펙트 제거
        this.GetComponent<BoxCollider>().enabled = false; // 탱크의 충돌체를 비활성화
        this.GetComponent<Rigidbody>().isKinematic = true; // 탱크의 물리 엔진을 비활성화
        SetTankVisible(false); // 탱크 메쉬를 비활성화

        this.gameObject.tag = "Untagged"; // 탱크의 태그를 언태그로 변경
        ScriptsCtrl(false);
        yield return ws; // 잠시 대기
        SetTankVisible(true); // 탱크 메쉬를 다시 활성화
        this.GetComponent<BoxCollider>().enabled = true; // 탱크의 충돌체를 다시 활성화
        this.GetComponent<Rigidbody>().isKinematic = false; // 탱크의 물리 엔진을 다시 활성화
        this.curHp = this.maxHp; // 부활 시 체력을 최대 체력으로 초기화
        this.hpBar.fillAmount = (float)this.curHp / (float)this.maxHp; // 체력바 UI 업데이트


        this.gameObject.tag = this.tankTag; // 탱크의 태그를 다시 탱크 태그로 변경
        ScriptsCtrl(true); // 탱크의 MonoBehaviourPun 스크립트를 다시 활성화
    }

    private void ScriptsCtrl(bool onoff)
    {
        var script = this.GetComponentsInChildren<MonoBehaviourPun>();
        foreach (var s in script)
        {
            s.enabled = onoff;
        }
    }

    void SetTankVisible(bool isVisible)
    {
        foreach (var meshRenderer in this.meshRenderers)
        {
            meshRenderer.enabled = isVisible; // 메쉬 렌더러의 활성화 상태를 설정
        }
        if (isVisible)
        {
            this.curHp = this.maxHp; // 부활 시 체력을 최대 체력으로 초기화
            //HpBarInit(this.tankTag); // 체력바 UI 업데이트
        }
    }
    void HpBarInit(string tag)
    {
        if(tag == tankTag)
        {
            this.curHp -= 40;
        }
        this.hpBar.fillAmount = (float)this.curHp / (float)this.maxHp; // 체력바 UI 업데이트
    }
}
