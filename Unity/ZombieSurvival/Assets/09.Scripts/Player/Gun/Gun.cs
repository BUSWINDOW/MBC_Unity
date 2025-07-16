using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviourPun, IPunObservable
{
    public enum eState
    {
        Ready, // ��� ����
        Fire, // ��� ��
        Reload // ���� ��
    }
    //public eState state = eState.Ready; // ���� ���� ����

    public eState State { get; private set; } = eState.Ready; // ���� ���� ���� (�ڵ� ������Ƽ�� ����)
    //���� ���� ���¸� ��Ÿ���� ����, �⺻���� Ready
    public Transform firePos; // �Ѿ��� �߻�Ǵ� ��ġ
    public ParticleSystem muzzleFlashEffect; // �ѱ� �÷��� ����Ʈ
    public ParticleSystem shellEffect; // ź�� ����Ʈ

    private LineRenderer lineRenderer; // �Ѿ� ������ �׸��� ���� ���� ������

    public GunData gunData; // �� ������ ��ũ���ͺ� ������Ʈ

    private float fireDistance = 100f; // �Ѿ��� ���ư��� �ִ� �Ÿ�(�����Ÿ�)

    public int ammo { get; set; } // ���� �������� �Ѿ� ��
    public int magAmmo { get; private set; } // ���� źâ �� �Ѿ� ��

    public float lastShotTime = 0f; // ������ ��� �ð�
    private Vector3 hitPos; // �Ѿ��� ���� ��ġ

    private AudioSource source;

    public Action reloadAnimationAction;

    
    void Awake()
    {
        this.source = GetComponent<AudioSource>();

        this.lineRenderer = GetComponent<LineRenderer>();
        this.lineRenderer.positionCount = 2; // ���� �������� ������ ������ 2�� ���� (�������� ����)
        this.lineRenderer.enabled = false; // �ʱ⿡�� ���� ������ ��Ȱ��ȭ

        this.ammo = gunData.maxAmmo; // �Ѿ� ���� �ʱ�ȭ
        this.magAmmo = gunData.magCapacity; // źâ �� �Ѿ� ���� �ʱ�ȭ
        this.ammo -= this.magAmmo; // �ʱ�ȭ �� ���� �Ѿ� �� ���

        this.shotEffectTime = new WaitForSeconds(0.03f);
        this.reloadTime = new WaitForSeconds(gunData.reloadTime); // ���� �ð� �ʱ�ȭ
    }

    private void Start()
    {
        photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
        //�����Ͱ� �߿��� �����̹Ƿ� TCP/IP�� ����
        photonView.ObservedComponents[0] = this; // �̹� ���ֱ�������, �ڵ�� �׳� ��а�
    }

    private void OnEnable()
    {
        // �÷��̾� �� �� ������ �� �� �����ͷ� ����
        this.State = eState.Ready; // ���� ���¸� Ready�� �ʱ�ȭ
        this.lastShotTime = 0f; // ������ ��� �ð��� �ʱ�ȭ
    }
    public void Fire()
    {
        // �߻�
        if (this.State == eState.Ready && Time.time >= this.lastShotTime + this.gunData.timeBetweenShots)
        {
            this.lastShotTime = Time.time;
            this.Shot();
        }
        
    }

    private void Shot()
    {
        //��Ƽ�����϶� ��� ó���� RPC�� ���ؼ� �Ѵ�.
        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient); // ������ Ŭ���̾�Ʈ�� Shot ����, �������� ����ȭ ��Ŵ
        if (this.magAmmo <= 0)
        {
            this.Reload(); // źâ �� �Ѿ��� 0�� �Ǹ� ���� ����
            return;
        }
        else
        {
            this.magAmmo--; // źâ �� �Ѿ� �� ����
        }
        #region �̱� �����϶� ��� ó�� �κ�
        /*RaycastHit hit;
        Vector3 hitPos = Vector3.zero; // �Ѿ��� ���� ��ġ �ʱ�ȭ
        if (Physics.Raycast(this.firePos.position, this.firePos.forward, out hit, this.fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                
                target.OnDamage(gunData.damage, hit.point, hit.normal); // ���� ��󿡰� ������ ����
            }
            hitPos = hit.point; // ���� ��ġ�� ����
        }
        else
        {
            hitPos = this.firePos.position + (this.firePos.forward * this.fireDistance); // ���� ��ġ�� ������ �����Ÿ� ������ ����
        }
        StartCoroutine(this.ShotEffect(hitPos)); // �ѱ� �÷��ÿ� ���� ������ ����Ʈ ���*/
        #endregion
    }
    [PunRPC]
    private void ShotProcessOnServer()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero; // �Ѿ��� ���� ��ġ �ʱ�ȭ

        if (Physics.Raycast(this.firePos.position, this.firePos.forward, out hit, this.fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal); // ���� ��󿡰� ������ ����
            }

            hitPos = hit.point; // ���� ��ġ�� ����
        }
        else
        {
            //�ȸ¾Ҵٸ�
            hitPos = this.firePos.position + (this.firePos.forward * this.fireDistance); // ���� ��ġ�� ������ �����Ÿ� ������ ����
        }

        //�߻� ����Ʈ ���(��� Ŭ���̾�Ʈ����)
        photonView.RPC("ShotEffectOnServer", RpcTarget.All, hitPos); // ��� Ŭ���̾�Ʈ���� ShotEffectOnServer ����
    }

    [PunRPC]
    private void ShotEffectOnServer(Vector3 hitPos)
    {
        StartCoroutine(this.ShotEffect(hitPos)); // �ѱ� �÷��ÿ� ���� ������ ����Ʈ ���
    }

    private WaitForSeconds shotEffectTime;
    IEnumerator ShotEffect(Vector3 hitPos)
    {
        this.muzzleFlashEffect.Play(); // �ѱ� �÷��� ����Ʈ ���
        this.shellEffect.Play(); // ź�� ����Ʈ ���
        this.source.PlayOneShot(gunData.shotClip); // ��� ���� ���

        this.lineRenderer.SetPosition(0, this.firePos.position); // ���� �������� �������� �ѱ� ��ġ�� ����
        this.lineRenderer.SetPosition(1, hitPos); 
        // ���� �������� ������ �ѱ� ��ġ���� �����Ÿ���ŭ �������� ����

        this.lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ
        yield return this.shotEffectTime;
        this.lineRenderer.enabled = false; // ���� ������ ��Ȱ��ȭ
    }
    public bool Reload()
    {
        if (this.State == eState.Reload || this.ammo <= 0 || this.magAmmo >= this.gunData.magCapacity)
            return false;// �̹� ���� ���̰ų�, ���� �Ѿ��� ���ų�, źâ�� ���� á�� ���� ���� �Ұ�

        StartCoroutine(ReloadRoutine()); // ���� �ڷ�ƾ ����
        return true; // ���� ���� ����
    }
    private WaitForSeconds reloadTime; // ���� �ð�
    IEnumerator ReloadRoutine()
    {
        this.State = eState.Reload; // ���¸� Reload�� ����
        this.reloadAnimationAction(); // ������ �ִϸ��̼� Ʈ���� ȣ��
        this.source.PlayOneShot(gunData.reloadClip); // ���� ���� ���
        yield return this.reloadTime; // ���� �ð���ŭ ���
        int ammoToReload = Mathf.Min(gunData.magCapacity - this.magAmmo, ammo); // �ִ� źâ �뷮�� �Ǳ����� �ʿ��� �Ѿ� �� �Ǵ� ���� �Ѿ� �� �� ���� ��
        magAmmo += ammoToReload; // źâ �� �Ѿ� �� ����
        ammo -= ammoToReload; // ���� �Ѿ� �� ����
        this.State = eState.Ready; // ���¸� Ready�� ����
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)this.State); // ���� �� ���¸� ����
            stream.SendNext(this.magAmmo); // ���� źâ �� �Ѿ� �� ����
            stream.SendNext(this.ammo); // ���� ���� ���� �Ѿ� �� ����
        }
        else
        {
            this.State = (eState)stream.ReceiveNext(); // �� ���¸� ����
            this.magAmmo = (int)stream.ReceiveNext(); // źâ �� �Ѿ� ���� ����
            this.ammo = (int)stream.ReceiveNext(); // ���� ���� �Ѿ� ���� ����
        }
    }

}
