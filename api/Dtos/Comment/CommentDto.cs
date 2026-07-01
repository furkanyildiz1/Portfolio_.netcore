using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CommentDto
    {
        
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } =string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        //primary key budur yani vertabanda bu id ile yorumları birbirinden ayıracağız
        public int? StockId { get; set; }

    }
}