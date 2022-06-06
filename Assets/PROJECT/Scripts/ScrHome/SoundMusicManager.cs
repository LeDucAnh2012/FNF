using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using FridayNightFunkin.Data;

public class SoundMusicManager : MonoBehaviour
{
    public static SoundMusicManager instance;

    private const string SOUND = "SETTING_SOUND";
    private const string MUSIC = "SETTING_MUSIC";

    static bool isReadly = false;

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSound;

    [TabGroup("1", "MusicHome")] [SerializeField] AudioClip musicHome;
    [TabGroup("1", "SE_Home")] [SerializeField] AudioClip audClipClickPlayFree;
    [TabGroup("1", "SE_Home")] [SerializeField] AudioClip audClipClickButton;
    [TabGroup("1", "SE_Home")] [SerializeField] AudioClip audClipClickExit;

    [TabGroup("2", "MusicGameplay")] [SerializeField] AudioClip musicGamePlay;
    [TabGroup("2", "SE_Gameplay")] [SerializeField] AudioClip audClipMissNote;
    [TabGroup("2", "SE_Gameplay")] [SerializeField] AudioClip audClipHitNote;

    bool IsGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("GameStartFirstTime"))
        {
            PlayerPrefs.SetInt("GameStartFirstTime", 0);
            return true;
        }

        return false;
    }

    private void Start()
    {

        if (IsGameStartedForTheFirstTime())
        {
            SetMusic(true);
            SetSound(true);
        }
    }

    //========================================================
    public void SetMusic(bool isState)
    {
        PlayerPrefs.SetInt(MUSIC, isState ? 1 : 0);
    }

    public bool GetMusic()
    {
        return PlayerPrefs.GetInt(MUSIC, 0) == 1;
    }

    //---------
    public void SetSound(bool isState)
    {
        PlayerPrefs.SetInt(SOUND, isState ? 1 : 0);
    }

    public bool GetSound()
    {
        return PlayerPrefs.GetInt(SOUND, 0) == 1;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (isReadly)
        {
            Destroy(gameObject);
            return;
        }

        isReadly = true;
        DontDestroyOnLoad(this.gameObject);
    }

    public virtual void PlaySound(AudioClip _clip, float _volume = 1f)
    {
        if (this == null)
            return;
        if (audioSourceSound == null || _clip == null)
            return;
        var _onSound = GetSound();
        if (!_onSound)
            return;
        AudioSource.PlayClipAtPoint(_clip, new Vector3(transform.position.x, transform.position.y, -10), _volume);
    }
    public void MusicGame()
    {
        musicGamePlay = DataSong.GetSongByID(0);
        if (GetMusic())
        {
            audioSourceMusic.volume = 1;
            audioSourceMusic.Stop();
            audioSourceMusic.loop = true;
            audioSourceMusic.clip = musicGamePlay;
            audioSourceMusic.Play();
        }
        else
            audioSourceMusic.Stop();
    }
    #region[Sound Home]
    public void ClickButton()
    {
        PlaySound(audClipClickButton);
    }
    public void ClickButtonPlayFree()
    {
        PlaySound(audClipClickPlayFree);
    }
    public void ClickButtonExit()
    {
        PlaySound(audClipClickExit);
    }
    #endregion
    #region[Sound Gameplay]
    public void MissNote()
    {
        PlaySound(audClipMissNote);
    }
    public void HitNote()
    {
        PlaySound(audClipHitNote);
    }
    #endregion
}
