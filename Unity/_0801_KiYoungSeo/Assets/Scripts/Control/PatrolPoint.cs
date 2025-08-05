using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPoint : MonoBehaviour
    {
        List<Transform> points = new List<Transform>();
        void Awake()
        {
            GetComponentsInChildren<Transform>(points);
            this.points.RemoveAt(0);
        }
        public Vector3 GetPoint(ref int curIdx)
        {
            if (++curIdx == this.points.Count)
            {
                curIdx = 0;
            }
            return this.points[curIdx].position;
        }
    }

}

