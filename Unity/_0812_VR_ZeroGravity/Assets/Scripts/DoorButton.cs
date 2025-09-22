using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorButton : MonoBehaviour
{
    public Animator doorAnim;
    private readonly string doorOpenPram = "Button_Pressed";
    private XRSimpleInteractable interactable;
    public Transform buttonOffset;

    private void Start()
    {
        this.interactable = GetComponent<XRSimpleInteractable>();
        this.interactable.firstHoverEntered.AddListener((args) =>
        {
            Debug.Log("호버 시작");
            StartCoroutine(ButtonActiveRoutine());
        });
        this.interactable.lastHoverExited.AddListener((args) =>
        {
            StopAllCoroutines();
            Debug.Log("호버 끝");
        });
    }
    IEnumerator ButtonActiveRoutine()
    {
        //업데이트에서 계속 체크하는게 아닌 hover라도 해서 누르기 직전일때부터 시작
        while (true)//이거 안해놓으니 hover인 상태에서 버튼을 여러번 누르면 한번만 작동함
        {
            Debug.Log("클릭 루틴 시작");
            while (this.buttonOffset.localPosition.y > 0)
            {
                //손만 대고 아직 안눌렀다면
                //눌릴때까지 프레임 넘김
                yield return null;
            }
            //눌렀다면
            Debug.Log("버튼 눌림");
            this.doorAnim.SetBool(doorOpenPram, !this.doorAnim.GetBool(this.doorOpenPram));
            while (this.buttonOffset.localPosition.y <= 0)
            {
                //손을 떼고 버튼이 올라올때까지 프레임 넘김
                yield return null;
            }
            Debug.Log("클릭 루틴 한번 끝");
        }
    }
}
