using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TankDamage : MonoBehaviourPun
{
    //��ũ�� 3���� ������ ���
    //�޽��� ���� ���ٰ� , 5�ʵ� �ٽ� �Ѽ� ��Ȱ
    public Image hpBar; // ��ũ�� ü�¹� UI

    [SerializeField]
    private MeshRenderer[] meshRenderers; // ��ũ�� �޽� ��������
    private readonly string tankTag = "Player"; // ��ũ �±�
    private readonly string apacheTag = "Apache"; // ����ġ �±�
    private GameObject expEffect; // ���� ����Ʈ

    private readonly int maxHp = 100; // �ִ� ü��
    private int curHp; // ���� ü��

    void Start()
    {
        this.expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        this.curHp = this.maxHp; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        this.meshRenderers = GetComponentsInChildren<MeshRenderer>(); // ��ũ�� ��� �޽� �������� ������
    }
    public void OnDamage(string tag)
    {

        photonView.RPC("OnDamageRPC", RpcTarget.All, tag); // ��� Ŭ���̾�Ʈ�� ������ ����
        Debug.Log("OnDamage called with tag: " + tag);

    }
    [PunRPC]
    void OnDamageRPC(string tag) 
    {
        if (this.curHp > 0 && (tag == this.tankTag||tag == this.apacheTag)) 
        {
            //������ ����
            HpBarInit(tag); // ü�¹� UI ������Ʈ
            if(this.curHp <= 0)
            {
                //�ı� ����(����Ʈ)
                StartCoroutine(Explosion());
            }
        }
    }
    WaitForSeconds ws = new WaitForSeconds(5f);
    IEnumerator Explosion()
    {
        var eff = Instantiate(this.expEffect, this.transform.position, Quaternion.identity);
        Destroy(eff, 2f); // ���� ����Ʈ ����
        this.GetComponent<BoxCollider>().enabled = false; // ��ũ�� �浹ü�� ��Ȱ��ȭ
        this.GetComponent<Rigidbody>().isKinematic = true; // ��ũ�� ���� ������ ��Ȱ��ȭ
        SetTankVisible(false); // ��ũ �޽��� ��Ȱ��ȭ

        this.gameObject.tag = "Untagged"; // ��ũ�� �±׸� ���±׷� ����
        ScriptsCtrl(false);
        yield return ws; // ��� ���
        SetTankVisible(true); // ��ũ �޽��� �ٽ� Ȱ��ȭ
        this.GetComponent<BoxCollider>().enabled = true; // ��ũ�� �浹ü�� �ٽ� Ȱ��ȭ
        this.GetComponent<Rigidbody>().isKinematic = false; // ��ũ�� ���� ������ �ٽ� Ȱ��ȭ
        this.curHp = this.maxHp; // ��Ȱ �� ü���� �ִ� ü������ �ʱ�ȭ
        this.hpBar.fillAmount = (float)this.curHp / (float)this.maxHp; // ü�¹� UI ������Ʈ


        this.gameObject.tag = this.tankTag; // ��ũ�� �±׸� �ٽ� ��ũ �±׷� ����
        ScriptsCtrl(true); // ��ũ�� MonoBehaviourPun ��ũ��Ʈ�� �ٽ� Ȱ��ȭ
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
            meshRenderer.enabled = isVisible; // �޽� �������� Ȱ��ȭ ���¸� ����
        }
        if (isVisible)
        {
            this.curHp = this.maxHp; // ��Ȱ �� ü���� �ִ� ü������ �ʱ�ȭ
            //HpBarInit(this.tankTag); // ü�¹� UI ������Ʈ
        }
    }
    void HpBarInit(string tag)
    {
        if(tag == tankTag)
        {
            this.curHp -= 40;
        }
        this.hpBar.fillAmount = (float)this.curHp / (float)this.maxHp; // ü�¹� UI ������Ʈ
    }
}
