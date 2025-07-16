using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WomanShooter : MonoBehaviourPun
{
    public Gun gun; // 총을 참조하기 위한 변수
    public Transform gunPivot; // 총의 회전 중심점
    public Transform leftHandleMount; // 왼손이 잡는 위치
    public Transform rightHandleMount; // 오른손이 잡는 위치

    private WomanInput input; // 플레이어 입력을 처리하는 스크립트
    private Animator animator; // 애니메이터 컴포넌트

    private readonly string reloadParam = "Reload"; // 재장전 애니메이션 파라미터 이름

    private void OnEnable()
    {
        this.gun.gameObject.SetActive(true); // 총을 활성화
    }
    void Awake()
    {
        this.input = GetComponent<WomanInput>();
        this.animator = GetComponent<Animator>();
        this.gun = GetComponentInChildren<Gun>(); // 자식 오브젝트에서 Gun 컴포넌트를 찾음

        this.gun.reloadAnimationAction = () =>
        {
            this.animator.SetTrigger(this.reloadParam); // 재장전 애니메이션 트리거 설정
        };
    }
    private void OnDisable()
    {
        this.gun.gameObject.SetActive(false); // 총을 비활성화
    }
    void Update()
    {
        if (!photonView.IsMine) return; // 로컬 플레이어가 아닐 경우 사격 X
        //input쪽 내용 받아서 발사하거나 재장전
        if (this.input.Fire)
        {
            this.gun.Fire(); // 입력이 있으면 총 발사
        }
        else if (this.input.Reload)
        {
            if (this.gun.Reload())// 입력이 있으면 재장전
            {
                //this.animator.SetTrigger("Reload"); // 재장전 애니메이션 트리거 설정
            }
            else
            {
                //재장전을 못할 때 소리가 나거나, 그에 따른 알림 처리
            }
        }
        this.UpdateUI(); // UI 업데이트 호출
    }
    void UpdateUI()
    {
        if(UIManager.Instance != null)
        {
            // UIManager가 활성화되어 있으면 UI 업데이트
            UIManager.Instance.UpdateAmmoTxt(this.gun.magAmmo, this.gun.ammo);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //animator IK 를 사용하여 손 위치 조정
        //애니메이터의 실시간 IK업데이트
        this.gunPivot.position = this.animator.GetIKHintPosition(AvatarIKHint.RightElbow); // 오른손 위치에 총 회전 중심점 위치 설정
        this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        // 오른손 IK 위치 가중치 설정
        this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        // 오른손 IK 회전 가중치 설정
        this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        // 왼손 IK 위치 가중치 설정
        this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        // 왼손 IK 회전 가중치 설정


        this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightHandleMount.position);
        this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftHandleMount.position);

        this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightHandleMount.rotation);
        this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftHandleMount.rotation);


    }
}
