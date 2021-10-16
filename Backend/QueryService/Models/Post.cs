﻿using System.Collections.Generic;

namespace QueryService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}