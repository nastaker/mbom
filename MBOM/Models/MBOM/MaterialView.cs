using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class MaterialView
    {
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public bool ItemSale { get; set; }
        public bool ItemPurchase { get; set; }
        public bool ItemSelfmade { get; set; }
        public bool ItemVirtual { get; set; }
        public bool ItemCombine { get; set; }
    }
    public class ProductView
    {
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
    }
}