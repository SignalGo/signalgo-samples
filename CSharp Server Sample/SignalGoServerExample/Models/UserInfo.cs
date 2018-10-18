using System;
using System.Collections.Generic;
using System.Text;

namespace SignalGoServerExample.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<BookInfo> Books { get; set; }
    }
}
