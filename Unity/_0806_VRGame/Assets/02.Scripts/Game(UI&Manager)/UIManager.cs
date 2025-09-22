using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public Transform head;
    public float distance = 2f;
    public GameObject menu;
    public InputActionProperty menuBtn;
    // Start is called before the first frame update
    private void Update()
    {
        if (this.menuBtn.action.triggered)
        {
            this.menu.SetActive(!this.menu.activeSelf);
            this.menu.transform.position = this.head.position + this.head.forward * distance;
            this.menu.transform.LookAt(this.head.transform);
            this.menu.transform.forward *= -1;
        }
    }

}
