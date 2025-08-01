using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ApacheAI_Attack : MonoBehaviourPun
{
    [SerializeField] public Transform firePosL;
    [SerializeField] public Transform firePosR;
    [SerializeField] public LaserBeam leaserBeamL;
    [SerializeField] public LaserBeam leaserBeamR;

    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject expEffect;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip expClip;
    [SerializeField] private int terrainLayer;
    [SerializeField] private int tankLayer;

    Ray ray;
    void Start()
    {
        /*firePosL = transform.GetChild(3).GetChild(0).GetComponent<Transform>();
        firePosR = transform.GetChild(3).GetChild(1).GetComponent<Transform>();
        leaserBeamL = firePosL.GetComponentInChildren<LaserBeam>();
        leaserBeamR = firePosR.GetComponentInChildren<LaserBeam>();

        //source = GetComponent<AudioSource>();
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");*/
        terrainLayer = LayerMask.NameToLayer("Terrain");
        tankLayer = LayerMask.NameToLayer("Tank");
    }

    public void Fire(Transform firePos, LaserBeam leaserBeam)
    {
        source.PlayOneShot(fireClip, 1.0f);
        //SoundManager.s_Instance.PlaySfx(transform.position, fireClip, false);
        bool isHit;

        RaycastHit hit;
        ray = new Ray(firePos.position, firePos.forward);
        if (Physics.Raycast(ray, out hit, 200f, 1 << terrainLayer | 1 << tankLayer))
        {
            isHit = true;
            if (photonView.IsMine && hit.collider.CompareTag("Player"))
            {
                string tag = hit.collider.tag;
                if (PhotonNetwork.IsMasterClient)
                    hit.collider.gameObject.SendMessage("OnDamage", tag, SendMessageOptions.DontRequireReceiver);
            }
        }
            
        else
            isHit = false;

        leaserBeam.FireRay(); // 라인 랜더러
        ShowEffect(hit, firePos, isHit);
    }

    void ShowEffect(RaycastHit hit, Transform firePos, bool isHit)
    {
        Vector3 hitPoint;

        if (isHit)
            hitPoint = hit.point;
        else
            hitPoint = ray.GetPoint(200f);

        Vector3 _normal = (firePos.position - hitPoint).normalized;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        //var eff = PoolingManager.p_Instance.GetExp();
        /*eff.transform.position = hitPoint;
        eff.transform.rotation = rot;
        StartCoroutine(ExpEffect(eff));*/

        var eff = Instantiate(expEffect, hitPoint, rot);
        Destroy(eff, 1.5f);
        source.PlayOneShot(expClip, 1.0f);
        //SoundManager.s_Instance.PlaySfx(hitPoint, expClip, false);

    }

    IEnumerator ExpEffect(GameObject eff)
    {
        eff.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        eff.gameObject.SetActive(false);
    }
}
