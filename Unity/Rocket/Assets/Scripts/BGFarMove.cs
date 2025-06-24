using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFarMove : MonoBehaviour
{
    public float speed;
    public Transform tr;
    public BoxCollider2D box2D;
    private float width;//폭 너비
    void Start()
    {
        tr = GetComponent<Transform>();
        box2D = GetComponent<BoxCollider2D>();
        speed =8f;
        width = box2D.size.x; //박스콜라이더의 사이즈 x값을 너비로 지정
    }
    void Update()
    {
        if (GameManager.instance.isGameOver == true) //게임오버가 되면
        {
            return; //여기서 하위로 로직으로 진행 되지 않음
        }

        if (tr.position.x <= -width*1.8f) //트랜스폼의 포지션 x값이 너비보다 작으면
        {
            RePosition();
        }
        tr.Translate(Vector3.left * speed * Time.deltaTime);
       

    }

    private void RePosition()
    {
        Vector2 ofsset = new Vector2(width * 2.5f, 0f); //오프셋을 너비의 2.5f로 지정
        tr.position = (Vector2)tr.position + ofsset; //트랜스폼의 포지션에 오프셋을 더함
    }
}
