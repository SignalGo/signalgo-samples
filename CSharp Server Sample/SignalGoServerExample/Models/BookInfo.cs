using System;
using System.Collections.Generic;
using System.Text;

namespace SignalGoServerExample.Models
{
    public class BookInfo
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
