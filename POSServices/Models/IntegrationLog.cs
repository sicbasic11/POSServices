using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class IntegrationLog
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string RefNumber { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        public int? NrOfSuccessfullTransactions { get; set; }
        public int? NrOfFailedTransactions { get; set; }
        public int? NumOfLineSubmited { get; set; }
        public string Json { get; set; }
    }
}
