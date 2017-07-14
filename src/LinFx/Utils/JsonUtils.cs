﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace LinFx.Utils
{
    public static class JsonUtils
    {
		private const char TypeSeperator = '|';

		/// <summary>
		/// Serializes an object with a type information included.
		/// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
		/// </summary>
		public static string SerializeWithType(object obj)
		{
			return SerializeWithType(obj, obj.GetType());
		}

		/// <summary>
		/// Serializes an object with a type information included.
		/// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
		/// </summary>
		public static string SerializeWithType(object obj, Type type)
		{
			var serialized = obj.ToJsonString();

			return string.Format(
				"{0}{1}{2}",
				type.AssemblyQualifiedName,
				TypeSeperator,
				serialized);
		}

		/// <summary>
		/// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
		/// </summary>
		public static T DeserializeWithType<T>(string serializedObj)
		{
			return (T)DeserializeWithType(serializedObj);
		}

		/// <summary>
		/// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
		/// </summary>
		public static object DeserializeWithType(string serializedObj)
		{
			var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
			var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
			var serialized = serializedObj.Substring(typeSeperatorIndex + 1);

			var options = new JsonSerializerSettings();
			//options.Converters.Insert(0, new AbpDateTimeConverter());

			return JsonConvert.DeserializeObject(serialized, type, options);
		}
	}

	public static class JsonExtensions
	{
		/// <summary>
		/// Converts given object to JSON string.
		/// </summary>
		/// <returns></returns>
		public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
		{
			var options = new JsonSerializerSettings();

			if(camelCase)
				options.ContractResolver = new CamelCasePropertyNamesContractResolver();

			if(indented)
				options.Formatting = Formatting.Indented;

			//options.Converters.Insert(0, new DateTimeConverter());

			return JsonConvert.SerializeObject(obj, options);
		}
	}

	/// <summary>
	/// 转化小写
	/// </summary>
	public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
    /// <summary>  
    /// Newtonsoft.Json序列化扩展特性  
    /// <para>DateTime序列化（输出为时间戳）</para>  
    /// </summary>  
    public class TimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ConvertIntDateTime(int.Parse(reader.Value.ToString()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ConvertDateTimeInt((DateTime)value));
        }

        public static DateTime ConvertIntDateTime(int aSeconds)
        {
            return new DateTime(1970, 1, 1).AddSeconds(aSeconds);
        }

        public static int ConvertDateTimeInt(DateTime aDT)
        {
            return (int)(aDT - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }

    /// <summary>  
    /// Newtonsoft.Json序列化扩展特性  
    /// <para>String Unicode 序列化（输出为Unicode编码字符）</para>  
    /// </summary>  
    public class UnicodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ToUnicode(value.ToString()));
        }

        public static string ToUnicode(string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2)
            {
                r += "\\u" + bts[i + 1].ToString("X").PadLeft(2, '0') + bts[i].ToString("X").PadLeft(2, '0');
            }
            return r;
        }
    }
}
