namespace wms.utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Data;
    using System.Collections;
    using System.Web;
    using Newtonsoft.Json;
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;



    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;


    //JSON方法
    public class JsonHelper
    {
       
        /// <summary>
        /// 把Json转成Map<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<R, T> ConvertToMap<R, T>(string input)
        {
            Dictionary<R, T> map;
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            try
            {
                map = JsonConvert.DeserializeObject<Dictionary<R, T>>(input,settings);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"无法将 JSON 字符串转换为 {typeof(Dictionary<R, T>)} 类型的对象：{ex.Message}");
                map = null;
            }

            return map;
        }


        /// <summary>
        /// 把Json转成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string html)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            return JsonConvert.DeserializeObject<List<T>>(html,settings);
        }
        #region // 格式化Json字符串
        /// <summary>
        /// 格式化Json字符串，使之能转换成List
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FormatJson(string value)
        {
            string p = @"(new Date)\(+([0-9,-]+)+(\))";
            MatchEvaluator matchEvaluator = new MatchEvaluator(FormatJsonMatch);
            Regex reg = new Regex(p);
            bool isfind = reg.IsMatch(value);
            value = reg.Replace(value, matchEvaluator);
            return value;
        }
        /// <summary>
        /// 将Json序列化的时间由new Date(1373387734703)转为字符串"\/Date(1373387734703)\/"
        /// </summary>
        private static string FormatJsonMatch(Match m)
        {
            return string.Format("\"\\/Date({0})\\/\"", m.Groups[2].Value);
        }

        #endregion // 格式化Json字符串

        #region // 格式化日期
        /// <summary>
        /// 将Json序列化的时间由new Date(1373390933250) 或Date(1373390933250)或"\/Date(1373390933250+0800)\/"
        /// 转为2013-07-09 17:28:53
        /// 主要用于传给前台
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FormatJsonDate(string value)
        {
            string p = @"(new Date)\(+([0-9,-]+)+(\))";
            MatchEvaluator matchEvaluator = new MatchEvaluator(FormatJsonDateMatch);
            Regex reg = new Regex(p);
            value = reg.Replace(value, matchEvaluator);

            p = @"(Date)\(+([0-9,-]+)+(\))";
            matchEvaluator = new MatchEvaluator(FormatJsonDateMatch);
            reg = new Regex(p);
            value = reg.Replace(value, matchEvaluator);

            p = "\"\\\\\\/" + @"Date(\()([0-9,-]+)([+])([0-9,-]+)(\))" + "\\\\\\/\"";
            matchEvaluator = new MatchEvaluator(FormatJsonDateMatch);
            reg = new Regex(p);
            value = reg.Replace(value, matchEvaluator);

            return value;

        }
        /// <summary>
        /// 将Json序列化的时间由Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string FormatJsonDateMatch(Match m)
        {

            string result = string.Empty;

            DateTime dt = new DateTime(1970, 1, 1);

            dt = dt.AddMilliseconds(long.Parse(m.Groups[2].Value));

            dt = dt.ToLocalTime();

            result = dt.ToString("yyyy-MM-dd HH:mm:ss");

            return result;
        }
        #endregion // 格式化日期

        #region // 属性和变量转换
        /// <summary>
        /// 属性转变量
        /// 把"<address>k__BackingField":"1",
        /// 转成"address":"1",
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AttributeToVariable(string value)
        {
            string p = @"\<([A-Z,a-z,0-9,_]*)\>k__BackingField";
            MatchEvaluator matchEvaluator = new MatchEvaluator(AttributeToVariableMatch);
            Regex reg = new Regex(p);
            bool isfind = reg.IsMatch(value);
            value = reg.Replace(value, matchEvaluator);
            return value;
        }
        private static string AttributeToVariableMatch(Match m)
        {
            return m.Groups[1].Value;
        }

        /// <summary>
        /// 变量转属性
        /// 把"address":"1",
        /// 转成"<address>k__BackingField":"1",
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string VariableToAttribute(string value)
        {
            string p = "\\\"([A-Z,a-z,0-9,_]*)\\\"\\:";
            MatchEvaluator matchEvaluator = new MatchEvaluator(VariableToAttributeMatch);
            Regex reg = new Regex(p);
            bool isfind = reg.IsMatch(value);
            value = reg.Replace(value, matchEvaluator);
            return value;
        }
        private static string VariableToAttributeMatch(Match m)
        {
            return string.Format("\"<{0}>k__BackingField\":", m.Groups[1].Value);
        }
 
        #endregion // 属性和变量转换

        /// <summary> 
        /// 对象转JSON默认为方法 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary>
        /// 对象转JSON另一种方法
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>格式的字符串</returns>
        public static string DateObjectTOJson(object obj)
        {
            try
            {
                var settings = new JsonSerializerSettings();
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

                // 创建 UnixDateTimeConverter 实例，并设置日期转换为 Java 的 UNIX 时间戳格式（以毫秒为单位）
                var converter = new UnixDateTimeConverter();
             
                settings.Converters.Add(converter);


                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.SerializeObject(): " + ex.Message);
            }
        }
        /// <summary>
        ///JSON反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T JSONToObject<T>(string obj)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            if (obj == null)
            {
                System.Windows.MessageBox.Show("数据为空！");
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(obj,settings);
        }
        /// <summary>
        /// JSON反序列化对象
        /// </summary>
        /// <typeparam name="T">匿名对象</typeparam>
        /// <param name="obj">字符串</param>
        /// <param name="anonymous">匿名对象</param>
        /// <returns>和匿名对象同结构的新匿名对象</returns>
        public static T JSONToObject<T>(string obj, T anonymous)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            /// 在调用函数中处理异常
            //try
            //{
            return JsonConvert.DeserializeObject<T>(obj,settings);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            //}
        }
        /// <summary>
        /// JSON反序列化为匿名对象列表
        /// </summary>
        /// <typeparam name="T">匿名对象</typeparam>
        /// <param name="obj">字符串</param>
        /// <param name="anonymous">匿名对象</param>
        /// <returns>匿名对象列表</returns>
        public static IList<T> JSONToObjectIList<T>(string obj, T anonymous)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            /// 在调用函数中处理异常
            //try
            //{
            return JsonConvert.DeserializeObject<IList<T>>(obj,settings);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            //} 
        }
        /// <summary>
        /// JSON数据打印出来
        /// </summary>
        /// <param name="msg"></param>
        public static void ResponseJson(object msg)
        {
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(msg));
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// JSON数据打印出来,多是异步时调用才传入请求上下文
        /// </summary>
        /// <param name="msg"></param>
        public static void ResponseJson(HttpContext cnt, object msg)
        {
            cnt.Response.Write(JsonConvert.SerializeObject(msg));
            cnt.Response.End();
        }
        /// <summary>
        /// 返回json格式请求结果
        /// </summary>
        /// <param name="result">请求结果</param>
        /// <param name="msg">返回信息</param>
        public static void WriteJsonResult(HttpContext cnt, bool result, string msg)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            dic.Add("msg", msg);
            cnt.Response.Write(ObjectToJSON(dic));
        }
        /// <summary>
        /// 获取json格式返回结果
        /// </summary>
        /// <param name="result">请求结果 成功：true 失败：false</param>
        /// <param name="data">返回数据</param>
        /// <param name="msg">返回信息</param>
        public static string GetJsonResult(bool result, string msg, object data)
        {
            var ht = new Hashtable();
            ht.Add("result", result);
            ht.Add("msg", msg);
            ht.Add("data", data);
            return JsonHelper.ObjectToJSON(ht);
        }
        /// <summary>
        /// 泛型接口转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string IListToJson<T>(IList<T> list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                return jss.Serialize(list);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary>
        /// 泛型接口转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ClassName"></param>
        /// <returns>{"rows":[{"c_id":"21bb6911-af52-42a4-9732-24a6e8384411","eav_id":"4a9fe8ca-112a-47c0-b074-229837cfe6e6","e_id":"cfe929e3-accd-4efb-910b-07705077b6d6","ea_id":"","ea_name":"555","eav_value":"555","eav_memo":"","sud":"0"}]}</returns>
        public static string IListToJson<T>(IList<T> list, string ClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + ClassName + "\":[");
            foreach (T t in list)
            {
                sb.Append(ObjectToJson(t) + ",");
            }
            string _temp = sb.ToString().TrimEnd(',');
            _temp += "]}";
            return _temp;
        }
        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>{"SM_ID":"71","SM_Img":"title.gif","SM_Memo":"当前位置：会所服务 ─ 宾客管理 ─ 宾客列表"}</returns>
        public static string ObjectToJson<T>(T t)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                return jss.Serialize(t);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary>
        /// 对象转Json（重载）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ClassName"></param>
        /// <returns>{"menu":[{"SM_ID":"71","SM_Img":"title.gif","SM_Memo":"当前位置：会所服务 ─ 宾客管理 ─ 宾客列表"}]}</returns>
        public static string ObjectToJson<T>(T t, string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + className + "\":[");
            string json = "";
            if (t != null)
            {
                sb.Append("{");
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append("\"" + pi.Name.ToString() + "\"");//.ToLower()
                    sb.Append(":");
                    if (pi.PropertyType == typeof(DateTime))
                    {
                        string date = ((DateTime)pi.GetValue(t, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        sb.Append("\"" + date + "\"");   // 将DateTime类型转换成Java所需的日期格式
                    }
                    else
                    {
                        sb.Append("\"" + pi.GetValue(t, null) + "\"");
                    }
                    sb.Append(",");
                }
                json = sb.ToString().TrimEnd(',');
                json += "}]}";
            }
            return json;
        }

        /// <summary>
        /// List转成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="IL"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(IList<T> IL, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    Type type = obj.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pis.Length; j++)
                    {
                        Json.Append("\"" + pis[j].Name.ToString() + "\":\"" + pis[j].GetValue(IL[i], null) + "\"");
                        if (j < pis.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < IL.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        /// 将数组转换为JSON格式的字符串
        /// </summary>
        /// <typeparam name="T">数据类型，如string,int ...</typeparam>
        /// <param name="list">泛型list</param>
        /// <param name="propertyname">JSON的类名</param>
        /// <returns></returns>
        public static string ArrayToJson<T>(List<T> list, string propertyname)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                sb.Append("[{\"");
                sb.Append(propertyname);
                sb.Append("\":[");
                foreach (T t in list)
                {
                    sb.Append("\"");
                    sb.Append(t.ToString());
                    sb.Append("\",");
                }
                string _temp = sb.ToString();
                _temp = _temp.TrimEnd(',');
                _temp += "]}]";
                return _temp;
            }
            else
                return "";
        }
 
        /// <summary>
        /// 数组转Json
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string ArrayToJson(string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strs.Length; i++)
            {
                sb.AppendFormat("'{0}':'{1}',", i + 1, strs[i]);
            }
            if (sb.Length > 0)
                return "{" + sb.ToString().TrimEnd(',') + "}";
            return "";
        }

       
        /// <summary>  
        /// Json特符字符过滤
        /// </summary>  
        /// <param name="sourceStr">要过滤的源字符串</param>  
        /// <returns>返回过滤的字符串</returns>  
        private static string JsonCharFilter(string sourceStr)
        {
            return sourceStr;
        }


        /// <summary> 
        /// 将JSON文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据行的字典</returns>
        public static Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }
    }
}
