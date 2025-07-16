using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WomanShooter : MonoBehaviourPun
{
    public Gun gun; // ���� �����ϱ� ���� ����
    public Transform gunPivot; // ���� ȸ�� �߽���
    public Transform leftHandleMount; // �޼��� ��� ��ġ
    public Transform rightHandleMount; // �������� ��� ��ġ

    private WomanInput input; // �÷��̾� �Է��� ó���ϴ� ��ũ��Ʈ
    private Animator animator; // �ִϸ����� ������Ʈ

    private readonly string reloadParam = "Reload"; // ������ �ִϸ��̼� �Ķ���� �̸�

    private void OnEnable()
    {
        this.gun.gameObject.SetActive(true); // ���� Ȱ��ȭ
    }
    void Awake()
    {
        this.input = GetComponent<WomanInput>();
        this.animator = GetComponent<Animator>();
        this.gun = GetComponentInChildren<Gun>(); // �ڽ� ������Ʈ���� Gun ������Ʈ�� ã��

        this.gun.reloadAnimationAction = () =>
        {
            this.animator.SetTrigger(this.reloadParam); // ������ �ִϸ��̼� Ʈ���� ����
        };
    }
    private void OnDisable()
    {
        this.gun.gameObject.SetActive(false); // ���� ��Ȱ��ȭ
    }
    void Update()
    {
        if (!photonView.IsMine) return; // ���� �÷��̾ �ƴ� ��� ��� X
        //input�� ���� �޾Ƽ� �߻��ϰų� ������
        if (this.input.Fire)
        {
            this.gun.Fire(); // �Է��� ������ �� �߻�
        }
        else if (this.input.Reload)
        {
            if (this.gun.Reload())// �Է��� ������ ������
            {
                //this.animator.SetTrigger("Reload"); // ������ �ִϸ��̼� Ʈ���� ����
            }
            else
            {
                //�������� ���� �� �Ҹ��� ���ų�, �׿� ���� �˸� ó��
            }
        }
        this.UpdateUI(); // UI ������Ʈ ȣ��
    }
    void UpdateUI()
    {
        if(UIManager.Instance != null)
        {
            // UIManager�� Ȱ��ȭ�Ǿ� ������ UI ������Ʈ
            UIManager.Instance.UpdateAmmoTxt(this.gun.magAmmo, this.gun.ammo);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //animator IK �� ����Ͽ� �� ��ġ ����
        //�ִϸ������� �ǽð� IK������Ʈ
        this.gunPivot.position = this.animator.GetIKHintPosition(AvatarIKHint.RightElbow); // ������ ��ġ�� �� ȸ�� �߽��� ��ġ ����
        this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        // ������ IK ��ġ ����ġ ����
        this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        // ������ IK ȸ�� ����ġ ����
        this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        // �޼� IK ��ġ ����ġ ����
        this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        // �޼� IK ȸ�� ����ġ ����


        this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightHandleMount.position);
        this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftHandleMount.position);

        this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightHandleMount.rotation);
        this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftHandleMount.rotation);


    }
}
