using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.Models
{
    public class HO_MsgModel
    {
        public class syncDownloadDetail
        {
            public string IdDetail { get; set; }
            public string JobId { get; set; }
            public string StoreId { get; set; }
            public string TableName { get; set; }
            public string DownloadPath { get; set; }
            public string RowFatch { get; set; }
            public string RowApplied { get; set; }
            public string Status { get; set; }
            public string SyncDate { get; set; }
            public string createTable { get; set; }
            public string downloadSession { get; set; }
            public int syncType { get; set; }
        }

        public class updateSyncDetailDownload
        {
            public string syncDetailsId { get; set; }
            public string status { get; set; }
            public string rowFatch { get; set; }
            public string rowApplied { get; set; }
            public string downloadSessionId { get; set; }
        }

        public class bracketSyncDetail
        {
            public List<updateSyncDetailDownload> syncDetailDownload { get; set; }
        }
        
        public class APIResponse
        {
            public String code { set; get; }
            public String message { set; get; }
        }

        public class ftpServer
        {
            public string serverName { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
        }
    }
}
