using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    [SerializeField] GameObject startPanel, authorsPanel, settingsPanel;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        if (GetComponent<MatchScript>().IsSound == true) audioSource.volume = 0.6f;
        else audioSource.volume = 0;
    }

    public void Button1Player()
    {
        SceneManager.LoadScene(1);
        GetComponent<MatchScript>().IsAI = true;
        GetComponent<MatchScript>().IsArrows = false;
    }

    public void Button2Players()
    {
        SceneManager.LoadScene(1);
        GetComponent<MatchScript>().IsAI = false;
        GetComponent<MatchScript>().IsArrows = true;
    }

    public void Authors()
    {
        authorsPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void SoundOffOn()
    {
        if (GetComponent<MatchScript>().IsSound == true)
        {
            GetComponent<MatchScript>().IsSound = false;
            audioSource.volume = 0;
        }
        else
        {
            GetComponent<MatchScript>().IsSound = true;
            audioSource.volume = 0.6f;
        }
    }

    public void BackButton()
    {
        authorsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        startPanel.SetActive(true);

    }
}