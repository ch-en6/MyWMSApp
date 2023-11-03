using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApp1.dto;
using wms.param;
using wms.pojo;
using wms.utils;
using wmsApp.param;

namespace wms
{
    class LoginApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result login(long userId,string password)
        {
            LoginParams loginParams=new LoginParams(userId, password);
            Result result = JsonHelper.JSONToObject<Result>(http.Post("/login", JsonHelper.ObjectToJSON(loginParams)));
            if (!result.success) MessageBox.Show(result.errorMsg);
            return result;
        }
        public static void logout()
        {
            JsonHelper.JSONToObject<Result>(http.Get("/logout"));
            return;
        }
    }

    class MaterialApi
    {
        public static HttpHelper http = new HttpHelper();

        public static Result search(int page)
        {

            return JsonHelper.JSONToObject<Result>(http.Get($"/material/search/{page}"));
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

        public static Result delPermissionType(long resourceId,string type)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/permissiontype/del/{resourceId}/{type}"));
        }
    }
    class PermissionApi
    {
        public static HttpHelper http = new HttpHelper();
        

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
            return JsonHelper.JSONToObject<Result>(http.Get("/permission/types/" + resourceId));
        }

        internal static Result get_permissions(int page, long resourceId)
        {
            return JsonHelper.JSONToObject<Result>(http.Post($"/permission/{page}/{resourceId}/get", ""));
        }

        internal static Result updatePermission(bool isChecked, UpdatePermissionParams permissionParams)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/update/" + isChecked, JsonHelper.ObjectToJSON(permissionParams)));
        }

        internal static int get_totalpage()
        {
            string str=http.Get("/user/totalpage");
            if (str == null) return 0;
            return int.Parse(http.Get("/user/totalpage"));
        }

        internal static Result delPermissionByuserId(long userId, long resourceId)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/del/" + userId + "/" + resourceId, ""));
        }

        internal static Result savePermissions(AddPermissionParams param)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/add", JsonHelper.ObjectToJSON(param)));
        }

        internal static Result searchByUser(SearchPermissionParams condition)
        {
           
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/search", JsonHelper.ObjectToJSON(condition)));
        }

        internal static Result searchByRole(SearchPermissionParams condition)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/permission/search/role", JsonHelper.ObjectToJSON(condition)));
        }
    }
}
