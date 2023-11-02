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





    //JSON方法
    public class JsonHelper
    {
        /// <summary>
        /// 把Json转成HashSet<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HashSet<string> ConvertToHashSet(string json)
        {
            HashSet<string> set = JsonConvert.DeserializeObject<HashSet<string>>(json);
            return set;
        }
        /// <summary>
        /// 把Json转成Map<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<R, T> ConvertToMap<R, T>(string input)
        {
            Dictionary<R, T> map;

            try
            {
                map = JsonConvert.DeserializeObject<Dictionary<R, T>>(input);
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
            return JsonConvert.DeserializeObject<List<T>>(html);
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
            //try
            //{
            //    return JsonConvert.SerializeObject(obj);
            //}
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
        public static string SerializeObject(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
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
            if (obj == null)
            {
                System.Windows.MessageBox.Show("数据为空！");
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(obj);
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
            /// 在调用函数中处理异常
            //try
            //{
            return JsonConvert.DeserializeObject<T>(obj);
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
            /// 在调用函数中处理异常
            //try
            //{
            return JsonConvert.DeserializeObject<IList<T>>(obj);
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
        public static string ObjectToJson<T>(T t, string ClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + ClassName + "\":[");
            string json = "";
            if (t != null)
            {
                sb.Append("{");
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append("\"" + pi.Name.ToString() + "\"");//.ToLower()
                    sb.Append(":");
                    sb.Append("\"" + pi.GetValue(t, null) + "\"");
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
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTableEmptyToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("}");
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// DataTable数据表转化成JSON格式字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null && dt.Rows.Count < 1)
                return "[]";

            var dic = new Dictionary<string, object>();
            dic.Add("result", true);
            dic.Add("msg", "");

            var list = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = new Dictionary<string, object>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.Add(dt.Columns[j].ColumnName.ToLower(), dt.Rows[i][j]);
                }
                list.Add(row);
            }
            dic.Add("data", list);
            return ObjectToJSON(dic);
        }
        /// <summary>  
        /// dataTable转换成Json格式(输出字段名全小写)  Unit项目中专用
        /// </summary>  
        /// <param name="count">总记录数,为0时自动获取dt行数</param>  
        public static string DataTableToJson(DataTable dt, int count)
        {
            if (dt == null && dt.Rows.Count < 1)
                return "[]";

            var dic = new Dictionary<string, object>();
            dic.Add("result", true);
            dic.Add("total", count == 0 ? dt.Rows.Count : count);

            var list = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = new Dictionary<string, object>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.Add(dt.Columns[j].ColumnName.ToLower(), dt.Rows[i][j]);
                }
                list.Add(row);
            }
            dic.Add("list", list);
            return ObjectToJSON(dic);
        }
        /// <summary>  
        /// dataTable转换成Json格式  除Unit项目外专用
        /// </summary>  
        /// <param name="count">总记录数</param>  
        /// <returns></returns>  
        public static string DataTable2ToJson(DataTable dt, int count)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Total\":\"" + count + "\",\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>  
        /// dataSet转换成Json格式(输出字段名全小写)  
        /// </summary>  
        public static string DataSetToJson(DataSet ds)
        {
            if (ds == null && ds.Tables.Count < 1)
                return "[]";

            var dic = new Dictionary<string, object>();
            dic.Add("result", true);

            var tbs = new List<object>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                var table = new Dictionary<string, object>();
                var list = new List<object>();
                if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        var row = new Dictionary<string, object>();
                        for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                        {
                            row.Add(ds.Tables[i].Columns[k].ColumnName.ToLower(), ds.Tables[i].Rows[j][k]);
                        }
                        list.Add(row);
                    }
                }
                table.Add("count", list.Count);
                table.Add("list", list);
                tbs.Add(table);
            }
            dic.Add("table", tbs);
            return ObjectToJSON(dic);
        }
        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dt">DataTable数据集</param>
        /// <param name="dtName">json名</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dtName);
            sb.Append("\":[");
            if (IsExistRows(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        sb.Append("\"");
                        sb.Append(dc.ColumnName);
                        sb.Append("\":\"");
                        if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                            sb.Append(dr[dc]).Replace("\\", "/");
                        else
                            sb.Append("&nbsp;");
                        sb.Append("\",");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]}");
            return JsonCharFilter(sb.ToString());
        }
        /// <summary>  
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string DatasetToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DataTableToJson(dt));
                json.Append("}");
            }
            return json.ToString();
        }

        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2ToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 数据行转Json
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns></returns>
        public static string DataRowToJson(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (DataColumn dc in dr.Table.Columns)
            {
                sb.Append("\"");
                sb.Append(dc.ColumnName);
                sb.Append("\":\"");
                if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                    sb.Append(dr[dc]);
                else
                    sb.Append("&nbsp;");
                sb.Append("\",");
            }
            sb = sb.Remove(0, sb.Length - 1);
            sb.Append("},");
            return sb.ToString();
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

        #region ListToJson
        /// <summary>
        /// list 转换json格式
        /// </summary>
        /// <param name="jsonName">类名</param>
        /// <param name="objlist">list集合</param>
        /// <returns></returns>
        public static string ListToJson<T>(List<T> objlist, string jsonName)
        {
            string result = "{";
            //如果没有给定类的名称， 指定一个
            if (jsonName.Equals(string.Empty))
            {
                object o = objlist[0];
                jsonName = o.GetType().ToString();
            }
            result += "\"" + jsonName + "\":[";
            //处理第一行前面不加","号
            bool firstline = true;
            foreach (object oo in objlist)
            {
                if (!firstline)
                {
                    result = result + "," + ObjectToJson(oo);
                }
                else
                {
                    result = result + ObjectToJson(oo) + "";
                    firstline = false;
                }
            }
            return result + "]}";
        }
        /// <summary>
        /// 单个对象转换json
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns></returns>
        private static string ObjectToJson(object o)
        {
            string result = "{";
            List<string> ls_propertys = new List<string>();
            ls_propertys = GetObjectProperty(o);
            foreach (string str_property in ls_propertys)
            {
                if (result.Equals("{"))
                {
                    result = result + str_property;
                }
                else
                {
                    result = result + "," + str_property + "";
                }
            }
            return result + "}";
        }
        /// <summary>
        /// 获取对象属性
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns></returns>
        private static List<string> GetObjectProperty(object o)
        {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                propertyslist.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(o, null) + "\"");

            }
            return propertyslist;
        }


        #endregion

        public static string HashtableToJson(Hashtable data, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dtName);
            sb.Append("\":[{");
            foreach (object key in data.Keys)
            {
                object value = data[key];
                sb.Append("\"");
                sb.Append(key);
                sb.Append("\":\"");
                if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                {
                    sb.Append(value).Replace("\\", "/");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append("\",");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("}]}");
            return JsonCharFilter(sb.ToString());
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

        #region 检查DataTable 是否有数据行
        /// <summary>
        /// 检查DataTable 是否有数据行
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static bool IsExistRows(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
                return true;

            return false;
        }
        #endregion

        #region DataTable转JSon
        /// <summary>
        /// DataTable转JSon
        /// </summary>
        /// <param name="table">table</param>
        /// <returns></returns>
        public static string DataTableToJsonLB(DataTable table)
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
        }
        #endregion

        /// <summary> 
        /// 将JSON文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
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


    /// <summary>
    /// Json序列化和反序列化辅助类 
    /// </summary>
/*    public class JsonHelper
    {
        /// <summary> 
        /// Json序列化 
        /// </summary> 
        public static string JsonSerializer<T>(T obj)
        {
            string jsonString = string.Empty;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    jsonString = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                jsonString = string.Empty;
            }
            return jsonString;
        }

        /// <summary> 
        /// Json反序列化
        /// </summary> 
        public static T JsonDeserialize<T>(string jsonString)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());//typeof(T)
                    T jsonObject = (T)ser.ReadObject(ms);
                    ms.Close();

                    return jsonObject;
                }
            }
            catch
            {
                return default(T);
            }
        }

        // 将 DataTable 序列化成 json 字符串
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return "\"\"";
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return myJson.Serialize(list);
        }

        // 将对象序列化成 json 字符串
        public static string ObjectToJson(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.Serialize(obj);
        }

        // 将 json 字符串反序列化成对象
        public static object JsonToObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.DeserializeObject(json);
        }

        // 将 json 字符串反序列化成对象
        public static T JsonToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.Deserialize<T>(json);
        }
    }
*/
}
