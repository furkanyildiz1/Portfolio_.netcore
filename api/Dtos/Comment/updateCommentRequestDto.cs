using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class updateCommentRequestDto
    {

        [Required]
        [MaxLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long.")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [MaxLength(500, ErrorMessage = "Title cannot exceed 500 characters.")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
        public string Title { get; set; } =string.Empty;
    }
}