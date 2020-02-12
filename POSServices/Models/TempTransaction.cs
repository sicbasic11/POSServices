using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class TempTransaction
    {
        public long Id { get; set; }
        public string StoreCode { get; set; }
        public int? CustomerId { get; set; }
        public string RecieptCode { get; set; }
        public int? EmployeeId { get; set; }
        public int? Spgid { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalAmounTransaction { get; set; }
        public int? Qty { get; set; }
        public string TransactionId { get; set; }
        public int? TransactionType { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Edc1 { get; set; }
        public decimal? Edc2 { get; set; }
        public decimal? Change { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TimeSpan? TransactionTime { get; set; }
        public string ClosingStoreId { get; set; }
        public string ClosingShiftId { get; set; }
        public int? Status { get; set; }
        public string CustomerIdStore { get; set; }
        public string SequenceNumber { get; set; }
        public string Currency { get; set; }
        public int? PaymentType { get; set; }
        public string OpenShiftId { get; set; }
        public string OpenStoreId { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
    }
}
