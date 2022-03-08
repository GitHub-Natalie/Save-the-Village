using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenClose : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Sprite UpMark;
    [SerializeField] private Sprite DownMark;

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

public void OpenOrClose(GameObject pannel)
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
            img.sprite = DownMark;
        }
        else
        {
            Panel.SetActive(true);
            img.sprite = UpMark;
        }
    }
}