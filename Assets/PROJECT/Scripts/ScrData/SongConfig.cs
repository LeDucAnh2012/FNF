using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class SongConfig
{
    public enum TypeUnlockSong
    {
        Coin,
        ADS,
        LuckyWheel
    }
    public enum TypePlay
    {
        Eazy,
        Normal,
        Hard,
    }
    public enum TypeSong
    {
        Voice,
        Song
    }
    [HorizontalGroup("Split", Width = 60), HideLabel, PreviewField(100)] public Sprite spriteSong;
    [VerticalGroup("Split/Properties")] public int id;
    [VerticalGroup("Split/Properties")] public int week;
    [VerticalGroup("Split/Properties")] public string nameSong;
    [VerticalGroup("Split/Properties")] public AudioClip song;
    [VerticalGroup("Split/Properties")] public AudioClip voice;
    [VerticalGroup("Split/Properties")] public TextAsset jsonFileEazy;
    [VerticalGroup("Split/Properties")] public TextAsset jsonFileNormal;
    [VerticalGroup("Split/Properties")] public TextAsset jsonFileHard;
    [VerticalGroup("Split/Properties")] public float delay;

    [ShowInInspector]
    [VerticalGroup("Split/Properties")]
    public bool isUnlock
    {
        get => id == 0 ? true : PlayerPrefs.GetInt(nameSong + "_" + VariableSystem.NameMode + "_IsUnLock", 0) == 1 ? true : false;
    }
    [VerticalGroup("Split/Properties")] public TypeUnlockSong typeUnlockSong;
    [VerticalGroup("Split/Properties")] [ShowIf("typeUnlockSong", TypeUnlockSong.Coin)] public int costSong;

    [ShowInInspector]
    [VerticalGroup("Split/Properties")]
    public TypePlay typePlay
    {
        get
        {
            string _str = PlayerPrefs.GetString(nameSong + "_" + VariableSystem.NameMode + "_Difficul", TypePlay.Normal.ToString());
            TypePlay _typePlay = TypePlay.Normal;
            if (Enum.TryParse(_str, out _typePlay))
                return _typePlay;
            return _typePlay;
        }
    }

    [ShowInInspector]
    [VerticalGroup("Split/Properties")]
    public int Score
    {
        get => PlayerPrefs.GetInt(nameSong + "_" + VariableSystem.NameMode + "_Score", 0);
    }
}