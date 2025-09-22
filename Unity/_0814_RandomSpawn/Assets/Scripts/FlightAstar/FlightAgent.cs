using System.Collections.Generic;
using UnityEngine;

public class FlightAgent : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float turnSpeed = 5f;
    GridManager gridManager;
    [SerializeField]
    List<GridManager.Node> path;
    int pathIndex;


    public bool test;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        UpdatePath();
    }

    void UpdatePath()
    {
        path = gridManager.FindPath(transform.position, target.position);
        pathIndex = 0;
    }

    void Update()
    {
        if (test)
        {
            this.test = false;
            UpdatePath();
        }
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetPoint = path[pathIndex].worldPosition;
        this.transform.LookAt(targetPoint);
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, targetPoint) < 0.5f)
            pathIndex++;
    }
}