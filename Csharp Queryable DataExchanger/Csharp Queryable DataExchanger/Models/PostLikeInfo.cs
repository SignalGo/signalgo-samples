using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Queryable_DataExchanger.Models
{
    public class PostLikeInfo
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public PostInfo PostInfo { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
