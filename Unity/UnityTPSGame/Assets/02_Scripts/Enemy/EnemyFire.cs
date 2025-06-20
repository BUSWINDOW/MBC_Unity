using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//1, 총알, 사운드 2, 플레이어 방향으로 사격 3. 사격 애니메이션, 4. 사격간 간격 5. 플레이어 방향으로 회전(damping)
//isReload = 재장전중 발사 제한
public class EnemyFire : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform firePos;
    [SerializeField] private MeshRenderer muzzleFlash;

    private readonly static int hashFire = Animator.StringToHash("Fire");
    private readonly static int hashReload = Animator.StringToHash("Reload");

    private readonly float reloadTime = 2.0f;
    private WaitForSeconds WS_reload;
    private readonly int maxBullet = 20;
    private int curBullet;
    public AudioClip reloadClip;
    

    private float damping = 7.0f;
    private float fireRate = 0.3f;
    private float nextTime = 0;
    public bool isReload = false;
    public bool isFire = false;

    [SerializeField] private Transform playerTr;
    void Start()
    {
        this.curBullet = this.maxBullet;
        this.playerTr = GameObject.FindWithTag("Player").transform;
        this.source= GetComponent<AudioSource>();
        this.animator = GetComponent<Animator>();
        this.nextTime = Time.time;
        this.WS_reload = new WaitForSeconds(this.reloadTime);
    }
    void Update()
    {
        if (this.GetComponent<EnemyAI>().isDie) return;
        if (this.isFire&&!this.isReload)
        {
            if (Time.time > this.nextTime)
            {
                if (this.GetComponent<EnemyAI>().isDie) return;
                this.Fire();
                nextTime = Time.time + this.fireRate + Random.Range(0, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(this.playerTr.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, damping * Time.deltaTime);
        }
    }
    void Fire()
    {
        var bullet = PoolingManager.Instance.GetE_Bullet();

        bullet.transform.position = this.firePos.position;
        bullet.transform.rotation = this.firePos.rotation;
        bullet.SetActive(true);

        this.animator.SetTrigger(hashFire);
        this.source.PlayOneShot(this.fireSfx);
        StartCoroutine(this.ShowMuzzleRoutine());

        isReload = (--curBullet == 0);
        if (isReload) 
        {
            StartCoroutine(this.Reloading());
        }
    }
    IEnumerator ShowMuzzleRoutine()
    {
        this.muzzleFlash.enabled = true;
        this.muzzleFlash.transform.localScale = Vector3.one * Random.Range(0.5f, 1.2f);
        var rot = Quaternion.Euler(Vector3.forward * Random.Range(0.0f, 360));
        this.muzzleFlash.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.15f));
        this.muzzleFlash.enabled = false; ;
    }
    IEnumerator Reloading()
    {
        this.animator.SetTrigger(hashReload);
        this.source.PlayOneShot(this.reloadClip);
        yield return this.WS_reload;
        this.isReload = false;
        this.curBullet = this.maxBullet;
    }
}
