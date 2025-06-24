using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGNearMove : MonoBehaviour
{
    public float speed;
    public Transform tr;
    public BoxCollider2D box2D;
    private float width;//�� �ʺ�
    void Start()
    {
        speed = 6f;
        tr = GetComponent<Transform>();
        box2D = GetComponent<BoxCollider2D>();
        width = box2D.size.x; //�ڽ��ݶ��̴��� ������ x���� �ʺ�� ����
    }
    void Update()
    {
        if(GameManager.instance.isGameOver == true) //���ӿ����� �Ǹ�
        {
            return; //���⼭ ������ �������� ���� ���� ����
        }
        if (tr.position.x <= -width * 1.8f) //Ʈ�������� ������ x���� �ʺ񺸴� ������
        {
            Vector2 ofsset = new Vector2(width * 1.5f, 0f); //�������� �ʺ��� 2.5f�� ����
            tr.position = (Vector2)tr.position + ofsset; //Ʈ�������� �����ǿ� �������� ����
        }
        tr.Translate(Vector3.left * speed * Time.deltaTime);

    }
}
