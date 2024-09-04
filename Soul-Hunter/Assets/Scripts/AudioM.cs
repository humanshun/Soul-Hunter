using UnityEngine;

public class AudioM : MonoBehaviour
{
    public static AudioM Instance { get; private set; }

    [SerializeField] private AudioClip buttonSelectSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip mantisAttackSound;
    [SerializeField] private AudioClip shootAttackSound;

    private AudioSource audioSource;
    private AudioSource bgmSource; // BGM用のAudioSource

    [SerializeField] private float defaultBGMVolume = 0.3f; // デフォルトのBGM音量
    [SerializeField] private float defaultEffectsVolume = 0.2f; // デフォルトの効果音音量

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

        // BGM用のAudioSourceを追加
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // BGMをループ再生する設定

        // 音量の初期設定
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume", defaultEffectsVolume);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayButtonSelectSound()
    {
        if (audioSource != null && buttonSelectSound != null)
        {
            audioSource.PlayOneShot(buttonSelectSound);
        }
    }

    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }
    public void PlayMantisAttackSound()
    {
        audioSource.PlayOneShot(mantisAttackSound);
    }
    public void PlayShootAttackSound()
    {
        audioSource.PlayOneShot(shootAttackSound);
    }

    public void PlayBackgroundMusic()
    {
        if (bgmSource != null && backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // BGM音量を設定するメソッド
    public void SetBackgroundMusicVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // 効果音音量を設定するメソッド
    public void SetEffectsVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    // BGMの音量プロパティ
    public float BGMVolume
    {
        get => bgmSource.volume;
        set
        {
            bgmSource.volume = value;
            PlayerPrefs.SetFloat("BGMVolume", value);
            PlayerPrefs.Save();
        }
    }

    // 効果音の音量プロパティ
    public float EffectsVolume
    {
        get => audioSource.volume;
        set
        {
            audioSource.volume = value;
            PlayerPrefs.SetFloat("EffectsVolume", value);
            PlayerPrefs.Save();
        }
    }
}

