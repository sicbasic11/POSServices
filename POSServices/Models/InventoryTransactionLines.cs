using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class InventoryTransactionLines
    {
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public int? Qty { get; set; }
        public int? InventoryTransactionId { get; set; }
        public string ArticleName { get; set; }
        public int? RecieveQty { get; set; }
        public int? Urdlix { get; set; }
        public int? Urridl { get; set; }
        public decimal? ValueSalesPrice { get; set; }
        public string PackingNumber { get; set; }
        public string Urridn { get; set; }
    }
}
