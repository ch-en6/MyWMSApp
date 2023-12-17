using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp1.dto;
using wms.param;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.param;
using wmsApp.pojo;
using MessageBox = System.Windows.Forms.MessageBox;

namespace wms
{
    class UserInfoApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result show()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/userInfo/show"));
        }
    }
    class RsaApi
    {
        public static HttpHelper http = new HttpHelper();
        public static String getJavaPublicKey()
        {
            return http.Get("/rsa");
        }

        public static void test()
        {
            
            Result result = http.GetEncryptedData("/rsa/test2");
            MessageBox.Show(result.data.ToString());

        }

        public static void modify()
        {
            DateTime date = new DateTime(2023, 12, 2);
            Store store = new Store(1, 1437831, "华南888仓", date, 320231202000001, 10, 1, "123");
            Result result = http.PostEncryptedData("/store/update", JsonHelper.DateObjectTOJson(store));
            MessageBox.Show(result.success.ToString());
        }

        //static void Main(string[] args)
        //{
        //    modify();
        //}
    }
    class UserApi
    {
        public static HttpHelper http = new HttpHelper();

        public static Result search(int page)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/search/{page}"));
        }
      

        public static Result save(User user)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/user/save", JsonHelper.DateObjectToJson<User>(user)));
        }
        //通过名称查询
        public static Result searchByName(int page, string name)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/searchByName/{page}/{name}"));
        }

        //通过id查询
        public static Result searchById(int page, long id)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/searchById/{page}/{id}"));
        }

        public static Result delete(long id)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/delete/{id}"));
        }

        public static Result update(User user)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/user/update", JsonHelper.DateObjectToJson<User>(user)));
        }

        public static Result resetPassword(User user)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/user/resetPassword", JsonHelper.DateObjectToJson<User>(user)));
        }
    }


    class ResourceApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result getResources()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/resource/all"));
        }

    }
    class LoginApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result login(long userId, string password)
        {
            LoginParams loginParams = new LoginParams(userId, password);
            Result result = JsonHelper.JSONToObject<Result>(http.Post("/login", JsonHelper.ObjectToJSON(loginParams)));
            if (!result.success) MessageBox.Show(result.errorMsg);
            return result;
        }
        public static Result logout()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/logout"));
        }
    }

    class MaterialApi
    {
        public static HttpHelper http = new HttpHelper();

        //显示
        public static Result search(int page)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/search/{page}"));
        }

        //通过id查询
        public static Result searchById(int page, long id)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchById/{page}/{id}"));
        }

        //通过名称查询
        public static Result searchByName(int page, string name)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchByName/{page}/{name}"));
        }

        //通过仓库id查询
        public static Result searchByHouseName(int page, string house_Name)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchByHouseName/{page}/{house_Name}"));
        }

        //通过类型查询
        public static Result searchByType(int page, string type)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchByType/{page}/{type}"));
        }

        //通过备注查询
        public static Result searchByComments(int page, string comments)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchByComments/{page}/{comments}"));
        }

        public static Result searchHouseName()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchHouseName"));
        }

        public static Result searchTypeName()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/typeName"));
        }

        public static Result addMaterial(Material material)
        {
            return JsonHelper.JSONToObject<Result>(http.Post($"/material/save", JsonHelper.DateObjectTOJson(material)));
        }

        //修改
        public static Result updateMaterial(Material material)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/material/update", JsonHelper.DateObjectToJson(material)));
        }

        public static Result deleteMaterial(long id)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/del/{id}"));
        }
    }

    class StoreApi
    {
        public static HttpHelper http = new HttpHelper();

        public static Result searchAll(int page)
        {
            return http.GetDncryptedData($"/store/searchAll/{page}");
        }
    }

    class PermissionTypesApi
    {
        public static HttpHelper http = new HttpHelper();
        /* 获取所有资源下的所有权限类型 */
        public static Result getPermissionTypesMap()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/permissiontype/types/map"));
        }

        public static Result delPermissionType(long resourceId, string type)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/permissiontype/del/{resourceId}/{type}"));
        }

        public static Result getSelectMap(long resourceId)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/permissiontype/select/map/{resourceId}"));
        }
    }

    class PermissionApi
    {
        public static HttpHelper http = new HttpHelper();
        /**
         * 访问权限
         */
        public static Result enter(String uri)
        {
            return JsonHelper.JSONToObject<Result>(http.Get(uri));
        }


        /*获取所有用户的姓名和id*/
        public static Result getUserNamesAndIds()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/user/getNamesAndIds"));
        }

        /*获取所有资源*/
        public static Result get_resources()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/resource"));
        }

        /*获取*/
        public static Result get_resourcetypesMap()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/permissiontype/map"));
        }

        /*获取该资源下的所有权限类型*/
        internal static Result get_resource_types(long resourceId)
        {
            return http.GetDncryptedData("/permission/types/" + resourceId);
            /*return JsonHelper.JSONToObject<Result>(http.Get("/permission/types/" + resourceId));*/
        }

        internal static Result get_permissions(int page, long resourceId)
        {
            return http.PostDncryptedData($"/permission/{page}/{resourceId}/get", "");
        }

        internal static Result updatePermission(bool isChecked, UpdatePermissionParams permissionParams)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/update/" + isChecked, JsonHelper.ObjectToJSON(permissionParams)));
        }

        internal static int get_totalpage()
        {
            string str = http.Get("/user/totalpage");
            if (str == null) return 0;
            return int.Parse(http.Get("/user/totalpage"));
        }

        internal static Result updatePermissionByuserId(long userId, long resourceId, bool? isChecked)
        {
            return JsonHelper.JSONToObject<Result>(http.Post($"/permission/update/{userId}/{resourceId}/{isChecked}", ""));
        }

        internal static Result savePermissions(AddPermissionParams param)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/add", JsonHelper.ObjectToJSON(param)));
        }

        internal static Result searchByUser(SearchPermissionParams condition)
        {
            return http.PostDncryptedData("/permission/search", JsonHelper.ObjectToJSON(condition));
        }

        internal static Result searchByRole(SearchPermissionParams condition)
        {
            return http.PostDncryptedData("/permission/search/role", JsonHelper.ObjectToJSON(condition));
        }
    }
}
