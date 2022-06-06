using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	// Token: 0x02000174 RID: 372
	public static class Json
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x00033EFC File Offset: 0x000322FC
		public static T Deserialize<T>(string json)
		{
			return (T)((object)Json.Deserialize(json, typeof(T)));
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00033F13 File Offset: 0x00032313
		public static object Deserialize(string json, Type type)
		{
			if (json == null)
			{
				return null;
			}
			return JSONParser.Parse(json, type);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00033F24 File Offset: 0x00032324
		public static string Serialize(object obj)
		{
			return JSONSerializer.Serialize(obj);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00033F2C File Offset: 0x0003232C
		public static void SerializeTest<T>(T obj)
		{
			string text = Json.Serialize(obj);
			//CustomDebug.Log(text);
			//T t = Json.Deserialize<T>(text);
			//CustomDebug.Log(t);
			//CustomDebug.Log(t.GetType());
			//string message = Json.Serialize(t);
			//CustomDebug.Log(message);
		}

		// Token: 0x040008A9 RID: 2217
		public static Dictionary<Type, JSONCustomConverter> JsonConverters = new Dictionary<Type, JSONCustomConverter>
		{
			{
				typeof(DateTime),
				new JSONDateTimeConverter()
			},
			{
				typeof(AnimationCurve),
				new JSONAnimationCurveConverter()
			},
			{
				typeof(Keyframe),
				new JSONKeyFrameConverter()
			}
		};

		// Token: 0x040008AA RID: 2218
		public static Dictionary<string, JSONCustomConverter> JsonConvertersString = new Dictionary<string, JSONCustomConverter>
		{
			{
				"DateTime",
				new JSONDateTimeConverter()
			},
			{
				"AnimationCurve",
				new JSONAnimationCurveConverter()
			},
			{
				"Keyframe",
				new JSONKeyFrameConverter()
			}
		};
	}
}
