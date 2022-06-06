using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SongConfig;

namespace FridayNightFunkin.Data
{

    [CreateAssetMenu]
    public class DataSong : ScriptableObject
    {
        private static ResourceAsset<DataSong> asset = new ResourceAsset<DataSong>("SourceData/Song/DataSongs");
        public List<SongConfig> listSongConfig;

        public static int Count
        {
            get => asset.Value.listSongConfig.Count;
        }
        public static List<SongConfig> GetListSongConfig()
        {
            return asset.Value.listSongConfig;
        }
        public static List<SongConfig> GetListSongConfig(TypeUnlockSong _typeUnlockSong)
        {
            List<SongConfig> _l = new List<SongConfig>();
            List<SongConfig> _listSongConfig = asset.Value.listSongConfig;

            for (int i = 0; i < Count; i++)
            {
                if (_listSongConfig[i].typeUnlockSong == _typeUnlockSong)
                    _l.Add(_listSongConfig[i]);
            }
            return _l;
        }
        public static List<SongConfig> GetListSongConfig(bool _isUnLock)
        {
            List<SongConfig> _l = new List<SongConfig>();
            List<SongConfig> _listSongConfig = asset.Value.listSongConfig;

            for (int i = 0; i < Count; i++)
            {
                if (_listSongConfig[i].isUnlock == _isUnLock)
                    _l.Add(_listSongConfig[i]);
            }
            return _l;
        }
        public static List<SongConfig> GetListSongConfig(TypeUnlockSong _typeUnlockSong, bool _isUnLock)
        {
            List<SongConfig> _l = new List<SongConfig>();

            List<SongConfig> _listSongConfig = asset.Value.listSongConfig;

            for (int i = 0; i < Count; i++)
            {
                if (_listSongConfig[i].typeUnlockSong == _typeUnlockSong && _listSongConfig[i].isUnlock == _isUnLock)
                    _l.Add(_listSongConfig[i]);
            }
            return _l;
        }

        public static string GetNameSongByID(int id)
        {
            //List<SongConfig> _listSongConfig = asset.Value.listSongConfig;
            //for (int i = 0; i < Count; i++)
            //{
            //    if (_listSongConfig[i].id == id)
            //        return _listSongConfig[i].nameSong;
            //}
            //return string.Empty;
            return asset.Value.listSongConfig[id].nameSong;
        }
        public static AudioClip GetSongByID(int id)
        {
            //List<SongConfig> _listSongConfig = asset.Value.listSongConfig;
            //for (int i = 0; i < Count; i++)
            //{
            //    if (_listSongConfig[i].id == id)
            //        return _listSongConfig[i].song;
            //}
            //return null;
            return asset.Value.listSongConfig[id].song;
        }
        public static AudioClip GetVoiceByID(int id)
        {
            //List<SongConfig> _listSongConfig = asset.Value.listSongConfig;
            //for (int i = 0; i < Count; i++)
            //{
            //    if (_listSongConfig[i].id == id)
            //        return _listSongConfig[i].voice;
            //}
            //return null;
            return asset.Value.listSongConfig[id].voice;
        }
        public static float GetDelay(int id)
        {
            return asset.Value.listSongConfig[id].delay;
        }
        public static TextAsset GetJsonFile(int id, TypePlay typePlay)
        {
            List<SongConfig> _listSongConfig = asset.Value.listSongConfig;
            for (int i = 0; i < Count; i++)
            {
                if (_listSongConfig[i].id == id)
                {
                    switch (typePlay)
                    {
                        case TypePlay.Eazy:
                            return _listSongConfig[i].jsonFileEazy;
                        case TypePlay.Normal:
                            return _listSongConfig[i].jsonFileNormal;
                        case TypePlay.Hard:
                            return _listSongConfig[i].jsonFileHard;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Check Song Unlock
        /// </summary>
        /// <param name="id">id Song</param>
        /// <returns>True if Song Unlocked</returns>
        public static bool CheckSongUnlock(int id)
        {
            //List<SongConfig> _l = GetListSongConfig(isState);
            //for (int i = 0; i < _l.Count; i++)
            //{
            //    if (_l[i].id == id)
            //        return true;
            //}
            //return false;
            return asset.Value.listSongConfig[id].isUnlock;
        }
        /// <summary>
        /// Get Cost Song
        /// </summary>
        /// <param name="id">id Song</param>
        /// <returns>cost of Song</returns>
        public static int GetCostSong(int id)
        {
            //List<SongConfig> _l = GetListSongConfig(TypeUnlockSong.Coin, false);
            //for (int i = 0; i < _l.Count; i++)
            //{
            //    if (_l[i].id == id)
            //        return _l[i].costSong;
            //}
            //return 0;
            return asset.Value.listSongConfig[id].costSong;
        }
        public static TypePlay GetTypePlay(int id)
        {
            //List<SongConfig> _l = GetListSongConfig();
            //for (int i = 0; i < _l.Count; i++)
            //{
            //    if (_l[i].id == id)
            //        return _l[i].typePlay;
            //}
            //return TypePlay.Normal;
            return asset.Value.listSongConfig[id].typePlay;
        }
        public static TypeUnlockSong GetTypeUnLock(int id)
        {
            List<SongConfig> _l = GetListSongConfig(TypeUnlockSong.Coin, false);
            for (int i = 0; i < _l.Count; i++)
            {
                if (_l[i].id == id)
                    return _l[i].typeUnlockSong;
            }
            return TypeUnlockSong.Coin;
        }
        public static int GetScoreSong(int id)
        {
            //List<SongConfig> _l = GetListSongConfig();
            //for (int i = 0; i < _l.Count; i++)
            //{
            //    if (_l[i].id == id)
            //        return _l[i].Score;
            //}
            //return 0;
            return asset.Value.listSongConfig[id].Score;
        }
        public static float GetTimeDelayVoice(int id)
        {
            //List<SongConfig> _l = GetListSongConfig();
            //for (int i = 0; i < _l.Count; i++)
            //{
            //    if (_l[i].id == id)
            //        return _l[i].delay;
            //}
            //return 0;
            return asset.Value.listSongConfig[id].delay;
        }
        #region[Set DATA]
        public static void UnlockSong(int id)
        {
            PlayerPrefs.SetInt(asset.Value.listSongConfig[id].nameSong + "_" + VariableSystem.NameMode + "_IsUnLock", 1);
        }
        public static void SetTypePlay(int id = -1, TypePlay _type = TypePlay.Normal)
        {
            List<SongConfig> _l = GetListSongConfig();
            for (int i = 0; i < _l.Count; i++)
            {
                if (id == -1)
                {
                    PlayerPrefs.SetString(_l[i].nameSong + "_" + VariableSystem.NameMode + "_Difficul", TypePlay.Normal.ToString());
                }
                else
                if (_l[i].id == id)
                {
                    PlayerPrefs.SetString(_l[i].nameSong + "_" + VariableSystem.NameMode + "_Difficul", _type.ToString());
                    return;
                }
            }
        }
        public static void SetScoreSong(int id, int Score)
        {
            PlayerPrefs.SetInt(asset.Value.listSongConfig[id].nameSong + "_" + VariableSystem.NameMode + "_Score", Score);
        }
        #endregion
    }

}