using System;
using System.Collections.Generic;

namespace MiniJSON
{
	// Token: 0x0200016F RID: 367
	public class JSONDateTimeConverter : JSONCustomConverter
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x00032C5A File Offset: 0x0003105A
		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("DateTime");
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00032C67 File Offset: 0x00031067
		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(DateTime));
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00032C7C File Offset: 0x0003107C
		public override object Serialize(object obj)
		{
			string value = ((DateTime)obj).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
			return new Dictionary<string, string>
			{
				{
					"__type",
					"DateTime"
				},
				{
					"iso",
					value
				}
			};
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00032CC8 File Offset: 0x000310C8
		public override object Deserialize(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			string s = dictionary["iso"] as string;
			return DateTime.Parse(s);
		}

		// Token: 0x04000894 RID: 2196
		internal static readonly long EPOX_TICKS = 621355968000000000L;
	}
}
