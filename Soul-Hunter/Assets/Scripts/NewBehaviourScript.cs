using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundClip; // InspectorからAudioClipを設定
    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        // AudioSourceにAudioClipを設定
        audioSource.clip = soundClip;
    }

    void Update()
    {
        // Wキーが押されたときに音を再生
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        // 音を再生
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip); // 音を再生
        }
    }
}
