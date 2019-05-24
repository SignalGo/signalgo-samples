using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Queryable_DataExchanger.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CountryName { get; set; }
        public List<NewsInfo> NewsInfoes { get; set; }
        public List<ArticleInfo> Articles { get; set; }
        public List<PostInfo> Posts { get; set; }
        public List<PostLikeInfo> PostLikes { get; set; }

    }
}
