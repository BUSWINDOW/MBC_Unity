using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SwatFov))] // EnemyFov ��ũ��Ʈ�� ���� Ŀ���� ������
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        SwatFov fov = (SwatFov)target; //FoV ��ũ��Ʈ�� �ν��Ͻ��� �����´�

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f); //���� ���� �������� ��ǥ�� ��� (�־��� ������ ���ݸ�ŭ�� �¿��
                                                                       //�������� ������ ����) �������� ���߸� ������ �����ϱ� ������
                                                                       //������ �ȸ��� �� 



        Handles.color = Color.white; //�þ߰��� ǥ���ϱ� ���� �ڵ��� ����

        Handles.DrawSolidArc( //������ �׸��� �Լ�
            fov.transform.position, //���� �߽� ��ǥ
            Vector3.up, //ȸ�� ���� ��(y��)
            fromAnglePos, // ��ä���� ������ ��ǥ
            fov.viewAngle, //�þ߰� ��ä���� ����
            fov.viewRange //��ä���� ������
        );

        Handles.Label( // �� ȭ�鿡 ���� �߰��ؼ� �ؽ�Ʈ�� ǥ���ϰ� ���ִ� �Լ�
            fov.transform.position + Vector3.up * 2f, // �� ��ġ
            "Fov Range : " + fov.viewRange + "\n" + "Fov Angle : " + fov.viewAngle //���� ��
            );


    }
}
