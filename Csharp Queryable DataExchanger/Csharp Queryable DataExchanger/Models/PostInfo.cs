using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Queryable_DataExchanger.Models
{
    public class PostInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }

        public UserInfo UserInfo { get; set; }

        public List<PostLikeInfo> PostLikes { get; set; }
        public List<FileInfo> Files { get; set; }
        
    }
}
