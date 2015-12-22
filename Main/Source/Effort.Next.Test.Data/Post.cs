using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effort.Next.Test.Data
{
    public class Post
    {
        public Int64 Id { get; set; } // Post_Id (Primary key)
        public string Subject { get; set; } // Subject
        public string Body { get; set; } // Body
        public DateTime? DateCreated { get; set; } // DateCreated
        public DateTime? DateModified { get; set; } // DateModified
        public Int64 AutId { get; set; } // AUT_ID

        // Foreign keys
        public virtual Author Author { get; set; } // FK_POST_AUTHOR
    }
}
