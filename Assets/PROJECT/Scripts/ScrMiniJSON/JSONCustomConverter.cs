using System;

namespace MiniJSON
{
	// Token: 0x0200016E RID: 366
	public abstract class JSONCustomConverter
	{
		// Token: 0x06000AAE RID: 2734
		public abstract bool IsCanBeDeserialized(string type);

		// Token: 0x06000AAF RID: 2735
		public abstract bool IsCanBeDeserialized(Type type);

		// Token: 0x06000AB0 RID: 2736
		public abstract object Serialize(object obj);

		// Token: 0x06000AB1 RID: 2737
		public abstract object Deserialize(object obj);
	}
}
