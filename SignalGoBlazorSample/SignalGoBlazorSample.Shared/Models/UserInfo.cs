using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalGoBlazorSample.Shared.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<BookInfo> Books { get; set; }
    }
}
