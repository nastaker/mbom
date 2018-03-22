using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class FileResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int size { get; set; }
        public string thumbnailUrl { get; set; }
    }
}