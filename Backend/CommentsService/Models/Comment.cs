using System;

namespace CommentsService.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}