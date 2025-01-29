using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonScript : MonoBehaviour
{
    [SerializeField] private int sceneNumber;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private AudioSource audioSource;

    private bool isSound = true;

    public void Home(int sceneNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNumber);
    }    

    public void PauseButton()
    {
        if (Time.timeScale == 1)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SoundOffOn()
    {
        if (isSound == true)
        {
            isSound = false;
            audioSource.volume = 0;
        }
        else
        {
            isSound = true;
            audioSource.volume = 0.6f;
        }
    }
}