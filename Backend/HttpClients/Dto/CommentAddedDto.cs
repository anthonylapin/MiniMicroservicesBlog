using System;

namespace HttpClients.Dto
{
    public class CommentAddedDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}