using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCellMove : MonoBehaviour
{
    Vector2Int _currentCell;
    public Func<Vector3,Vector2Int> GetCellFunc;
    public Action<Vector2Int> CellMoveAction; // Cell이 바뀔 때 호출할 액션
    Vector2Int CurrentCell
    {
        get
        {
            return _currentCell;
        }
        set
        {
            _currentCell = value;
            //Cell이 바뀔 때 쓰래기 로딩하는 내용
            CellMoveAction(value); // Cell이 바뀌면 해당 Cell에 있는 쓰레기를 로드하는 함수 호출
        }
    }
    private IEnumerator Start()
    {
        while(this.GetCellFunc == null)
        {
            yield return null; // GetCellFunc이 할당될 때까지 대기
        }
        StartCoroutine(CellCheckRoutine());
    }
    IEnumerator CellCheckRoutine()
    {
        while (true)
        {
            Vector2Int newCell = GetCellFunc(this.transform.position);
            if (newCell != CurrentCell)
            {
                CurrentCell = newCell;
            }
            yield return null;
        }
    }
}
