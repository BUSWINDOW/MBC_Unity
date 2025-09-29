using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public int Index
    {
        get; private set;
    }

    [SerializeField]
    private int anomalyPercent = 70; // 이상현상이 나올 확률
    [SerializeField]
    private int notSeenPercent = 70; // 이상현상이 나왔을 때 안봤던 이상현상이 나올 확률
    private IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }
        if (!DataManager.Instance.IsDataExist) // 데이터가 없을 경우
        {
            for(int i = 0; i< this.transform.childCount; i++)
            {
                DataManager.Instance.dicAnomalySeen.Add(i,false);
            }
        }
        else//데이터가 이미 존재할 경우
        {

        }
    }

    public void SetAnomaly()
    {
        //어떤게 출력될지 미리 정함
        if(Random.Range(0,100) <= this.anomalyPercent) // 이상현상이 등장할 확률
        {
            //이상현상 등장
            if (!DataManager.Instance.dicAnomalySeen.ContainsValue(false)) //false인게 없음 -> 안본게 없음
            {
                this.Index = Random.Range(0, DataManager.Instance.dicAnomalySeen.Count);
            }
            else
            {
                if (Random.Range(0, 100) <= this.notSeenPercent) // 안봤던 이상현상이 등장할 확률
                {
                    var notSeenList = DataManager.Instance.dicAnomalySeen.Where(x => !x.Value).Select(x=>x.Key).ToList(); // false인것들 -> 안본것들
                    var idx = Random.Range(0, notSeenList.Count);
                    this.Index = notSeenList[idx];
                }
                else
                {
                    var seenList = DataManager.Instance.dicAnomalySeen.Where(x => x.Value).Select(x => x.Key).ToList(); // true인것들 -> 안본것들
                    var idx = Random.Range(0, seenList.Count);
                    this.Index = seenList[idx];
                }
            }
            this.transform.GetChild(this.Index).gameObject.SetActive(true);
        }
        else
        {
            //이상현상 미등장
        }
    }
}
