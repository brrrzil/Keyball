using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }
}