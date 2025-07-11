using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class FireCannon : MonoBehaviourPun
{
    public Transform firePos;
    private TankInput input;
    [SerializeField]
    private LaserBeam beam;

    public GameObject expEffect; //������ ����Ʈ
    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip expClip;

    public float maxDistance = 100f; //������ �ִ� �Ÿ�

    private readonly string tankTag = "Player"; //��ũ �±�
    private readonly string apacheTag = "Apache"; //����ġ �±�

    Ray ray;
    int TerrainLayer;

    public bool isHit = false;
    void Start()
    {
        this.input = this.GetComponent<TankInput>();
        this.beam = this.GetComponentInChildren<LaserBeam>();
        this.firePos = this.transform.GetChild(4).GetChild(1).GetChild(1);
        this.source = this.GetComponent<AudioSource>();
        this.expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        this.fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        this.expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");
        this.TerrainLayer = LayerMask.NameToLayer("Terrain"); 
    }

    void Update()
    {
        if (this.input.isFire)
        {
            if (photonView.IsMine) // �����̶��
            {
                Fire(); //�׳� �� Fireȣ��
                photonView.RPC("Fire", RpcTarget.Others);
            }

        }
    }
    [PunRPC]
    void Fire()
    {
        this.source.PlayOneShot(this.fireClip);
        this.beam.FireRay();
        RaycastHit hit;

        this.ray = new Ray(this.firePos.position, this.firePos.forward);
        if(Physics.Raycast(ray,out hit, this.maxDistance, 1<< this.TerrainLayer|1<<7))
        {
            isHit = true;
            if (hit.collider.CompareTag(tankTag))
            {
                string tag = hit.collider.tag;
                hit.collider.gameObject.SendMessage("OnDamage", tag, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            isHit = false;
        }
        this.ShowEffect(hit);
        
    }
    void ShowEffect(RaycastHit hit)
    {
        Vector3 normal;
        Quaternion rot; //���� ������ �ݴ�� ����Ʈ�� ���;� �� ����
        GameObject effect;
        if (isHit)
        {
            normal = (this.firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, hit.point, rot);

            Destroy(effect, 2f); //2�� �Ŀ� ����Ʈ ����
            this.source.PlayOneShot(this.expClip);
        }
        else // ���� �ȸ�����, �����Ÿ����� ���ư� �� �˾Ƽ� ����
        {
            var point = this.ray.GetPoint(this.maxDistance);
            normal = (this.firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, point, rot);

            Destroy(effect, 2f); //2�� �Ŀ� ����Ʈ ����
            this.source.PlayOneShot(this.expClip);
        }
    }

}
