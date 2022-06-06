using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	// Token: 0x0200016C RID: 364
	public class JSONAnimationCurveConverter : JSONCustomConverter
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x00032B5D File Offset: 0x00030F5D
		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("AnimationCurve");
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00032B6A File Offset: 0x00030F6A
		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(AnimationCurve));
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00032B7C File Offset: 0x00030F7C
		public override object Serialize(object obj)
		{
			AnimationCurve animationCurve = (AnimationCurve)obj;
			return new Dictionary<string, object>
			{
				{
					"__type",
					"AnimationCurve"
				},
				{
					"keys",
					animationCurve.keys
				}
			};
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00032BB8 File Offset: 0x00030FB8
		public override object Deserialize(object obj)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			object[] array = dictionary["keys"] as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Keyframe key = (Keyframe)Json.JsonConverters[typeof(Keyframe)].Deserialize(array[i]);
				animationCurve.AddKey(key);
			}
			return animationCurve;
		}
	}
}
