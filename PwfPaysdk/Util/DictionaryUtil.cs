using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Util
{
    public static class DictionaryUtil
    {

        public static object GetDicValue(Dictionary<string, object> dic, string keyName)
        {
            if (dic.ContainsKey(keyName))
            {
                return dic[keyName];
            }
            return null;
        }

        public static string GetDicValue(Dictionary<string, string> dic, string keyName)
        {
            if (dic != null && dic.ContainsKey(keyName))
            {
                return dic[keyName];
            }
            return null;
        }

        public static Dictionary<string, object> ObjToDictionary(Dictionary<string, object> iputObj)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in iputObj.Keys)
            {
                if (iputObj[key] is JArray)
                {
                    List<object> objList = ((JArray)iputObj[key]).ToObject<List<object>>();
                    result.Add(key, ConvertList(objList));
                }
                else if (iputObj[key] is JObject)
                {
                    Dictionary<string, object> dicObj = ((JObject)iputObj[key]).ToObject<Dictionary<string, object>>();
                    result.Add(key, ObjToDictionary(dicObj));
                }
                else
                {
                    result.Add(key, iputObj[key]);
                }
            }
            return result;
        }

        private static List<object> ConvertList(List<object> inputList)
        {
            List<object> result = new List<object>();
            foreach (var obj in inputList)
            {
                if (obj is JArray)
                {
                    List<object> listObj = ((JArray)obj).ToObject<List<object>>();
                    result.Add(ConvertList(listObj));
                }
                else if (obj is JObject)
                {
                    Dictionary<string, object> dicObj = ((JObject)obj).ToObject<Dictionary<string, object>>();
                    result.Add(ObjToDictionary(dicObj));
                }
                else
                {
                    result.Add(obj);
                }
            }
            return result;
        }


        public static IDictionary<string, object> GetSortedMap(Dictionary<string, object> Params)
        {

            IDictionary<string, object> sortedMap = new SortedDictionary<string, object>(Params, StringComparer.Ordinal);

            return sortedMap;
        }

        public static string ToJsonString(IDictionary<string, object> input)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (var pair in input)
            {
                result.Add(pair.Key, pair.Value);
            }
            return JsonConvert.SerializeObject(result);
        }


		public static T ToObject<T>(IDictionary dict) where T : class, new()
		{
			T obj = new T();
			if (dict == null)
			{
				return null;
			}
			return ToObject(dict.Keys.Cast<string>().ToDictionary((string key) => key, (string key) => dict[key]), obj);
		}

		public static T ToObject<T>(Dictionary<string, object> dict, T obj) where T : class
		{
			if (dict == null)
			{
				return null;
			}

			PropertyInfo[] properties = obj.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				Type propertyType = propertyInfo.PropertyType;
				NameInMapAttribute nameInMapAttribute = propertyInfo.GetCustomAttribute(typeof(NameInMapAttribute)) as NameInMapAttribute;
				string key = (nameInMapAttribute == null) ? propertyInfo.Name : nameInMapAttribute.Name;

				if (dict.ContainsKey(key))
				{
					object obj2 = dict[key];
					if (obj2 == null)
					{
						propertyInfo.SetValue(obj, obj2);
					}
					else
					{
						propertyInfo.SetValue(obj, MapObj(propertyType, obj2));
					}
				}
			}
			return obj;
		}

		private static object MapObj(Type propertyType, object value)
		{
			if (value == null)
			{
				return null;
			}
			if (typeof(IList).IsAssignableFrom(value.GetType()) && !typeof(Array).IsAssignableFrom(value.GetType()))
			{
				object obj = Activator.CreateInstance(propertyType);
				Type type = propertyType.GetGenericArguments()[0];
				{
					foreach (var item in (IList)value)
					{
						MethodInfo method = propertyType.GetMethod("Add", new Type[1] { type });
						if (method != null)
						{
							if (item == null)
							{
								method.Invoke(obj, new object[1]);
								continue;
							}
							object obj2 = MapObj(type, item);
							method.Invoke(obj, new object[1] {
							obj2
						});
						}
					}
					return obj;
				}
			}

			if (typeof(IDictionary).IsAssignableFrom(propertyType))
			{
				IDictionary dictionary = (IDictionary)value;
				if (propertyType.Equals(typeof(IDictionary)))
				{
					return dictionary;
				}
				IDictionary dictionary2 = (IDictionary)Activator.CreateInstance(propertyType);
				Type propertyType2 = propertyType.GetGenericArguments()[1];
				{
					foreach (DictionaryEntry item2 in dictionary)
					{
						if (item2.Value == null)
						{
							dictionary2.Add(item2.Key, null);
							continue;
						}
						item2.Value!.GetType();
						dictionary2.Add(item2.Key, MapObj(propertyType2, item2.Value));
					}
					return dictionary2;
				}
			}
			if (propertyType.Equals(typeof(int)) && value is long)
			{
				return Convert.ToInt32((long)value);
			}
			if (propertyType == typeof(int?))
			{
				return Convert.ToInt32(value);
			}
			if (propertyType == typeof(long?))
			{
				return Convert.ToInt64(value);
			}
			if (propertyType == typeof(float?))
			{
				return Convert.ToSingle(value);
			}
			if (propertyType == typeof(double?))
			{
				return Convert.ToDouble(value);
			}
			if (propertyType == typeof(bool?))
			{
				return Convert.ToBoolean(value);
			}
			if (propertyType == typeof(short?))
			{
				return Convert.ToInt16(value);
			}
			if (propertyType == typeof(ushort?))
			{
				return Convert.ToUInt16(value);
			}
			if (propertyType == typeof(uint?))
			{
				return Convert.ToUInt32(value);
			}
			if (propertyType == typeof(ulong?))
			{
				return Convert.ToUInt64(value);
			}
			return Convert.ChangeType(value, propertyType);
		}

		public static Dictionary<string, object> ToMap(object model)
		{
			if (model == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			PropertyInfo[] properties = model.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				Type propertyType = propertyInfo.PropertyType;
				NameInMapAttribute nameInMapAttribute = propertyInfo.GetCustomAttribute(typeof(NameInMapAttribute)) as NameInMapAttribute;
				string key = (nameInMapAttribute == null) ? propertyInfo.Name : nameInMapAttribute.Name;
				dictionary.Add(key, ToMapFactory(propertyType, propertyInfo.GetValue(model)));
			}
			return dictionary;
		}


		private static object ToMapFactory(Type type, object value)
		{
			if (value == null)
			{
				return null;
			}
			if (typeof(IList).IsAssignableFrom(type) && !typeof(Array).IsAssignableFrom(type))
			{
				IList list = (IList)value;
				Type type2 = type.GetGenericArguments()[0];
				List<object> list2 = new List<object>();
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] == null)
					{
						list2.Add(null);
					}
					else
					{
						list2.Add(ToMapFactory(type2, list[i]));
					}
				}
				return list2;
			}
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				IDictionary obj = (IDictionary)value;
				IDictionary dictionary = new Dictionary<string, object>();
				{
					foreach (DictionaryEntry item in obj)
					{
						if (item.Value == null)
						{
							dictionary.Add(item.Key, null);
							continue;
						}
						Type type3 = item.Value!.GetType();
						dictionary.Add(item.Key, ToMapFactory(type3, item.Value));
					}
					return dictionary;
				}
			}

			return value;
		}
	}

}
