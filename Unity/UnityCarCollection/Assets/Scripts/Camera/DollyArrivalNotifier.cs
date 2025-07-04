using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

[RequireComponent(typeof(CinemachineDollyCart))]
public class DollyArrivalNotifier : MonoBehaviour
{
    [Tooltip("0~1 ���� ��: Ʈ�� ��ü�� 1�� ���� �� ���� �Ӱ�ġ")]
    [Range(0f, 1f)] public float normalizedThreshold = 0.99f;

    public UnityEvent OnArrived;

    public CinemachineDollyCart _cart;
    CinemachinePathBase _path;
    CinemachineSmoothPath _smoothPath;
    bool _hasArrived;

    void Awake()
    {
        //_cart = GetComponent<CinemachineDollyCart>();
        _path = _cart.m_Path;  // �ݵ�� Path ������Ʈ�� �Ҵ�� �־�� �մϴ�.
    }

    void Update()
    {
        if (_hasArrived || _path == null)
            return;

        // 1. Ʈ���� �ִ� ������ ��
        //    PathLength: _path.MaxPos �� �� �Ÿ�  
        //    PathUnits : _path.MaxPos �� �� ���׸�Ʈ ��
        float maxPos = _path.MaxPos;

        // 2. ���� ��ġ ����ȭ (0~1)
        float normalizedPos = Mathf.Clamp01(_cart.m_Position / maxPos);

        // 3. �Ӱ�ġ ���� �� �̺�Ʈ �ߵ�
        if (normalizedPos >= normalizedThreshold)
        {
            _hasArrived = true;
            //OnArrived?.Invoke();
            Debug.Log("���� ����");

        }
    }
}