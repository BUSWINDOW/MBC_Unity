using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public enum eGrade
{
    C, B, A, S, V, SV, SSV
}
public class CatchUManager : MonoBehaviour
{
    public Dropdown gradeDropDown;
    private eGrade grade;


    [Header("첫번째 UI")]
    public GameObject firstUI;
    public CapDrag caseCap;
    public CanvasGroup firstOpenUI;

    [Header("V등급 이상일 때, 두번째 UI")]
    public GameObject secondUI;
    public CapDrag boxCap;
    public CanvasGroup secondOpenUI;

    [Header("이펙트")]
    public GameObject effectGO;
    private ParticleSystem[] effects;

    [Header("상품")]
    public GameObject awardImageGO;
    private readonly int hashGetAward = Animator.StringToHash("GetAward");
    public GameObject awardTextGO;
    public Text awardGradeTxt;
    public Text awardNameTxt;
    public Text awardPriceTxt;
    public Button awardGetBtn;

    void Start()
    {
        this.effects = this.effectGO.GetComponentsInChildren<ParticleSystem>();

        this.secondUI.SetActive(false);
        this.gradeDropDown.onValueChanged.AddListener((value) =>
        {
            this.grade = (eGrade)value;
            this.secondUI.SetActive(value > 3 ? true : false); // V등급 이상일 때만 2번째 UI를 활성화
        });



        #region 첫번째 오픈
        this.caseCap.OnDragAction += (height) =>
        {
            //올라가는 수치 변화에 맞춰서 ui의 알파값이 변화되는등의 내용을 넣기
            this.firstOpenUI.alpha = (500 - height) / 500f;
        };

        this.caseCap.EndDragAction += () =>
        {
            //드래그가 끝났을때, 알파값 등을 마저 조절하는 내용

            this.firstOpenUI.DOFade(0,2).SetSpeedBased(); // SetSpeedBased : 단위 시간당 저정도 수치만큼 움직이게 한다.
                // 즉 Drag쪽에서 총 500올려야하는거 스피드 수치에 1000(2배)을 줬기때문에, Alpha값은 총 1만큼 바뀌므로,
                // 스피드에 2배값인 2를 입력하면 자연스럽게 바뀌게 된다.
            
        };

        this.caseCap.DragCompleteAction += () =>
        {
            // Drag가 완전히 위로 올라간 다음 실행될 액션
            this.firstUI.SetActive(false);
            if((int)this.grade > 3) // V등급 이상일 경우
            {
                EffectColorChange(this.grade);
            }
            else // 아닐 경우
            {
                this.AwardAppear();
            }
        };

        this.caseCap.NotEndDragAction += () =>
        {
            //충분히 올려지지 않아서 원래 위치로 돌아갈때, 알파값도 돌려놓음
            this.firstOpenUI.DOFade(1, 2).SetSpeedBased();
        };
        #endregion


        #region 두번째 오픈
        this.boxCap.OnDragAction += (height) =>
        {
            //올라가는 수치 변화에 맞춰서 ui의 알파값이 변화되는등의 내용을 넣기
            //Debug.Log(height);
            this.secondOpenUI.alpha = (500 - height) / 500f;
        };

        this.boxCap.EndDragAction += () =>
        {
            //드래그가 끝났을때, 알파값 등을 마저 조절하는 내용
            this.secondOpenUI.DOFade(0, 2).SetSpeedBased();
        };

        this.boxCap.DragCompleteAction += () =>
        {
            // Drag가 완전히 위로 올라간 다음 실행될 액션
            this.secondUI.SetActive(false);
            AwardAppear();

        };

        this.boxCap.NotEndDragAction += () =>
        {
            //충분히 올려지지 않아서 원래 위치로 돌아갈때, 알파값도 돌려놓음
            this.secondOpenUI.DOFade(1,2).SetSpeedBased();
        };

        #endregion

        this.awardGetBtn.onClick.AddListener(() =>
        {
            this.awardImageGO.GetComponent<Image>().DOFade(0, 2).SetSpeedBased(); // 자연스럽게 없어지는 애니메이션 연출
            this.awardTextGO.GetComponent<CanvasGroup>().DOFade(0, 2).SetSpeedBased().OnComplete(() =>
            {
                this.firstUI.SetActive(true);
                this.firstOpenUI.alpha = 1;
                this.caseCap.transform.localPosition = new Vector3(0, 0, 0); // 캡을 원래 위치로 돌려놓음
                this.awardImageGO.GetComponent<Image>().color = new Color(1, 1, 1, 1); // 상품 알파값 초기화
                this.awardImageGO.GetComponent<Image>().sprite = null; // 상품 이미지 초기화
                this.EffectColorChange(eGrade.C); // 이펙트 색상 초기화
                this.awardGetBtn.GetComponent<CanvasGroup>().alpha = 0; // 상품 받기 버튼도 초기화
                this.awardGetBtn.GetComponent<CanvasGroup>().interactable = false; // 버튼 비활성화

                if ((int)this.grade > 3) // V등급 이상이여서 2번째 UI도 활성화 되었을 경우
                {
                    this.secondUI.SetActive(true);
                    this.secondOpenUI.alpha = 1;
                    this.boxCap.transform.localPosition = new Vector3(0, 0, 0); // 박스 캡도 원래 위치로 돌려놓음
                }
            });
        });
    }

    private void AwardAppear()
    {
        this.awardImageGO.GetComponent<Animator>().SetTrigger(this.hashGetAward);
        AwardSetting();

        StartCoroutine(UtilScripts.WaitForSec(() =>
        {
            this.awardTextGO.GetComponent<CanvasGroup>().DOFade(1, 2).SetSpeedBased();
            this.awardGetBtn.GetComponent<CanvasGroup>().DOFade(1, 2).SetSpeedBased().OnComplete(() =>
            {
                this.awardGetBtn.GetComponent<CanvasGroup>().interactable = true; // 버튼 활성화
            }); // 상품 받기 버튼
            
        }, 1f)); // 위의 애니메이션 길이 : 0.667초
                 //애니메이션이 완료 된 후에 살짝 텀을 주고 상품 설명이 서서히 나오도록
    }

    private void AwardSetting()
    {

        //상품 정보 설정 부분
        var award = DataManager.Instance.awardDic[(int)this.grade]; // id값을 등급과 똑같이 해놓았음
        this.awardImageGO.GetComponent<Image>().sprite = Resources.Load<Sprite>(award.ImagePath);
        this.awardGradeTxt.text = $"{this.grade} {award.CompanyName}";
        this.awardNameTxt.text = award.AwardName;
        this.awardPriceTxt.text = $"{string.Format("{0:#,###}", award.Price)}원";
    }

    private void EffectColorChange(eGrade grade)
    {
        switch (grade)
        {
            case eGrade.V:
                {
                    foreach (var eff in this.effects)
                    {
                        var effMain = eff.main;
                        effMain.startColor = new Color(1, 0.5f, 0);
                    }
                    break;
                }
            case eGrade.SV:
                {
                    foreach (var eff in this.effects)
                    {
                        var effMain = eff.main;
                        effMain.startColor = new Color(1, 0, 0);
                    }
                    break;
                }
            case eGrade.SSV:
                {
                    foreach (var eff in this.effects)
                    {
                        var effMain = eff.main;
                        effMain.startColor = new Color(0, 0, 1);
                    }
                    break;
                }
            default:
                {
                    foreach (var eff in this.effects)
                    {
                        var effMain = eff.main;
                        effMain.startColor = new Color(1, 1, 1);
                    }
                    break;
                }
        }
    }
}
