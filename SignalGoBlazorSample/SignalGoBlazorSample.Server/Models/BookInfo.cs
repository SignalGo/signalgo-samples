using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalGoBlazorSample.Server.Models
{
    public class BookInfo
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
