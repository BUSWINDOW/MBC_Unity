using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TriggerZone>().onEnterAction += (obj) =>
        {
            obj.SetActive(false);
        };
    }
}
