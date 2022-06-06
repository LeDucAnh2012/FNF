using System;
using UnityEngine;

// Token: 0x02000152 RID: 338
public sealed class CustomPlayerPrefs
{
	public static void SetBool(string key, bool value, bool isSaveImmediately = false)
	{
		int value2 = (!value) ? 0 : 1;
		CustomPlayerPrefs.SetInt(key, value2, isSaveImmediately);
	}

	public static void SetFloat(string key, float value, bool isSaveImmediately = false)
	{
		PlayerPrefs.SetFloat(key, value);
		if (isSaveImmediately)
		{
			CustomPlayerPrefs.Save();
		}
	}

	public static void SetDouble(string key, double value, bool isSaveImmediately = false)
	{
		PlayerPrefs.SetString(key, value.ToString("G17"));
		if (isSaveImmediately)
		{
			CustomPlayerPrefs.Save();
		}
	}

	public static void SetInt(string key, int value, bool isSaveImmediately = false)
	{
		PlayerPrefs.SetInt(key, value);
		if (isSaveImmediately)
		{
			CustomPlayerPrefs.Save();
		}
	}

	public static void SetDateTime(string key, DateTime value, bool isSaveImmediately = false)
	{
		PlayerPrefs.SetString(key, value.ToBinary().ToString());
		if (isSaveImmediately)
		{
			CustomPlayerPrefs.Save();
		}
	}

	public static void SetString(string key, string value, bool isSaveImmediately = false)
	{
		PlayerPrefs.SetString(key, value);
		if (isSaveImmediately)
		{
			CustomPlayerPrefs.Save();
		}
	}

	public static void SetEnumValue<T>(string key, T value, bool isSaveImmediately = false) where T : struct, IConvertible
	{
		CustomPlayerPrefs.SetString(key, value.ToString(), isSaveImmediately);
	}

	public static void SetObjectValue<T>(string key, T value, bool saveImmediately = false) where T : class
	{
		string value2 = (value != null) ? JsonConvert.SerializeObject(value) : string.Empty;
		CustomPlayerPrefs.SetString(key, value2, saveImmediately);
	}

	public static bool GetBool(string key)
	{
		return CustomPlayerPrefs.GetInt(key) == 1;
	}

	public static bool GetBool(string key, bool defaultValue)
	{
		int defaultValue2 = (!defaultValue) ? 0 : 1;
		return CustomPlayerPrefs.GetInt(key, defaultValue2) == 1;
	}

	public static float GetFloat(string key)
	{
		return CustomPlayerPrefs.GetFloat(key, 0f);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	public static double GetDouble(string key)
	{
		return CustomPlayerPrefs.GetDouble(key, 0.0);
	}

	public static double GetDouble(string key, double defaultValue)
	{
		string @string = PlayerPrefs.GetString(key);
		double result = defaultValue;
		if (!string.IsNullOrEmpty(@string))
		{
			double num = 0.0;
			if (double.TryParse(@string, out num))
			{
				result = num;
			}
		}
		return result;
	}

	public static int GetInt(string key)
	{
		return CustomPlayerPrefs.GetInt(key, 0);
	}

	public static int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public static DateTime GetDateTime(string key)
	{
		return CustomPlayerPrefs.GetDateTime(key, CustomPlayerPrefs.DEFAULT_DATE_TIME);
	}

	public static DateTime GetDateTime(string key, DateTime defaultValue)
	{
		string @string = PlayerPrefs.GetString(key);
		DateTime result = defaultValue;
		if (!string.IsNullOrEmpty(@string))
		{
			long dateData = Convert.ToInt64(@string);
			result = DateTime.FromBinary(dateData);
		}
		return result;
	}

	public static string GetString(string key)
	{
		return CustomPlayerPrefs.GetString(key, CustomPlayerPrefs.DEFAULT_STRING);
	}

	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static T GetEnumValue<T>(string key, T defaultValue = default(T)) where T : struct, IConvertible
	{
		string @string = CustomPlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return (T)((object)Enum.Parse(typeof(T), @string));
		}
		return defaultValue;
	}

	public static T GetObjectValue<T>(string key) where T : class
	{
		string @string = CustomPlayerPrefs.GetString(key);
		return (!string.IsNullOrEmpty(@string)) ? JsonConvert.DeserializeObject<T>(@string) : ((T)((object)null));
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
		CustomPlayerPrefs.Save();
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
		CustomPlayerPrefs.Save();
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	private const string DOUBLE_FORMAT_SPECIFIER = "G17";

	public static readonly string DEFAULT_STRING = string.Empty;

	public static readonly DateTime DEFAULT_DATE_TIME = DateTime.MinValue;

	private const int DEFAULT_INT_VALUE = 0;

	private const int BOOL_TRUE_INT_VALUE = 1;

	private const int BOOL_FALSE_INT_VALUE = 0;

	private const float DEFAULT_FLOAT_VALUE = 0f;

	private const double DEFAULT_DOUBLE_VALUE = 0.0;
}
