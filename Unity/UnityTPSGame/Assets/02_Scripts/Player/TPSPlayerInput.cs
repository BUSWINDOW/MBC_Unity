using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSPlayerInput : MonoBehaviour
{
    public string moveYAxisName = "Vertical";
    public string moveXAxisName = "Horizontal";
    public string rotateAxisName = "Mouse X";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";

    public bool isRun { get; private set; }

    public float MoveZ {  get; private set; }//������ �������� �Է°�
    public float MoveX { get; private set; }//������ ȸ���� �Է°�
    public float MouseX { get; private set; } //���콺 X�� ȸ����
    public bool Fire { get; private set; }
    public bool Reload { get; private set; }
    

    void Start()
    {
        
    }
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            MoveZ = 0; MoveX = 0; Fire = false; Reload = false; isRun = false;
            return;
        }
        MoveZ = Input.GetAxis(moveYAxisName);
        MoveX = Input.GetAxis(moveXAxisName);
        Fire = Input.GetButton(fireButtonName);
        Reload = Input.GetButtonDown(reloadButtonName);
        this.MouseX = Input.GetAxis(rotateAxisName);

        if (Input.GetKey(KeyCode.LeftShift)&& this.MoveZ > 0.1f)
        {
            this.isRun = true;
        }
        else
        {
            this.isRun = false;
        }
    }
}
