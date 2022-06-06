using UnityEngine;

public static class VariableSystem
{
    #region[Data Game]
    public static int StoreCoin
    {
        set => PlayerPrefs.SetInt("StoreCoin", value);
        get => PlayerPrefs.GetInt("StoreCoin", 0);
    }
    public static int StoreHeart
    {
        set => PlayerPrefs.SetInt("StoreHeart", value);
        get => PlayerPrefs.GetInt("StoreHeart", 0);
    }
    public static int IndexSongPlaying
    {
        set => PlayerPrefs.SetInt("IndexSong", value);
        get => PlayerPrefs.GetInt("IndexSong", 0);
    }
    public static int CurrentSong
    {
        set => PlayerPrefs.SetInt("CurrentSong", value);
        get => PlayerPrefs.GetInt("CurrentSong", 0);
    }
    public static int CurrentWeek
    {
        set => PlayerPrefs.SetInt("CurrentWeek", value);
        get => PlayerPrefs.GetInt("CurrentWeek", 0);
    }
    public static bool Vibration
    {
        set => PlayerPrefs.SetInt("CurrentWeek", value ? 1 : 0);
        get => PlayerPrefs.GetInt("CurrentWeek", 0) == 1 ? true : false;
    }
    public static int TotalScore
    {
        set => PlayerPrefs.SetInt("TotalScore", value);
        get => PlayerPrefs.GetInt("TotalScore", 0);
    }
    public static int CountTimeGetHeart
    {
        set => PlayerPrefs.SetInt("CountTimeGetHeart", value);
        get => PlayerPrefs.GetInt("CountTimeGetHeart", 0);
    }
    public static int MaxHeart
    {
        get => PlayerPrefs.GetInt("MaxHeart", 5);
    }
    public static string TimeEndGame
    {
        set => PlayerPrefs.SetString("TimeEndGame", value);
        get => PlayerPrefs.GetString("TimeEndGame", "");
    }
    /// <summary>
    /// return total time get heart (s)
    /// </summary>
    public static float TimeGetHeart
    {
        get => PlayerPrefs.GetFloat("TimeGetHeart", 1) * 60;
    }
    #endregion
    #region[Name Mode]
    public static string NameMode
    {
        set => PlayerPrefs.SetString("NameMode", value);
        get => PlayerPrefs.GetString("NameMode", "NameMode");
    }
    #endregion
    #region[Lose]
    public static bool ShowListSong
    {
        set => PlayerPrefs.SetInt("ShowListSong", value ? 1 : 0);
        get => PlayerPrefs.GetInt("ShowListSong", 0) == 1 ? true : false;
    }
    #endregion
    #region[SAVE BASE]
    //public enum KeyData
    //{
    //    LevelCurrent,
    //};
    //======================================================================
    //public static int LevelCurrent
    //{
    //    set => PlayerPrefs.SetInt("LevelCurrent", value);
    //    get => PlayerPrefs.GetInt("LevelCurrent", 0);
    //}
    //#region[GET SET DATA]
    //public static void SetData(KeyData _strKey, int value)
    //{
    //    PlayerPrefs.SetInt(_strKey.ToString(), value);
    //}
    //public static void SetData(KeyData _strKey, float value)
    //{
    //    PlayerPrefs.SetFloat(_strKey.ToString(), value);
    //}
    //public static void SetData(KeyData _strKey, bool value)
    //{
    //    PlayerPrefs.SetInt(_strKey.ToString(), value ? 1 : 0);
    //}
    //=======================================================================
    /// <summary>
    /// Get data Type Int
    /// </summary>
    /// <param name = "_strKey" > Key Data</param>
    /// <param name = "_i" > Type </ param >
    /// < returns > -99 If Haskey = false </ returns >
    //public static int GetData(KeyData _strKey, int _i = 0)
    //{
    //    if (PlayerPrefs.HasKey(_strKey.ToString()))
    //        return PlayerPrefs.GetInt(_strKey.ToString(), 0);
    //    return -99;
    //}
    /// <summary>
    /// Get data Type Float
    /// </summary>
    /// <param name = "_strKey" > Key Data</param>
    /// <param name = "_f" > Type </ param >
    /// < returns > -99 If Haskey = false </ returns >
    //public static float GetData(KeyData _strKey, float _f = 0f)
    //{
    //    if (PlayerPrefs.HasKey(_strKey.ToString()))
    //        return PlayerPrefs.GetFloat(_strKey.ToString(), 0);
    //    return -99;
    //}
    /// <summary>
    /// Get data Type Bool
    /// </summary>
    /// <param name = "_strKey" > Key Data</param>
    /// <param name = "_b" > Type </ param >
    /// < returns > True False</returns>
    //public static bool GetData(KeyData _strKey, bool _b = false)
    //{
    //    return PlayerPrefs.GetInt(_strKey.ToString(), 0) == 1 ? true : false;
    //}
    //#endregion
    #endregion
    public enum KeyData
    {
        PlayerData,
        GameData
    }
    public class GameData
    {
        public int TypeMode;
        public int Level;
        public int Coin;
        public float TimeOnScene;
    }
    public static GameData gameData = null;
    static VariableSystem() { }
    public static void InitData()
    {
        if (!CustomPlayerPrefs.HasKey("GameData"))
            SaveData();
        else
            GetData();
        if (gameData == null)
            gameData = new GameData();
        gameData.TimeOnScene = Time.timeSinceLevelLoad;
    }
    public static void SaveData()
    {
        CustomPlayerPrefs.SetObjectValue<GameData>("GameData", gameData, false);
        Save();
    }
    public static void GetData()
    {
        gameData = CustomPlayerPrefs.GetObjectValue<GameData>("GameData");
    }
    public static void Save()
    {
        CustomPlayerPrefs.Save();
    }
    public static void SetLevel(int _level)
    {
        gameData.Level = _level;
        SaveData();
    }
}