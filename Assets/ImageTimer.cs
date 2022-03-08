using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float MaxTime;
    public bool Tick;
    public float currentTime;

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        Tick = false;
        currentTime += Time.deltaTime;

        if (currentTime >= MaxTime)
        {
            currentTime = 0;
            Tick = true;
        }

        img.fillAmount = currentTime / MaxTime;
    }
}
