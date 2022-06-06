#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
public class ClearData : Editor
{
    [MenuItem("TooltipAttribute/ClearData")]
    public static void ClearDataUser()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
#endif
