using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class JobTabletoSynchDetailDownload
    {
        public long SynchDetail { get; set; }
        public long JobId { get; set; }
        public string StoreId { get; set; }
        public string TableName { get; set; }
        public string DownloadPath { get; set; }
        public DateTime Synchdate { get; set; }
        public string CreateTable { get; set; }
        public int RowFatch { get; set; }
        public int Id { get; set; }
        public int Synctype { get; set; }
        public string TablePrimarykey { get; set; }
    }
}
