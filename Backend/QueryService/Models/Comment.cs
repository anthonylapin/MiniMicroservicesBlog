using System;

namespace QueryService.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}