using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    class SearchPermissionParams
{
        public static int PERMISSION_SEARCH_PAGE_SIZE = 5;
        public long? userId;
        public String empName;

        public String role;

        public long? resourceId;

        public int? currentPage;

        public int? pageSize = PERMISSION_SEARCH_PAGE_SIZE;

        public SearchPermissionParams(long? userId, string empName, string role, long resourceId, int currentPage)
        {
            this.userId = userId;
            this.empName = empName;
            this.role = role;
            this.resourceId = resourceId;
            this.currentPage = currentPage;
        }

        public SearchPermissionParams(long? userId, string empName, string role, long resourceId, int currentPage, int pageSize) : this(userId, empName, role, resourceId, currentPage)
        {
            this.pageSize = pageSize;
        }
    }
}
