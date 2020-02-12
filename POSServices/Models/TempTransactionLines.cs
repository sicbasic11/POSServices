using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class TempTransactionLines
    {
        public long Id { get; set; }
        public string ArticleId { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public string TransactionId { get; set; }
        public int? DiscountType { get; set; }
        public string DiscountCode { get; set; }
        public string Spgid { get; set; }
        public string Department { get; set; }
        public string DepartmentType { get; set; }
    }
}
