using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class PrimaryTag:BaseTag
    {
        public int PrimaryTagId { get; set; }
        public Post? Post {get; set;}
    }
}