using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    GameObject[] breakPieces;
    private void Start()
    {
        this.breakPieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            this.breakPieces[i] = transform.GetChild(i).gameObject;
            this.breakPieces[i].SetActive(false);
        }
    }
    public void Break()
    {
        foreach (var piece in this.breakPieces)
        {
            piece.SetActive(true);
            piece.transform.SetParent(null); // 부모를 제거하여 독립적인 오브젝트로 만듭니다.
        }
        this.gameObject.SetActive(false); // 원래 오브젝트를 비활성화합니다.
    }
}
