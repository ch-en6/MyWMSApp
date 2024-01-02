using Newtonsoft.Json;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using WindowsFormsApp1.dto;
using wms.param;
using wms.pojo;
using wms.utils;
using wmsApp.param;
using wmsApp.pojo;
using static System.Net.WebRequestMethods;
using MessageBox = System.Windows.Forms.MessageBox;

namespace wms
{
    class MsmApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result sendCode(string phone)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/msm/send/{phone}"));
        }
        public static Result checkCode(string key, string code)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/msm/checkCode/{key}/{code}"));
        }
    }
    class UserInfoApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result show()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/userInfo/show"));
        }
        public static Result updatePhone(String newPhone)
        {
            //var data = new
            //{
            //    newPhone = newPhone
            //};
            //var jsonData = JsonConvert.SerializeObject(data);
            //return http.PostEncryptedData($"/userInfo/updatePhone", jsonData);
            return JsonHelper.JSONToObject<Result>(http.Get($"/userInfo/updatePhone/{newPhone}"));
        }
        public static Result updatePassword(string newPassword)
        {
            var data = new
            {
                newPassword = newPassword
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/userInfo/updatePassword", jsonData);
            //return JsonHelper.JSONToObject<Result>(http.Get($"/userInfo/updatePassword/{newPassword}"));
        }

    }
    class RsaApi
    {
        public static HttpHelper http = new HttpHelper();

        /*        public static void test(string type)
                {
                    var data = new
                    {
                        typeName = type
                    };
                    string jsonData = JsonConvert.SerializeObject(data);
                    MessageBox.Show(jsonData);
                    MessageBox.Show(http.PostDncryptedData("/material/typeMaterial", jsonData).data.ToString());
                }*/

        public static String getJavaPublicKey()
        {
            return http.Get("/rsa");
        }

        public static void test()
        {
            User user = new User();
            user.id = 1;
            user.name = "hi";
            List<User> list = new List<User>();

            list.Add(user);
            string listJson = JsonConvert.SerializeObject(list);
            var data = new
            {
                list = listJson
            };

            string json = JsonConvert.SerializeObject(data);
            Result result = http.PostEncryptedData("/rsa/test", json);
            MessageBox.Show(result.data.ToString());

        }

        public static void modify()
        {
            DateTime date = new DateTime(2023, 12, 2);
            Store store = new Store(1, 1437831, "华南888仓", date, 320231202000001, 10, 1, "123");
            Result result = http.PostEncryptedData("/store/update", JsonHelper.DateObjectTOJson(store));
            MessageBox.Show(result.success.ToString());
        }

    }
    class UserApi
    {
        public static HttpHelper http = new HttpHelper();

        public static Result search(int page)
        {
            return http.GetDncryptedData($"/user/search/{page}");
        }


        public static Result save(User user)
        {
            return JsonHelper.JSONToObject<Result>(http.Post("/user/save", JsonHelper.DateObjectToJson<User>(user)));
        }
        //通过名称查询
        public static Result searchByName(int page, string name)
        {
            return http.GetDncryptedData($"/user/searchByName/{page}/{name}");
        }

        //通过id查询
        public static Result searchById(int page, long id)
        {
            return http.GetDncryptedData($"/user/searchById/{page}/{id}");
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
        public static Result findAllUserName()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/findAllUserName"));
        }
        public static Result getNowUser()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/user/getNowUser"));
        }

    }


    class ResourceApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result getResources()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/resource/all"));
        }

        public static Result getUserResources()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/resource/get"));
        }


    }
    class LoginApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result login(long userId, string password)
        {
            LoginParams loginParams = new LoginParams(userId, password);
            //Result result = JsonHelper.JSONToObject<Result>(http.Post("/login", JsonHelper.ObjectToJSON(loginParams)));
            Result result = http.PostAndEncryptData("/login", JsonHelper.ObjectToJSON(loginParams));
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

        public static Result searchAll()
        {
            return http.GetDncryptedData($"/material/searchAll");
        }

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

        //查询所有仓库名
        public static Result searchHouseName()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/searchHouseName"));
        }

        //查询所有物料类别
        public static Result searchTypeName()
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/typeName"));
        }

        public static Result addMaterial(Material material)
        {
            return http.PostEncryptedData($"/material/save", JsonHelper.DateObjectTOJson(material));
        }

        public static Result addMaterialEqual(Material material)
        {
            return http.PostEncryptedData($"/material/saveEqual", JsonHelper.DateObjectToJson<Material>(material));
        }

        //修改
        public static Result updateMaterial(Material material)
        {
            return http.PostEncryptedData($"/material/update", JsonHelper.DateObjectToJson(material));
        }

        public static Result updateEqualType(Material material)
        {
            return http.PostEncryptedData($"/material/updateEqualType", JsonHelper.DateObjectToJson(material));
        }

        public static Result deleteMaterial(long id)
        {
            return JsonHelper.JSONToObject<Result>(http.Get($"/material/del/{id}"));
        }

        //查询某一类别对应的所有物料
        public static Result getMaterialNameByType(string type)
        {
            var data = new
            {
                typeName = type
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostDncryptedData($"/material/getMaterialNameByType", jsonData);
        }


        //通过物料名查询仓库
        public static Result getHouseByMaterialName(string name)
        {
            var data = new
            {
                name = name
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/material/getHouseByMaterialName", jsonData);
        }

        //根据名字和仓库，返回物料信息
        public static Result getMaterialByNameAndHouse(string name, string house)
        {
            var data = new
            {
                name = name,
                house = house
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/material/getMaterialByNameAndHouse", jsonData);
        }

        public static Result ifStoreOrDeliver(long materialId)
        {
            return http.GetDncryptedData($"/material/ifStoreOrDeliver/{materialId}");
        }

        public static Result houseByYearAndMaterialName(string year, string materialName)
        {
            var data = new
            {
                Year = year,
                MaterialName = materialName
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/material/houseByYearAndMaterialName", jsonData);
        }
    }

    class StoreApi
    {
        public static HttpHelper http = new HttpHelper();

        public static Result searchAll(int page)
        {
            return http.GetDncryptedData($"/store/searchAll/{page}");
        }

        public static Result searchCondition(long storeNo, string houseName, DateTime? startTime, DateTime? endTime, long materialId,
            long userId, string notes, int page)
        {
            if (startTime == null)
            {
                startTime = new DateTime(1970, 1, 1);
            }
            if (endTime == null)
            {
                endTime = DateTime.Now;
            }

            StoreConSearchParams store = new StoreConSearchParams(storeNo, houseName, startTime, endTime, materialId, userId, notes, page);
            return http.PostEncryptedData($"/store/conditionSearch", JsonHelper.DateObjectToJson<StoreConSearchParams>(store));
        }

        public static Result getStoreByDate(string Year, string Month)
        {
            var data = new
            {
                year = Year,
                month = Month
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/store/selectStoreByDate", jsonData);
        }

        public static Result getStoreByYear(string year, Material material)
        {
            var data = new
            {
                Year = year,
                id = material.id,
                HouseName = material.houseName
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/store/storeByYear", jsonData);
        }

        public static Result storeProcedure(List<Store> stores)
        {
            string storesJson = JsonConvert.SerializeObject(stores);
            var data = new
            {
                storeList = storesJson
            };
            return http.PostEncryptedData($"/store/callStoreProcedure", JsonHelper.DateObjectToJson(data));
        }


        public static Result findNameBetweenDates(DateTime? startTime, DateTime? endTime)
        {
            TimeParam time = new TimeParam(startTime, endTime);
            return http.PostDncryptedData($"/store/findNames", JsonHelper.DateObjectToJson<TimeParam>(time));
 
        }

        public static Result findCountByNameBetweenDates(DateTime? startTime, DateTime? endTime)
        {
            TimeParam time = new TimeParam(startTime, endTime);
            return http.PostDncryptedData($"/store/findCountByNames", JsonHelper.DateObjectToJson<TimeParam>(time));

        }
    }






    class DeliverApi
    {
        public static HttpHelper http = new HttpHelper();
        public static Result findNameBetweenDates(DateTime? startTime, DateTime? endTime)
        {
             TimeParam time = new TimeParam(startTime, endTime);
            return http.PostDncryptedData($"/deliver/findNames", JsonHelper.DateObjectToJson<TimeParam>(time));
        }


        public static Result findCountByNameBetweenDates(DateTime? startTime, DateTime? endTime)
        {
            TimeParam time = new TimeParam(startTime, endTime);
            return http.PostDncryptedData($"/deliver/findCountByNames", JsonHelper.DateObjectToJson<TimeParam>(time));
        }


        public static Result multiDelivery(List<Deliver> deliverList)
        {
        string listJson =JsonConvert.SerializeObject(deliverList);
        var data = new
        {
            list = listJson
        };

            return http.PostEncryptedData("/deliver/multiDelivery", JsonHelper.ObjectToJSON(data));
        }

        public static Result searchAll(int page)
        {
            return http.GetDncryptedData($"/deliver/searchAll/{page}");
        }

        public static Result searchCondition(long deliverNo, string houseName, DateTime? startTime, DateTime? endTime, long materialId,
            long userId, string notes, int page)
        {
            if (startTime == null)
            {
                startTime = new DateTime(1970, 1, 1);
            }
            if (endTime == null)
            {
                endTime = DateTime.Now;
            }

            DeliverConSearchParams deliver = new DeliverConSearchParams(deliverNo, houseName, startTime, endTime, materialId, userId, notes, page);
            return http.PostEncryptedData($"/deliver/conditionSearch", JsonHelper.DateObjectToJson<DeliverConSearchParams>(deliver));
        }

        public static Result getdeliverByDate(string Year, string Month)
        {
            var data = new
            {
                year = Year,
                month = Month
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/deliver/selectdeliverByDate", jsonData);
        }

        public static Result getDeliverByDate(string Year, string Month)
        {
            var data = new
            {
                year = Year,
                month = Month
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/deliver/selectDeliverByDate", jsonData);
        }

        public static Result getDeliverByYear(string year, Material material)
        {
            var data = new
            {
                Year = year,
                id = material.id,
                HouseName = material.houseName
            };
            var jsonData = JsonConvert.SerializeObject(data);
            return http.PostEncryptedData($"/deliver/deliverByYear", jsonData);
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
            return http.GetDncryptedData($"/permission/get/{page}/{resourceId}");
        }

        internal static Result updatePermission(UpdatePermissionParams permissionParams)
        {
            return http.PostEncryptedData("/permission/update", JsonHelper.ObjectToJSON(permissionParams));
            //return JsonHelper.JSONToObject<Result>(http.Post("/permission/update", JsonHelper.ObjectToJSON(permissionParams)));
        }

        internal static Result updatePermissionByuserId(long userId, long resourceId, bool? isChecked)
        {
            UpdatePermissionParams update = new UpdatePermissionParams(userId, resourceId, null, isChecked);
            return http.PostEncryptedData("/permission/update/all", JsonHelper.ObjectToJSON(update));
            /*return JsonHelper.JSONToObject<Result>(http.Post($"/permission/update/{userId}/{resourceId}/{isChecked}", ""));*/
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

        internal static Result getAllUser()
        {
            return JsonHelper.JSONToObject<Result>(http.Get("/user/getAll"));
        }
    }
}

