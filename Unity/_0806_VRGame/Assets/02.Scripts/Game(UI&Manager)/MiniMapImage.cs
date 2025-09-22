using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapImage : MonoBehaviour
{
    private Image image;
    void Start()
    {
        this.image = GetComponent<Image>();
        StartCoroutine(this.OnOffRoutine());
    }
    IEnumerator OnOffRoutine()
    {
        while (true)
        {
            this.image.enabled = !this.image.enabled;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
