using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAndScale : MonoBehaviour
{
    public GameObject PlayImage;
    public GameObject PauseImage;
    public Button SpeedUpButton;
    public Button SpeedDownButton;

    public AudioSource Audio;
    public AudioSource BackgroundAudio;

    public bool paused;

    public void PlayOnClick()
    {
        Audio.Play();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
    }

    public void PauseAudio()
    {
        if (paused)
        {
            BackgroundAudio.Play();
            PlayImage.SetActive(true);
            PauseImage.SetActive(false);
        }
        else
        {
            BackgroundAudio.Pause();
            PlayImage.SetActive(false);
            PauseImage.SetActive(true);
        }
        paused = !paused;
    }

    public void SpeedUp()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            SpeedUpButton.interactable = false;
        }
        if (Time.timeScale == 0.5f)
        {
            Time.timeScale = 1;
            SpeedDownButton.interactable = true;
        }
    }

    public void SpeedDown()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.5f;
            SpeedDownButton.interactable = false;
        }
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
            SpeedUpButton.interactable = true;
        }
    }

    public void RestartAudio()
    {
        BackgroundAudio.Stop();
        BackgroundAudio.Play();
        SpeedUpButton.interactable = true;
        SpeedDownButton.interactable = true;
    }
}
