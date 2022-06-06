using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	// Token: 0x02000170 RID: 368
	public class JSONKeyFrameConverter : JSONCustomConverter
	{
		// Token: 0x06000AB9 RID: 2745 RVA: 0x00032D10 File Offset: 0x00031110
		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("Keyframe");
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00032D1D File Offset: 0x0003111D
		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(Keyframe));
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00032D30 File Offset: 0x00031130
		public override object Serialize(object obj)
		{
			Keyframe keyframe = (Keyframe)obj;
			return new Dictionary<string, object>
			{
				{
					"inTangent",
					keyframe.inTangent
				},
				{
					"outTangent",
					keyframe.outTangent
				},
				{
					"inWeight",
					keyframe.inWeight
				},
				{
					"outWeight",
					keyframe.outWeight
				},
				{
					"weightedMode",
					(short)keyframe.weightedMode
				},
				{
					"time",
					keyframe.time
				},
				{
					"value",
					keyframe.value
				}
			};
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00032DF0 File Offset: 0x000311F0
		public override object Deserialize(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			return new Keyframe
			{
				inTangent = Convert.ToSingle(dictionary["inTangent"]),
				outTangent = Convert.ToSingle(dictionary["outTangent"]),
				inWeight = Convert.ToSingle(dictionary["inWeight"]),
				outWeight = Convert.ToSingle(dictionary["outWeight"]),
				weightedMode = (WeightedMode)Convert.ToInt16(dictionary["weightedMode"]),
				time = Convert.ToSingle(dictionary["time"]),
				value = Convert.ToSingle(dictionary["value"])
			};
		}
	}
}
