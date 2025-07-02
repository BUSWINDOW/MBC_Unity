using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SwatFov))] // EnemyFov 스크립트에 대한 커스텀 에디터
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        SwatFov fov = (SwatFov)target; //FoV 스크립트의 인스턴스를 가져온다

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f); //원주 위의 시작점의 좌표를 계산 (주어진 각도의 절반만큼씩 좌우로
                                                                       //시작점과 끝점을 맞춤) 시작점만 맞추면 각도를 전달하기 때문에
                                                                       //끝점은 안만들어도 됨 



        Handles.color = Color.white; //시야각을 표현하기 위한 핸들의 색깔

        Handles.DrawSolidArc( //반지름 그리는 함수
            fov.transform.position, //원의 중심 좌표
            Vector3.up, //회전 기준 축(y축)
            fromAnglePos, // 부채꼴의 시작점 좌표
            fov.viewAngle, //시야각 부채꼴의 각도
            fov.viewRange //부채꼴의 반지름
        );

        Handles.Label( // 씬 화면에 라벨을 추가해서 텍스트로 표시하게 해주는 함수
            fov.transform.position + Vector3.up * 2f, // 라벨 위치
            "Fov Range : " + fov.viewRange + "\n" + "Fov Angle : " + fov.viewAngle //적을 글
            );


    }
}
