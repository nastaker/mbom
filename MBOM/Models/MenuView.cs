using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class TreeMenuView
    {
        public int id { get; set; }
        public int? parentid { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string iconcls { get; set; }
        public List<TreeMenuView> children { get; set; }
    }
    public class MenuView
    {
        public int id { get; set; }
        public int? parentid { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string iconcls { get; set; }
        public int order { get; set; }
        public string description { get; set; }
        public List<MenuView> children { get; set; }
    }
}