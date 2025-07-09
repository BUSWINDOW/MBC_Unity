using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApachiAttack : MonoBehaviour
{
    public Transform[] firePoses; // 발사 위치 배열
    public LaserBeam[] beams; // 레이저 빔 배열
    public float maxDistance = 100f; //레이저 최대 거리

    public GameObject expEffect; //터지는 이펙트
    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip expClip;

    Ray ray;
    int TerrainLayer;

    bool isHit = false; // 레이저가 맞았는지 여부

    void Start()
    {
        this.beams = new LaserBeam[this.firePoses.Length];
        for (int i = 0; i < this.firePoses.Length; i++)
        {
            this.beams[i] = this.firePoses[i].GetComponentInChildren<LaserBeam>();
        }
        this.source = this.GetComponent<AudioSource>();
        this.expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        this.fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        this.expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");

    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            this.source.PlayOneShot(this.fireClip);
            RaycastHit hit;



            for (int i = 0; i < this.firePoses.Length; i++)
            {
                this.beams[i].FireRay();
                ray = new Ray(this.firePoses[i].position, this.firePoses[i].forward);
                if (Physics.Raycast(ray, out hit, this.maxDistance, 1 << LayerMask.NameToLayer("Terrain")))
                {
                    this.isHit = true;
                }
                else
                {
                    this.isHit = false;
                }
                ShowEffect(this.firePoses[i], hit);
            }
        }

    }
    void ShowEffect(Transform firePos,RaycastHit hit)
    {
        Vector3 normal;
        Quaternion rot; //맞은 방향의 반대로 이펙트가 나와야 잘 보임
        GameObject effect;
        if (isHit)
        {
            normal = (firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, hit.point, rot);

            Destroy(effect, 2f); //2초 후에 이펙트 제거
            this.source.PlayOneShot(this.expClip);
        }
        else // 만약 안맞으면, 사정거리까지 날아간 뒤 알아서 터짐
        {
            var point = this.ray.GetPoint(this.maxDistance);
            normal = (firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, point, rot);

            Destroy(effect, 2f); //2초 후에 이펙트 제거
            this.source.PlayOneShot(this.expClip);
        }
    }
}
