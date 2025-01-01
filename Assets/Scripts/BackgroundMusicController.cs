using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); // Phát nhạc khi Scene được chạy
    }
}
