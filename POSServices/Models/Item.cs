using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Item
    {
        public Item()
        {
            DiscountItemSelected = new HashSet<DiscountItemSelected>();
            DiscountRetailLinesArticle = new HashSet<DiscountRetailLines>();
            DiscountRetailLinesArticleIdDiscountNavigation = new HashSet<DiscountRetailLines>();
            InventoryLines = new HashSet<InventoryLines>();
        }

        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string DepartmentType { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroupDesc { get; set; }
        public string ItemIdAlias { get; set; }
        public bool? IsServiceItem { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<DiscountItemSelected> DiscountItemSelected { get; set; }
        public virtual ICollection<DiscountRetailLines> DiscountRetailLinesArticle { get; set; }
        public virtual ICollection<DiscountRetailLines> DiscountRetailLinesArticleIdDiscountNavigation { get; set; }
        public virtual ICollection<InventoryLines> InventoryLines { get; set; }
    }
}
