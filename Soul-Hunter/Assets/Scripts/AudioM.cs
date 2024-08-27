using UnityEngine;

public class AudioM : MonoBehaviour
{
    public static AudioM Instance { get; private set; }

    [SerializeField] private AudioClip buttonSelectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayButtonSelectSound()
    {
        Debug.Log("PlayButtonSelectSound called");  // 追加
        if (audioSource != null && buttonSelectSound != null)
        {
            audioSource.PlayOneShot(buttonSelectSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or buttonSelectSound is null");
        }
    }
}
