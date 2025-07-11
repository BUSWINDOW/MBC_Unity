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

    public GameObject expEffect; //터지는 이펙트
    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip expClip;

    public float maxDistance = 100f; //레이저 최대 거리

    private readonly string tankTag = "Player"; //탱크 태그
    private readonly string apacheTag = "Apache"; //아파치 태그

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
            if (photonView.IsMine) // 로컬이라면
            {
                Fire(); //그냥 내 Fire호출
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
        Quaternion rot; //맞은 방향의 반대로 이펙트가 나와야 잘 보임
        GameObject effect;
        if (isHit)
        {
            normal = (this.firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, hit.point, rot);

            Destroy(effect, 2f); //2초 후에 이펙트 제거
            this.source.PlayOneShot(this.expClip);
        }
        else // 만약 안맞으면, 사정거리까지 날아간 뒤 알아서 터짐
        {
            var point = this.ray.GetPoint(this.maxDistance);
            normal = (this.firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, point, rot);

            Destroy(effect, 2f); //2초 후에 이펙트 제거
            this.source.PlayOneShot(this.expClip);
        }
    }

}
