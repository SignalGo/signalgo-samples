using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Queryable_DataExchanger.Models
{
    public class NewsInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
