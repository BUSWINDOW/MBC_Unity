using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MapPos
{
    public Transform fieldPos;
    public Transform turnelPos;
}
public class TriggerTurnel : MonoBehaviour
{
    List<TriggerCube> cubes = new List<TriggerCube>();

    public MapPos[] mapPoses = new MapPos[2];

    public GameObject field;
    public GameObject turnel;

    private void Start()
    {
        this.GetComponentsInChildren<TriggerCube>(cubes);
        StartCoroutine(CheckRoutine());
    }
    IEnumerator CheckRoutine()
    {
        while (true)
        {
            while (!cubes[0].Check && !cubes[1].Check)
            {
                yield return null; // 둘 중 하나가 활성화 될때까지 대기
            }
            var triggerCube = cubes.Find(c => c.Check); // Check가 true인 TriggerCube 찾기
            var targetCube = cubes.Find(c => !c.Check); // Check가 false인 TriggerCube 찾기
            Debug.Log("입장방향 체크 완료");
            int idx = cubes.IndexOf(targetCube);
            Debug.Log(idx);
            while (triggerCube.Check && !targetCube.Check)
            {
                yield return null; // Check가 false가 되거나, targer도 true가 될 동안 대기
            }
            if (targetCube.Check)
            {
                while (targetCube.Check)
                {
                    yield return null; // targetCube가 true인 동안 대기
                }
                if (!triggerCube.Check)
                {
                    Debug.Log("통과했음");
                    this.field.transform.position = mapPoses[idx].fieldPos.transform.position;
                    this.turnel.transform.position = mapPoses[idx].turnelPos.transform.position;
                    this.field.transform.rotation = mapPoses[idx].fieldPos.transform.rotation;
                    this.turnel.transform.rotation = mapPoses[idx].turnelPos.transform.rotation;
                }
                else
                {
                    Debug.Log("도로 나갔음1");
                }
            }
            else
            {
                Debug.Log("도로 나갔음2");
            }
        }
    }
}
