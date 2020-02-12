using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSServices.Models
{
    public partial class DiscountSetup
    {
        public DiscountSetup()
        {
            DiscountSetupLines = new HashSet<DiscountSetupLines>();
        }

        public string DiscountCode { get; set; }
        public int DiscountCategory { get; set; }
        public string DiscountName { get; set; }
        public int CustomerGroupId { get; set; }
        public string DiscountPartner { get; set; }
        public int? DiscountType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm }")]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm }")]
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public decimal? DiscountCash { get; set; }
        public int? DiscountPercent { get; set; }
        public int? QtyMin { get; set; }
        public int? QtyMax { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long Id { get; set; }
        public int? TableCode { get; set; }
        public int? Multi { get; set; }

        public virtual ICollection<DiscountSetupLines> DiscountSetupLines { get; set; }
    }

    public class discountSetupList
    {
        public List<DiscountSetup> discounts { get; set; }
    }
}
