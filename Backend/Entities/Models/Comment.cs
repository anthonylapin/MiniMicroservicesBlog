using System;
using System.Text.Json.Serialization;
using Entities.Enum;

namespace Entities.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        
        public string Content { get; set; }
        
        public int PostId { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CommentStatuses CommentStatus { get; set; }
    }
}