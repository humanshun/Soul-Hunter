using UnityEngine;

public class AudioM : MonoBehaviour
{
    // インスタンスを保持するためのプロパティ
    public static AudioM Instance { get; private set; }

    // 各種効果音のオーディオクリップ
    [SerializeField] private AudioClip buttonSelectSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip mantisAttackSound;
    [SerializeField] private AudioClip shootAttackSound;
    [SerializeField] private AudioClip slimeChangeSound;

    // 効果音用のオーディオソース
    private AudioSource audioSource;
    // BGM用のオーディオソース
    private AudioSource bgmSource;

    // デフォルトのBGM音量
    [SerializeField] private float defaultBGMVolume = 0.3f;
    // デフォルトの効果音音量
    [SerializeField] private float defaultEffectsVolume = 0.2f;

    private void Awake()
    {
        // 既にインスタンスが存在する場合、現在のオブジェクトを破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // インスタンスを設定し、シーン間で破棄されないようにする
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 効果音用のオーディオソースを追加
        audioSource = gameObject.AddComponent<AudioSource>();

        // BGM用のオーディオソースを追加
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // BGMをループ再生する設定

        // 音量の初期設定
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume", defaultEffectsVolume);
    }

    private void Start()
    {
        // ゲーム開始時にBGMを再生
        PlayBackgroundMusic();
    }

    // ボタン選択時の効果音を再生
    public void PlayButtonSelectSound()
    {
        if (audioSource != null && buttonSelectSound != null)
        {
            audioSource.PlayOneShot(buttonSelectSound);
        }
    }

    // ボタンクリック時の効果音を再生
    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // ジャンプ時の効果音を再生
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    // 攻撃時の効果音を再生
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    // ダメージ時の効果音を再生
    public void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }

    // カマキリの攻撃音を再生
    public void PlayMantisAttackSound()
    {
        audioSource.PlayOneShot(mantisAttackSound);
    }

    // シュート攻撃音を再生
    public void PlayShootAttackSound()
    {
        audioSource.PlayOneShot(shootAttackSound);
    }

    // スライム変身時の音を再生
    public void PlaySlimeChangeSound()
    {
        audioSource.PlayOneShot(slimeChangeSound);
    }

    // BGMを再生するメソッド
    public void PlayBackgroundMusic()
    {
        if (bgmSource != null && backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    // BGMを停止するメソッド
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
