using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApachiAttack : MonoBehaviour
{
    public Transform[] firePoses; // �߻� ��ġ �迭
    public LaserBeam[] beams; // ������ �� �迭
    public float maxDistance = 100f; //������ �ִ� �Ÿ�

    public GameObject expEffect; //������ ����Ʈ
    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip expClip;

    Ray ray;
    int TerrainLayer;

    bool isHit = false; // �������� �¾Ҵ��� ����

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
        Quaternion rot; //���� ������ �ݴ�� ����Ʈ�� ���;� �� ����
        GameObject effect;
        if (isHit)
        {
            normal = (firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, hit.point, rot);

            Destroy(effect, 2f); //2�� �Ŀ� ����Ʈ ����
            this.source.PlayOneShot(this.expClip);
        }
        else // ���� �ȸ�����, �����Ÿ����� ���ư� �� �˾Ƽ� ����
        {
            var point = this.ray.GetPoint(this.maxDistance);
            normal = (firePos.position - hit.point).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);
            effect = Instantiate(this.expEffect, point, rot);

            Destroy(effect, 2f); //2�� �Ŀ� ����Ʈ ����
            this.source.PlayOneShot(this.expClip);
        }
    }
}
