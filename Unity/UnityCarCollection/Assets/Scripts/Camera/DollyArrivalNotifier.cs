using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

[RequireComponent(typeof(CinemachineDollyCart))]
public class DollyArrivalNotifier : MonoBehaviour
{
    [Tooltip("0~1 사이 값: 트랙 전체를 1로 봤을 때 도착 임계치")]
    [Range(0f, 1f)] public float normalizedThreshold = 0.99f;

    public UnityEvent OnArrived;

    public CinemachineDollyCart _cart;
    CinemachinePathBase _path;
    CinemachineSmoothPath _smoothPath;
    bool _hasArrived;

    void Awake()
    {
        //_cart = GetComponent<CinemachineDollyCart>();
        _path = _cart.m_Path;  // 반드시 Path 컴포넌트가 할당돼 있어야 합니다.
    }

    void Update()
    {
        if (_hasArrived || _path == null)
            return;

        // 1. 트랙의 최대 포지션 값
        //    PathLength: _path.MaxPos → 총 거리  
        //    PathUnits : _path.MaxPos → 총 세그먼트 수
        float maxPos = _path.MaxPos;

        // 2. 현재 위치 정규화 (0~1)
        float normalizedPos = Mathf.Clamp01(_cart.m_Position / maxPos);

        // 3. 임계치 도달 시 이벤트 발동
        if (normalizedPos >= normalizedThreshold)
        {
            _hasArrived = true;
            //OnArrived?.Invoke();
            Debug.Log("돌리 도착");

        }
    }
}