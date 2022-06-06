using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MiniJSON
{
	// Token: 0x02000173 RID: 371
	internal sealed class JSONSerializer
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x00033908 File Offset: 0x00031D08
		private JSONSerializer()
		{
			this.builder = new StringBuilder();
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0003391B File Offset: 0x00031D1B
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00033922 File Offset: 0x00031D22
		public static bool SerializeEnumAsString { get; set; }

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003392C File Offset: 0x00031D2C
		public static string Serialize(object obj)
		{
			JSONSerializer jsonserializer = new JSONSerializer();
			jsonserializer.SerializeValue(obj);
			JSONSerializer.SerializeEnumAsString = false;
			return jsonserializer.builder.ToString();
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00033958 File Offset: 0x00031D58
		private void SerializeValue(object value)
		{
			if (value != null)
			{
				Type type = value.GetType();
				if (Json.JsonConverters.ContainsKey(type))
				{
					object value2 = Json.JsonConverters[type].Serialize(value);
					this.SerializeValue(value2);
					return;
				}
			}
			string str;
			IList anArray;
			IDictionary obj;
			if (value == null)
			{
				this.builder.Append("null");
			}
			else if ((str = (value as string)) != null)
			{
				this.SerializeString(str);
			}
			else if (value is bool)
			{
				this.builder.Append((!(bool)value) ? "false" : "true");
			}
			else if ((anArray = (value as IList)) != null)
			{
				this.SerializeArray(anArray);
			}
			else if ((obj = (value as IDictionary)) != null)
			{
				this.SerializeDictionary(obj);
			}
			else if (value is char)
			{
				this.SerializeString(new string((char)value, 1));
			}
			else
			{
				this.SerializeOther(value);
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00033A64 File Offset: 0x00031E64
		private void SerializeDictionary(IDictionary obj)
		{
			bool flag = true;
			this.builder.Append('{');
			IEnumerator enumerator = obj.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(obj2);
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.builder.Append('}');
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00033B14 File Offset: 0x00031F14
		private void SerializeArray(IList anArray)
		{
			this.builder.Append('[');
			bool flag = true;
			IEnumerator enumerator = anArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object value = enumerator.Current;
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.builder.Append(']');
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00033BA4 File Offset: 0x00031FA4
		private void SerializeString(string str)
		{
			this.builder.Append('"');
			char[] array = str.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '\b':
					this.builder.Append("\\b");
					break;
				case '\t':
					this.builder.Append("\\t");
					break;
				case '\n':
					this.builder.Append("\\n");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							int num = Convert.ToInt32(c);
							if (num >= 32 && num <= 126)
							{
								this.builder.Append(c);
							}
							else
							{
								this.builder.Append("\\u");
								this.builder.Append(num.ToString("x4"));
							}
						}
						else
						{
							this.builder.Append("\\\\");
						}
					}
					else
					{
						this.builder.Append("\\\"");
					}
					break;
				case '\f':
					this.builder.Append("\\f");
					break;
				case '\r':
					this.builder.Append("\\r");
					break;
				}
			}
			this.builder.Append('"');
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00033D18 File Offset: 0x00032118
		private void SerializeOther(object value)
		{
			if (value is float)
			{
				this.builder.Append(((float)value).ToString("R"));
			}
			else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
			{
				this.builder.Append(value);
			}
			else if (value is double || value is decimal)
			{
				this.builder.Append(Convert.ToDouble(value).ToString("R"));
			}
			else if (value is Enum)
			{
				if (JSONSerializer.SerializeEnumAsString)
				{
					this.SerializeString(value.ToString());
				}
				else
				{
					this.builder.Append((int)value);
				}
			}
			else if (value is Guid)
			{
				this.SerializeString(value.ToString());
			}
			else if (value.GetType().IsClass || value.GetType().IsValueType)
			{
				this.SerializeClass(value);
			}
			else
			{
				this.SerializeString(value.ToString());
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00033E8C File Offset: 0x0003228C
		private void SerializeClass(object value)
		{
			Type type = value.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.GetValue(value) != null)
				{
					dictionary.Add(fieldInfo.Name, fieldInfo.GetValue(value));
				}
			}
			this.SerializeDictionary((IDictionary)dictionary);
		}

		// Token: 0x040008A8 RID: 2216
		private StringBuilder builder;
	}
}
