using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MiniJSON
{
	// Token: 0x02000171 RID: 369
	public class JSONParser : IDisposable
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x00032EB3 File Offset: 0x000312B3
		private JSONParser(string jsonString)
		{
			this.json = new StringReader(jsonString);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00032EC7 File Offset: 0x000312C7
		public static bool IsWordBreak(char c)
		{
			return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00032EE8 File Offset: 0x000312E8
		public static object Parse(string jsonString, Type type)
		{
			object result;
			using (JSONParser jsonparser = new JSONParser(jsonString))
			{
				result = jsonparser.ParseValue(type);
			}
			return result;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00032F28 File Offset: 0x00031328
		public void Dispose()
		{
			this.json.Dispose();
			this.json = null;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00032F3C File Offset: 0x0003133C
		public object ParseObject(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == JSONParser.dictionaryType)
			{
				return this.ParseDictionary(type);
			}
			if (type.Equals(JSONParser.objectType))
			{
				return this.ParseDictionary(JSONParser.defaultDictionaryType);
			}
			return this.ParseClass(type);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00032F94 File Offset: 0x00031394
		public object ParseDictionary(Type type)
		{
			IDictionary dictionary = Activator.CreateInstance(type) as IDictionary;
			Type type2 = type.GetGenericArguments()[0];
			Type type3 = type.GetGenericArguments()[1];
			this.json.Read();
			for (;;)
			{
				JSONParser.TOKEN nextToken = this.NextToken;
				switch (nextToken)
				{
				case JSONParser.TOKEN.NONE:
					goto IL_4F;
				default:
					if (nextToken != JSONParser.TOKEN.COMMA)
					{
						object obj = this.ParseValue(type2);
						if (obj == null)
						{
							goto Block_5;
						}
						if (this.NextToken != JSONParser.TOKEN.COLON)
						{
							goto Block_6;
						}
						this.json.Read();
						object value = this.ParseValue(type3);
						if (obj.GetType() != type2)
						{
							obj = Convert.ChangeType(obj, type2);
						}
						dictionary[obj] = value;
					}
					break;
				case JSONParser.TOKEN.CURLY_CLOSE:
					goto IL_56;
				}
			}
			IL_4F:
			return null;
			IL_56:
			if (dictionary.Contains("__type"))
			{
				string text = dictionary["__type"] as string;
				if (text != null && Json.JsonConvertersString.ContainsKey(text))
				{
					return Json.JsonConvertersString[text].Deserialize(dictionary);
				}
			}
			return dictionary;
			Block_5:
			return null;
			Block_6:
			return null;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000330AC File Offset: 0x000314AC
		public object ParseClass(Type type)
		{
			object obj = Activator.CreateInstance(type);
			for (;;)
			{
				switch (this.NextToken)
				{
				case JSONParser.TOKEN.NONE:
					goto IL_35;
				case JSONParser.TOKEN.CURLY_OPEN:
					this.json.Read();
					continue;
				case JSONParser.TOKEN.CURLY_CLOSE:
					return obj;
				case JSONParser.TOKEN.COMMA:
					continue;
				}
				string text = this.ParseString(typeof(string)) as string;
				if (text == null)
				{
					goto Block_2;
				}
				if (this.NextToken != JSONParser.TOKEN.COLON)
				{
					goto Block_3;
				}
				this.json.Read();
				FieldInfo field = type.GetField(text);
				if (field != null)
				{
					object obj2 = this.ParseValue(field.FieldType);
					if (obj2 != null && (obj2.GetType() == field.FieldType || typeof(object) == field.FieldType))
					{
						field.SetValue(obj, obj2);
					}
				}
				else
				{
					this.ParseValue(typeof(object));
				}
			}
			IL_35:
			return null;
			Block_2:
			return null;
			Block_3:
			return null;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000331C4 File Offset: 0x000315C4
		public object ParseArray(Type type)
		{
			Type type2 = (!type.IsGenericType) ? type.GetElementType() : type.GetGenericArguments()[0];
			if (type2 == null)
			{
				type2 = JSONParser.objectType;
			}
			Type type3 = typeof(List<>).MakeGenericType(new Type[]
			{
				type2
			});
			IList list = Activator.CreateInstance(type3) as IList;
			this.json.Read();
			bool flag = true;
			while (flag)
			{
				JSONParser.TOKEN nextToken = this.NextToken;
				switch (nextToken)
				{
				case JSONParser.TOKEN.SQUARED_CLOSE:
					flag = false;
					break;
				default:
				{
					if (nextToken == JSONParser.TOKEN.NONE)
					{
						return null;
					}
					object value = this.ParseByToken(nextToken, type2);
					list.Add(value);
					break;
				}
				case JSONParser.TOKEN.COMMA:
					break;
				}
			}
			if (!type.IsGenericType)
			{
				Array array = Array.CreateInstance(type2, list.Count);
				int count = list.Count;
				while (count-- > 0)
				{
					array.SetValue(list[count], count);
				}
				return array;
			}
			return list;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000332DC File Offset: 0x000316DC
		public object ParseValue(Type type)
		{
			JSONParser.TOKEN nextToken = this.NextToken;
			if (Json.JsonConverters.ContainsKey(type))
			{
				return this.ParseDictionary(JSONParser.defaultDictionaryType);
			}
			return this.ParseByToken(nextToken, type);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00033314 File Offset: 0x00031714
		public object ParseByToken(JSONParser.TOKEN token, Type type)
		{
			object result;
			switch (token)
			{
			case JSONParser.TOKEN.STRING:
				result = this.ParseString(type);
				break;
			case JSONParser.TOKEN.NUMBER:
				result = this.ParseNumber(type);
				break;
			case JSONParser.TOKEN.TRUE:
				result = true;
				break;
			case JSONParser.TOKEN.FALSE:
				result = false;
				break;
			case JSONParser.TOKEN.NULL:
				result = null;
				break;
			default:
				switch (token)
				{
				case JSONParser.TOKEN.CURLY_OPEN:
					return this.ParseObject(type);
				case JSONParser.TOKEN.SQUARED_OPEN:
					return this.ParseArray(type);
				}
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000333B4 File Offset: 0x000317B4
		public object ParseString(Type type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.json.Read();
			bool flag = true;
			while (flag)
			{
				if (this.json.Peek() == -1)
				{
					break;
				}
				char nextChar = this.NextChar;
				if (nextChar != '"')
				{
					if (nextChar != '\\')
					{
						stringBuilder.Append(nextChar);
					}
					else if (this.json.Peek() == -1)
					{
						flag = false;
					}
					else
					{
						nextChar = this.NextChar;
						switch (nextChar)
						{
						case 'r':
							stringBuilder.Append('\r');
							break;
						default:
							if (nextChar != '"' && nextChar != '/' && nextChar != '\\')
							{
								if (nextChar != 'b')
								{
									if (nextChar != 'f')
									{
										if (nextChar == 'n')
										{
											stringBuilder.Append('\n');
										}
									}
									else
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else
							{
								stringBuilder.Append(nextChar);
							}
							break;
						case 't':
							stringBuilder.Append('\t');
							break;
						case 'u':
						{
							char[] array = new char[4];
							for (int i = 0; i < 4; i++)
							{
								array[i] = this.NextChar;
							}
							stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
							break;
						}
						}
					}
				}
				else
				{
					flag = false;
				}
			}
			string text = stringBuilder.ToString();
			if (type.IsEnum)
			{
				return Enum.Parse(type, text);
			}
			if (type == typeof(Guid))
			{
				return new Guid(text);
			}
			return text;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00033570 File Offset: 0x00031970
		public object ParseNumber(Type type)
		{
			string nextWord = this.NextWord;
			if (type.IsEnum)
			{
				int value;
				int.TryParse(nextWord, out value);
				return Enum.ToObject(type, value);
			}
			if (type == typeof(int))
			{
				int num;
				int.TryParse(nextWord, out num);
				return num;
			}
			if (type == typeof(uint))
			{
				uint num2;
				uint.TryParse(nextWord, out num2);
				return num2;
			}
			if (type == typeof(long))
			{
				long num3;
				long.TryParse(nextWord, out num3);
				return num3;
			}
			if (type == typeof(ulong))
			{
				ulong num4;
				ulong.TryParse(nextWord, out num4);
				return num4;
			}
			if (type == typeof(byte))
			{
				byte b;
				byte.TryParse(nextWord, out b);
				return b;
			}
			if (type == typeof(sbyte))
			{
				sbyte b2;
				sbyte.TryParse(nextWord, out b2);
				return b2;
			}
			if (type == typeof(float))
			{
				float num5;
				float.TryParse(nextWord, out num5);
				return num5;
			}
			if (type == typeof(double))
			{
				double num6;
				double.TryParse(nextWord, out num6);
				return num6;
			}
			if (nextWord.IndexOf('.') == -1)
			{
				int num7;
				int.TryParse(nextWord, out num7);
				return num7;
			}
			float num8;
			float.TryParse(nextWord, out num8);
			return num8;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000336FD File Offset: 0x00031AFD
		public void EatWhitespace()
		{
			while (char.IsWhiteSpace(this.PeekChar))
			{
				this.json.Read();
				if (this.json.Peek() == -1)
				{
					break;
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00033736 File Offset: 0x00031B36
		public char PeekChar
		{
			get
			{
				return Convert.ToChar(this.json.Peek());
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00033748 File Offset: 0x00031B48
		public char NextChar
		{
			get
			{
				return Convert.ToChar(this.json.Read());
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0003375C File Offset: 0x00031B5C
		public string NextWord
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!JSONParser.IsWordBreak(this.PeekChar))
				{
					stringBuilder.Append(this.NextChar);
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x000337B0 File Offset: 0x00031BB0
		public JSONParser.TOKEN NextToken
		{
			get
			{
				this.EatWhitespace();
				if (this.json.Peek() == -1)
				{
					return JSONParser.TOKEN.NONE;
				}
				char peekChar = this.PeekChar;
				switch (peekChar)
				{
				case ',':
					this.json.Read();
					return JSONParser.TOKEN.COMMA;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return JSONParser.TOKEN.NUMBER;
				default:
					switch (peekChar)
					{
					case '[':
						return JSONParser.TOKEN.SQUARED_OPEN;
					default:
						switch (peekChar)
						{
						case '{':
							return JSONParser.TOKEN.CURLY_OPEN;
						default:
							if (peekChar != '"')
							{
								string nextWord = this.NextWord;
								if (nextWord != null)
								{
									if (nextWord == "false")
									{
										return JSONParser.TOKEN.FALSE;
									}
									if (nextWord == "true")
									{
										return JSONParser.TOKEN.TRUE;
									}
									if (nextWord == "null")
									{
										return JSONParser.TOKEN.NULL;
									}
								}
								return JSONParser.TOKEN.NONE;
							}
							return JSONParser.TOKEN.STRING;
						case '}':
							this.json.Read();
							return JSONParser.TOKEN.CURLY_CLOSE;
						}
						break;
					case ']':
						this.json.Read();
						return JSONParser.TOKEN.SQUARED_CLOSE;
					}
					break;
				case ':':
					return JSONParser.TOKEN.COLON;
				}
			}
		}

		// Token: 0x04000895 RID: 2197
		private const string WORD_BREAK = "{}[],:\"";

		// Token: 0x04000896 RID: 2198
		private static Type objectType = typeof(object);

		// Token: 0x04000897 RID: 2199
		private static Type dictionaryType = typeof(Dictionary<, >);

		// Token: 0x04000898 RID: 2200
		private static Type defaultDictionaryType = typeof(Dictionary<string, object>);

		// Token: 0x04000899 RID: 2201
		private StringReader json;

		// Token: 0x02000172 RID: 370
		public enum TOKEN
		{
			// Token: 0x0400089B RID: 2203
			NONE,
			// Token: 0x0400089C RID: 2204
			CURLY_OPEN,
			// Token: 0x0400089D RID: 2205
			CURLY_CLOSE,
			// Token: 0x0400089E RID: 2206
			SQUARED_OPEN,
			// Token: 0x0400089F RID: 2207
			SQUARED_CLOSE,
			// Token: 0x040008A0 RID: 2208
			COLON,
			// Token: 0x040008A1 RID: 2209
			COMMA,
			// Token: 0x040008A2 RID: 2210
			STRING,
			// Token: 0x040008A3 RID: 2211
			NUMBER,
			// Token: 0x040008A4 RID: 2212
			TRUE,
			// Token: 0x040008A5 RID: 2213
			FALSE,
			// Token: 0x040008A6 RID: 2214
			NULL
		}
	}
}
